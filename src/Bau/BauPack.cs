// <copyright file="BauPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using ScriptCs.Contracts;

    public class BauPack : IScriptPackContext
    {
        private readonly List<string> topLevelTaskNames = new List<string>();
        private readonly Dictionary<string, Task> tasks = new Dictionary<string, Task>();
        private Task currentTask;

        public BauPack(IEnumerable<string> topLevelTaskNames)
        {
            Guard.AgainstNullArgument("topLevelTaskNames", topLevelTaskNames);

            this.topLevelTaskNames.AddRange(topLevelTaskNames);
        }

        public BauPack Task(string name)
        {
            this.currentTask = this.Intern<Task>(name);
            return this;
        }

        public BauPack DependsOn(params string[] tasks)
        {
            this.EnsureCurrentTask();
            foreach (var task in tasks.Where(p => !this.currentTask.Prerequisites.Contains(p)))
            {
                if (string.IsNullOrWhiteSpace(task))
                {
                    var message = string.Format(CultureInfo.InvariantCulture, "Invalid task name '{0}'.", task);
                    throw new ArgumentException(message, "tasks");
                }

                this.currentTask.Prerequisites.Add(task);
            }

            return this;
        }

        public BauPack Do(Action action)
        {
            this.EnsureCurrentTask();
            if (action != null)
            {
                this.currentTask.Actions.Add(action);
            }

            return this;
        }

        public void Execute()
        {
            var version = (AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).Single();

            Console.WriteLine("Bau version {0}.", version.InformationalVersion);
            Console.WriteLine("Copyright (c) Bau contributors. (baubuildch@gmail.com)");

            foreach (var task in this.topLevelTaskNames.Select(name => this.GetTask(name)))
            {
                task.Invoke(this);
            }

            Console.WriteLine("Bau succeeded.");
        }

        public Task GetTask(string name)
        {
            Task task;
            if (!this.tasks.TryGetValue(name, out task))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "'{0}' task not found.", name);
                throw new InvalidOperationException(message);
            }

            return task;
        }

        private Task Intern<TTask>(string name) where TTask : Task, new()
        {
            Task task;
            if (!this.tasks.TryGetValue(name, out task))
            {
                this.tasks.Add(name, task = new TTask() { Name = name });
            }

            var typedTask = task as TTask;
            if (typedTask == null)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "'{0}' task already exists with type '{1}'.",
                    name,
                    task.GetType().Name);

                throw new InvalidOperationException(message);
            }

            return typedTask;
        }

        private void EnsureCurrentTask()
        {
            if (this.currentTask == null)
            {
                this.Task("default");
            }
        }
    }
}
