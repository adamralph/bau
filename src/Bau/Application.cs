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
        private static readonly Dictionary<string, Target> targets = new Dictionary<string, Target>();
        private static string nextTargetDescription;

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

        public static void InvokeTargets(params string[] names)
        {
            if (names.Length == 0)
            {
                Invoke(GetTarget("default"));
            }
            else
            {
                foreach (var target in names.Select(name => GetTarget(name)))
                {
                    Invoke(target);
                }
            }
        }

        private static void Invoke(Target target)
        {
            log.TraceFormat(CultureInfo.InvariantCulture, "Invoke '{0}'.", target.Name);
            if (target.AlreadyInvoked)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", target.Name);
                return;
            }

            target.AlreadyInvoked = true;
            foreach (var prerequisite in target.Prerequisites.Select(name => GetTarget(name)))
            {
                Invoke(prerequisite);
            }

            target.Execute();
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

            typedTarget.Description = nextTargetDescription ?? target.Description;
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
