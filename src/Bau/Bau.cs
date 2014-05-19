// <copyright file="Bau.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using ScriptCs.Contracts;

    public class Bau : IScriptPackContext, ITaskBuilder
    {
        public const string DefaultTask = "default";

        private readonly List<string> topLevelTasks = new List<string>();
        private readonly Dictionary<string, IBauTask> tasks = new Dictionary<string, IBauTask>();
        private IBauTask currentTask;

        public Bau(params string[] topLevelTasks)
        {
            this.topLevelTasks.AddRange(topLevelTasks);
            if (this.topLevelTasks.Count == 0)
            {
                this.topLevelTasks.Add(Bau.DefaultTask);
            }
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
                    BauConsole.WriteInvalidTaskName(task);
                    var message = string.Format(CultureInfo.InvariantCulture, "Invalid task name '{0}'.", task);
                    throw new ArgumentException(message, "otherTasks");
                }

                this.currentTask.Dependencies.Add(task);
            }

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

            ////var trace = this.alreadyInvoked ? null : " (first time)";
            ////log.TraceFormat(CultureInfo.InvariantCulture, "Invoking '{0}'{1}.", this.Name, trace);
            if (taskRef.Invoked)
            {
                ////log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", this.Name);
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
            BauConsole.WriteHeader();
            foreach (var task in this.topLevelTasks)
            {
                this.Invoke(task);
            }
        }

        [Obsolete("Use Run() instead.")]
        public void Execute()
        {
            BauConsole.WriteExecuteDeprecated();
            this.Run();
        }

        public ITaskBuilder Intern<TTask>(string name = Bau.DefaultTask) where TTask : class, IBauTask, new()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                BauConsole.WriteInvalidTaskName(name);
                var message = string.Format(CultureInfo.InvariantCulture, "Invalid task name '{0}'.", name);
                throw new ArgumentException(message, "name");
            }

            IBauTask task;
            if (!this.tasks.TryGetValue(name, out task))
            {
                this.tasks.Add(name, task = new TTask());
            }

            var typedTask = task as TTask;
            if (typedTask == null)
            {
                BauConsole.WriteTasksAlreadyExists(name, task.GetType().Name);
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "'{0}' task already exists with type '{1}'.",
                    name,
                    task.GetType().Name);

                throw new InvalidOperationException(message);
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
            BauConsole.WriteTaskStarting(task);

            try
            {
                taskRef.Execute();
            }
            catch (Exception ex)
            {
                BauConsole.WriteTaskFailed(task, stopwatch.Elapsed.TotalMilliseconds, ex.Message);
                var message = string.Format(CultureInfo.InvariantCulture, "'{0}' task failed. {1}", task, ex.Message);
                throw new InvalidOperationException(message, ex);
            }

            BauConsole.WriteTaskFinished(task, stopwatch.Elapsed.TotalMilliseconds);
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
                return this.tasks[task];
            }
            catch (KeyNotFoundException ex)
            {
                BauConsole.WriteTaskNotFound(task);
                var message = string.Format(CultureInfo.InvariantCulture, "'{0}' task not found.", task);
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
