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

    public class Target
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly List<string> prerequisites = new List<string>();
        private readonly List<Action> actions = new List<Action>();

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
                    throw new ArgumentException("Invalid name.", "value");
                }

                this.name = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid description.", "value");
                }

                this.description = value;
            }
        }

        public IList<string> Prerequisites
        {
            get { return this.prerequisites; }
        }

        public IList<Action> Actions
        {
            get { return this.actions; }
        }

        public virtual void Invoke(Application application)
        {
            Guard.AgainstNullArgument("application", application);

            var trace = this.alreadyInvoked ? null : " (first time)";
            log.TraceFormat(CultureInfo.InvariantCulture, "Invoke '{0}'{1}.", this.Name, trace);
            if (this.alreadyInvoked)
            {
                log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", this.Name);
                return;
            }

            this.alreadyInvoked = true;
            foreach (var prerequisite in this.Prerequisites.Select(name => application.GetTarget(name)))
            {
                prerequisite.Invoke(application);
            }

            this.Execute();
        }

        public virtual void Execute()
        {
            log.DebugFormat(CultureInfo.InvariantCulture, "Execute '{0}'.", this.Name);
            foreach (var action in this.actions)
            {
                action();
            }
        }
    }
}
