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

        private readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();

        private string nextTargetDescription;
        private bool nextTargetDescribed;

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
                target.Actions.Add(action);
            }
        }

        public void Invoke(Target target)
        {
            log.TraceFormat(CultureInfo.InvariantCulture, "Invoke '{0}'.", target.Name);
            if (target.AlreadyInvoked)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", target.Name);
                return;
            }

            target.AlreadyInvoked = true;
            foreach (var prerequisite in target.Prerequisites.Select(name => this.GetTarget(name)))
            {
                this.Invoke(prerequisite);
            }

            target.Execute();
        }

        public Target GetTarget(string name)
        {
            Target target;
            if (!this.targets.TryGetValue(name, out target))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Target '{0}' not found.", name);
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
                    "The target has already been created with type '{0}'.",
                    target.GetType().Name);

                throw new InvalidOperationException(message);
            }

            return typedTarget;
        }
    }
}
