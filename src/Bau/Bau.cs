// <copyright file="Bau.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using ScriptCs.Contracts;

    public class Bau : IScriptPackContext, ITaskBuilder
    {
        private const string DefaultTask = "default";

        private readonly List<string> topLevelTasks = new List<string>();
        private readonly TaskListType? taskListType;
        private readonly bool help;
        private readonly Dictionary<string, IBauTask> tasks = new Dictionary<string, IBauTask>();
        private IBauTask currentTask;

        public Bau(Arguments arguments)
        {
            Guard.AgainstNullArgument("arguments", arguments);

            this.topLevelTasks.AddRange(arguments.Tasks);
            if (this.topLevelTasks.Count == 0)
            {
                this.topLevelTasks.Add(DefaultTask);
            }

            Log.LogLevel = arguments.LogLevel;
            this.taskListType = arguments.TaskListType;
            this.help = arguments.Help;
        }

        public IBauTask CurrentTask
        {
            get { return this.currentTask; }
        }

        public ITaskBuilder DependsOn(params string[] otherTasks)
        {
            this.EnsureCurrentTask();
            foreach (var task in otherTasks.Where(t => !this.currentTask.Dependencies.Contains(t)))
            {
                if (string.IsNullOrWhiteSpace(task))
                {
                    var message = new ColorText(
                        "Invalid task name '", new ColorToken(task, Log.TaskColor), "'.");

                    Log.Error(message);
                    throw new ArgumentException(message.ToString(), "otherTasks");
                }

                this.currentTask.Dependencies.Add(task);
            }

            return this;
        }

        public ITaskBuilder WithAliases(params string[] aliases)
        {
            this.EnsureCurrentTask();
            foreach (var alias in aliases.Where(a => !this.currentTask.Aliases.Contains(a)))
            {
                if (string.IsNullOrWhiteSpace(alias))
                {
                    var message = new ColorText(
                        "Invalid alias name '", new ColorToken(alias, Log.TaskColor), "'.");

                    Log.Error(message);
                    throw new ArgumentException(message.ToString(), "aliases");
                }

                var existingTaskWithAlias = this.tasks.Values.FirstOrDefault(task => task.Aliases.Contains(alias));
                if (existingTaskWithAlias != null)
                {
                    var message = string.Format(
                        "The 'nd' task alias cannot be used for '{0}' because it has already been used for '{1}'.",
                        this.currentTask.Name,
                        existingTaskWithAlias.Name);

                    throw new ArgumentException(message, "aliases");
                }

                this.currentTask.Aliases.Add(alias);
            }

            return this;
        }

        public ITaskBuilder Desc(string description)
        {
            this.EnsureCurrentTask();
            this.currentTask.Description = description;
            return this;
        }

        public ITaskBuilder Do(Action action)
        {
            this.EnsureCurrentTask();
            if (action != null)
            {
                this.currentTask.Actions.Add(action);
            }

            return this;
        }

        public void Invoke(string task)
        {
            var taskRef = this.GetTask(task);

            var taskTokens = GetTaskTokens(task, taskRef);
            var suffix = taskRef.Invoked ? null : " (first time)";
            Log.Trace(new ColorText(
                new ColorToken[] { "Invoking " }.Concat(taskTokens).Concat(new ColorToken[] { suffix, "." }).ToArray()));

            if (taskRef.Invoked)
            {
                Log.Trace(new ColorText(
                    new ColorToken[] { "Already invoked " }
                        .Concat(taskTokens)
                        .Concat(new ColorToken[] { ". Ignoring invocation." })
                        .ToArray()));

                return;
            }

            taskRef.Invoked = true;
            foreach (var dependency in taskRef.Dependencies)
            {
                this.Invoke(dependency);
            }

            Execute(task, taskRef);
        }

        public void Execute(string task)
        {
            Execute(task, this.GetTask(task));
        }

        public void Run()
        {
            var version = (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).Single();

            var header = new ColorText(
                new ColorToken("Bau", ConsoleColor.White),
                new ColorToken(" " + version.InformationalVersion, ConsoleColor.Gray),
                new ColorToken(" Copyright (c) Bau contributors (baubuildch@gmail.com)", ConsoleColor.DarkGray));

            if (this.help)
            {
                Arguments.ShowUsage(header);
                return;
            }

            if (this.taskListType.HasValue)
            {
                var taskListWriter = new TaskListWriter(this.taskListType.Value);
                foreach (var line in taskListWriter.CreateTaskListLines(this.tasks.Values))
                {
                    ColorConsole.WriteLine(line);
                }

                return;
            }

            Log.Info(header);
            var tasksText = this.topLevelTasks.Skip(1).Aggregate(
                new ColorText("'", new ColorToken(this.topLevelTasks[0], Log.TaskColor), "'"),
                (current, task) => current.Concat(new ColorText(", '", new ColorToken(task, Log.TaskColor), "'")));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Log.Info(new ColorText("Running ").Concat(tasksText).Concat(" and dependencies..."));
            foreach (var task in this.topLevelTasks)
            {
                this.Invoke(task);
            }

            Log.Info(new ColorText("Completed ")
                .Concat(tasksText)
                .Concat(new ColorText(
                    " and dependencies in ",
                    new ColorToken(stopwatch.Elapsed.TotalMilliseconds.ToStringFromMilliseconds(), ConsoleColor.DarkYellow),
                    ".")));
        }

        [Obsolete("Use Run() instead.")]
        public void Execute()
        {
            Log.Warn(new ColorText(
                new ColorToken("Bau.Execute() ", ConsoleColor.DarkGreen),
                "(with no parameters) will be removed shortly. Use ",
                new ColorToken("Bau.Run() ", ConsoleColor.DarkGreen),
                "instead."));

            this.Run();
        }

        public ITaskBuilder Intern<TTask>(string name = null) where TTask : class, IBauTask, new()
        {
            name = name ?? DefaultTask;
            if (string.IsNullOrWhiteSpace(name))
            {
                var message = new ColorText(
                    "Invalid task name '",
                    new ColorToken(name, Log.TaskColor),
                    "'.");

                Log.Error(message);
                throw new ArgumentException(message.ToString(), "name");
            }

            IBauTask task;
            if (!this.tasks.TryGetValue(name, out task))
            {
                this.tasks.Add(name, task = new TTask { Name = name });
            }

            var typedTask = task as TTask;
            if (typedTask == null)
            {
                var message = new ColorText(
                    "'",
                    new ColorToken(name, Log.TaskColor),
                    "' task already exists with type '",
                    new ColorToken(task.GetType().Name, ConsoleColor.DarkGreen),
                    "'.");

                Log.Error(message);
                throw new InvalidOperationException(message.ToString());
            }

            this.currentTask = typedTask;
            return this;
        }

        public void Reenable(string task)
        {
            this.GetTask(task).Invoked = false;
        }

        private static void Execute(string task, IBauTask taskRef)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var taskTokens = GetTaskTokens(task, taskRef);
            Log.Info(new ColorText(
                new ColorToken[] { "Starting " }.Concat(taskTokens).Concat(new ColorToken[] { "..." }).ToArray()));

            try
            {
                taskRef.Execute();
            }
            catch (Exception ex)
            {
                var message = new ColorText(
                    taskTokens
                        .Concat(new ColorToken[] { " task failed. ", new ColorToken(ex.Message, ConsoleColor.DarkRed) })
                        .ToArray());

                Log.Error(message);
                throw new InvalidOperationException(message.ToString());
            }

            Log.Info(new ColorText(
                new ColorToken[] { "Finished " }
                    .Concat(taskTokens)
                    .Concat(new ColorToken[]
                        {
                            " after ",
                            new ColorToken(stopwatch.Elapsed.TotalMilliseconds.ToStringFromMilliseconds(), ConsoleColor.DarkYellow),
                            "."
                        })
                    .ToArray()));
        }

        private static ColorToken[] GetTaskTokens(string task, IBauTask taskRef)
        {
            return task == taskRef.Name
                ? new ColorToken[]
                    {
                        "'",
                        new ColorToken(task, Log.TaskColor),
                        "'",
                    }
                : new ColorToken[]
                    {
                        "'",
                        new ColorToken(task, Log.TaskColor),
                        "'",
                        " ('",
                        new ColorToken(taskRef.Name, Log.TaskColor),
                        "')",
                    };
        }

        private void EnsureCurrentTask()
        {
            if (this.currentTask == null)
            {
                this.Intern<BauTask>();
            }
        }

        private IBauTask GetTask(string task)
        {
            try
            {
                return this.tasks.Values.FirstOrDefault(v => v.Aliases.Contains(task)) ?? this.tasks[task];
            }
            catch (KeyNotFoundException ex)
            {
                var message = new ColorText(
                    "'",
                    new ColorToken(task, Log.TaskColor),
                    "' task not found.");

                Log.Error(message);
                throw new InvalidOperationException(message.ToString(), ex);
            }
        }
    }
}
