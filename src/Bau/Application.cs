// <copyright file="Application.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Common.Logging;

    public class Application
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly Arguments arguments;
        private readonly List<string> topLevelTargets = new List<string>();
        private readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();

        private string nextTargetDescription;
        private bool nextTargetDescribed;

        public Application(Arguments arguments)
        {
            Guard.AgainstNullArgument("arguments", arguments);
            Guard.AgainstNullArgumentProperty("arguments", "Targets", arguments.Targets);

            this.arguments = arguments;
            this.topLevelTargets.AddRange(arguments.Targets);
            if (this.topLevelTargets.Count == 0)
            {
                this.topLevelTargets.Add("default");
            }
        }

        public void DescribeNextTarget(string description)
        {
            this.nextTargetDescription = description;
            this.nextTargetDescribed = true;
        }

        public void DefineTarget<TTarget>(string name, string[] prerequisites, Action<TTarget> action)
            where TTarget : Target, new()
        {
            var target = this.Intern<TTarget>(name);
            if (prerequisites != null)
            {
                foreach (var prerequisite in prerequisites.Where(p => !target.Prerequisites.Contains(p)))
                {
                    target.Prerequisites.Add(prerequisite);
                }
            }

            if (action != null)
            {
                target.Actions.Add(() => action(target));
            }
        }

        public void Execute()
        {
            if (this.arguments.DisplayTargets)
            {
                this.DisplayTargets();
            }
            else if (this.arguments.DisplayPrerequisites)
            {
                this.DisplayPrerequisites();
            }
            else
            {
                foreach (var target in this.topLevelTargets.Select(name => this.GetTarget(name)))
                {
                    target.Invoke(this);
                }
            }
        }

        public Target GetTarget(string name)
        {
            Target target;
            if (!this.targets.TryGetValue(name, out target))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "'{0}' target not found.", name);
                throw new InvalidOperationException(message);
            }

            return target;
        }

        private TTarget Intern<TTarget>(string name) where TTarget : Target, new()
        {
            Target target;
            if (!this.targets.TryGetValue(name, out target))
            {
                this.targets.Add(name, target = new TTarget() { Name = name });
                if (this.nextTargetDescribed)
                {
                    target.Description = this.nextTargetDescription;
                }
            }

            this.nextTargetDescription = null;
            this.nextTargetDescribed = false;

            var typedTarget = target as TTarget;
            if (typedTarget == null)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "'{0}' target already exists with type '{1}'.",
                    name,
                    target.GetType().Name);

                throw new InvalidOperationException(message);
            }

            return typedTarget;
        }

        private void DisplayTargets()
        {
            var displayableTargets = this.targets.Values
                .Where(target => !string.IsNullOrWhiteSpace(target.Description)).ToArray();

            var maxNameLength = displayableTargets.Max(target => target.Name.Length);
            var maxLength = Math.Max(80, maxNameLength + 8);
            foreach (var target in displayableTargets)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}  // {1}",
                    target.Name.PadRight(maxNameLength, ' '),
                    target.Description);

                Console.WriteLine(message.Truncate(maxLength));
            }
        }

        private void DisplayPrerequisites()
        {
            foreach (var target in this.targets.Values)
            {
                Console.WriteLine("Bau {0}", target.Name);
                foreach (var prerequisite in target.Prerequisites)
                {
                    Console.WriteLine("  " + prerequisite);
                }
            }
        }
    }
}
