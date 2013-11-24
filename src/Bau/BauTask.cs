// <copyright file="BauTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class BauTask
    {
        private static readonly Dictionary<string, BauTask> tasks = new Dictionary<string, BauTask>();
        private static string nextTaskDescription;

        private readonly List<string> prerequisites = new List<string>();
        private readonly List<Action> actions = new List<Action>();
        private string description;

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

        public static void ExecuteTasks(params string[] names)
        {
            if (names == null || names.Length == 0)
            {
                names = new[] { "default" };
            }

            foreach (var name in names)
            {
                BauTask task;
                if (!tasks.TryGetValue(name, out task))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Don't know how to build task '{0}'", name));
                }

                task.Execute();
            }
        }

        public void Execute()
        {
            if (this.prerequisites.Any())
            {
                ExecuteTasks(this.prerequisites.ToArray());
            }

            foreach (var action in this.actions)
            {
                action();
            }
        }

        private static BauTask Intern(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name is invalid.");
            }

            BauTask task;
            if (!tasks.TryGetValue(name, out task))
            {
                tasks.Add(name, task = new BauTask());
            }

            task.description = nextTaskDescription ?? task.description;
            nextTaskDescription = null;
            return task;
        }
    }
}
