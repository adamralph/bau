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

        private readonly string name;
        private readonly List<string> prerequisites = new List<string>();
        private readonly List<Action> actions = new List<Action>();
        private string description;
        private bool alreadyInvoked;

        public BauTask(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name is invalid.");
            }

            this.name = name;
        }

        public static void DescribeNextTask(string description)
        {
            if (description == null)
            {
                return;
            }

            nextTaskDescription = description.Trim();
        }

        public static void DefineTask(string name, Action action)
        {
            var task = Intern(name);
            if (action != null)
            {
                task.actions.Add(action);
            }
        }

        public static void DefineTask(string name, string[] prerequisites)
        {
            Guard.AgainstNullArgument("prerequisites", prerequisites);

            var task = Intern(name);
            task.prerequisites.AddRange(prerequisites.Where(p => p != null));
        }

        public static void DefineTask(string name, string[] prerequisites, Action action)
        {
            Guard.AgainstNullArgument("prerequisites", prerequisites);

            var task = Intern(name);
            task.prerequisites.AddRange(prerequisites.Where(p => p != null));
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
            log.TraceFormat(CultureInfo.InvariantCulture, "Invoke '{0}'.", this.name);
            if (this.alreadyInvoked)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", this.name);
                return;
            }

            this.alreadyInvoked = true;
            foreach (var task in this.prerequisites.Select(name => GetTask(name)))
            {
                task.Invoke();
            }

            foreach (var action in this.actions)
            {
                action.Invoke();
            }
        }

        private static BauTask Intern(string name)
        {
            BauTask task;
            if (!tasks.TryGetValue(name, out task))
            {
                tasks.Add(name, task = new BauTask(name));
            }

            task.description = nextTaskDescription ?? task.description;
            nextTaskDescription = null;
            return task;
        }

        private static BauTask GetTask(string name)
        {
            BauTask task;
            if (!tasks.TryGetValue(name, out task))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Don't know how to build task '{0}'", name));
            }

            return task;
        }
    }
}
