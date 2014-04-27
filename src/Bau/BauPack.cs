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

        public BauPack(IEnumerable<string> topLevelTaskNames)
        {
            if (topLevelTaskNames != null)
            {
                this.topLevelTaskNames.AddRange(topLevelTaskNames);
            }

            if (this.topLevelTaskNames.Count == 0)
            {
                this.topLevelTaskNames.Add("default");
            }
        }

        public void Task(string name, Action action)
        {
            this.Task(name, (Task task) => action());
        }

        public void Task<TTask>(string name, Action<TTask> action)
            where TTask : Task, new()
        {
            var task = this.Intern<TTask>(name);
            ////if (prerequisites != null)
            ////{
            ////    foreach (var prerequisite in prerequisites.Where(p => !task.Prerequisites.Contains(p)))
            ////    {
            ////        task.Prerequisites.Add(prerequisite);
            ////    }
            ////}

            if (action != null)
            {
                task.Actions.Add(() => action(task));
            }
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

        private TTask Intern<TTask>(string name) where TTask : Task, new()
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
    }
}
