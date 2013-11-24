// <copyright file="BauTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Common.Logging;

    public class BauTask
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<string, BauTask> tasks = new Dictionary<string, BauTask>();
        private static string nextTaskDescription;

        private readonly List<string> prerequisites = new List<string>();
        private readonly List<object> actions = new List<object>();
        private string name;
        private string description;
        private bool alreadyInvoked;

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The name is invalid.", "value");
                }

                this.name = value;
            }
        }

        public string Description
        {
            get { return this.description; }
        }

        public static void DescribeNextTask(string description)
        {
            if (description == null)
            {
                return;
            }

            nextTaskDescription = description.Trim();
        }

        public static void DefineTask<TTask>(string name, string[] prerequisites, Action<TTask> action)
            where TTask : BauTask, new()
        {
            var task = Intern<TTask>(name);
            if (prerequisites != null)
            {
                task.prerequisites.AddRange(prerequisites.Where(p => p != null));
            }

            if (action != null)
            {
                task.actions.Add(action);
            }
        }

        public static void InvokeTasks(params string[] names)
        {
            if (names.Length == 0)
            {
                GetTask("default").Invoke();
            }
            else
            {
                foreach (var task in names.Select(name => GetTask(name)))
                {
                    task.Invoke();
                }
            }
        }

        public void Invoke()
        {
            log.TraceFormat(CultureInfo.InvariantCulture, "Invoke '{0}'.", this.Name);
            if (this.alreadyInvoked)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", this.Name);
                return;
            }

            this.alreadyInvoked = true;
            foreach (var task in this.prerequisites.Select(name => GetTask(name)))
            {
                task.Invoke();
            }

            this.Execute();
        }

        public void Execute()
        {
            log.TraceFormat(CultureInfo.InvariantCulture, "Execute '{0}'.", this.Name);
            foreach (var action in this.actions)
            {
                this.Call(action);
            }
        }

        protected virtual void Call(object action)
        {
            ((Action<BauTask>)action)(this);
        }

        private static TTask Intern<TTask>(string name)
            where TTask : BauTask, new()
        {
            BauTask task;
            if (!tasks.TryGetValue(name, out task))
            {
                tasks.Add(name, task = new TTask() { Name = name });
            }

            var typedTask = task as TTask;
            if (typedTask == null)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "The task has already been created with type '{0}'.",
                    task.GetType().Name);

                throw new InvalidOperationException(message);
            }

            typedTask.description = nextTaskDescription ?? task.description;
            nextTaskDescription = null;
            return typedTask;
        }

        private static BauTask GetTask(string name)
        {
            BauTask task;
            if (!tasks.TryGetValue(name, out task))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Don't know how to build task '{0}'", name);
                throw new InvalidOperationException(message);
            }

            return task;
        }
    }
}
