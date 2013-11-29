// <copyright file="Target.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Common.Logging;

    // TODO (adamralph): move static stuff into an Application class, which can be instantiated and passed to the script host
    public class Target
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();
        private static string nextTargetDescription;

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

        public static void DescribeNextTarget(string description)
        {
            if (description == null)
            {
                return;
            }

            nextTargetDescription = description.Trim();
        }

        public static void DefineTarget<TTarget>(string name, string[] prerequisites, Action<TTarget> action)
            where TTarget : Target, new()
        {
            var target = Intern<TTarget>(name);
            if (prerequisites != null)
            {
                foreach (var prerequisite in prerequisites.Where(p => !target.prerequisites.Contains(p)))
                {
                    target.prerequisites.Add(prerequisite);
                }
            }

            if (action != null)
            {
                target.actions.Add(action);
            }
        }

        public static void InvokeTargets(params string[] names)
        {
            if (names.Length == 0)
            {
                GetTarget("default").Invoke();
            }
            else
            {
                foreach (var target in names.Select(name => GetTarget(name)))
                {
                    target.Invoke();
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
            foreach (var target in this.prerequisites.Select(name => GetTarget(name)))
            {
                target.Invoke();
            }

            this.Execute();
        }

        protected virtual void Execute()
        {
            log.TraceFormat(CultureInfo.InvariantCulture, "Execute '{0}'.", this.Name);
            foreach (var action in this.actions)
            {
                this.Call(action);
            }
        }

        protected virtual void Call(object action)
        {
            ((Action<Target>)action)(this);
        }

        private static TTarget Intern<TTarget>(string name) where TTarget : Target, new()
        {
            Target target;
            if (!targets.TryGetValue(name, out target))
            {
                targets.Add(name, target = new TTarget() { Name = name });
            }

            var typedTarget = target as TTarget;
            if (typedTarget == null)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "The target has already been created with type '{0}'.",
                    target.GetType().Name);

                throw new InvalidOperationException(message);
            }

            typedTarget.description = nextTargetDescription ?? target.description;
            nextTargetDescription = null;
            return typedTarget;
        }

        private static Target GetTarget(string name)
        {
            Target target;
            if (!targets.TryGetValue(name, out target))
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Don't know how to build target '{0}'", name);
                throw new InvalidOperationException(message);
            }

            return target;
        }
    }
}
