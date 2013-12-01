// <copyright file="Target.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Common.Logging;

    public class Target
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly List<string> prerequisites = new List<string>();
        private readonly List<Action> actions = new List<Action>();
        private string name;

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

        public string Description { get; set; }

        public IList<string> Prerequisites
        {
            get { return this.prerequisites; }
        }

        public IList<Action> Actions
        {
            get { return this.actions; }
        }

        public bool AlreadyInvoked { get; set; }

        public virtual void Execute()
        {
            log.TraceFormat(CultureInfo.InvariantCulture, "Execute '{0}'.", this.Name);
            foreach (var action in this.actions)
            {
                action();
            }
        }
    }
}
