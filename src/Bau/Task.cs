// <copyright file="Task.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class Task
    {
        private readonly List<string> dependencies = new List<string>();
        private readonly List<Action> actions = new List<Action>();

        private string name;
        private bool invoked;

        public string Name
        {
            get
            {
                return this.name;
            }

            internal set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    var message = string.Format(CultureInfo.InvariantCulture, "Invalid task name '{0}'.", value);
                    throw new ArgumentException(message, "value");
                }

                this.name = value;
            }
        }

        public IList<string> Dependencies
        {
            get { return this.dependencies; }
        }

        public IList<Action> Actions
        {
            get { return this.actions; }
        }

        // TODO (adamralph): lift this into BauPack and add a settable Invoked property, make Execute public
        public virtual void Invoke(BauPack bau)
        {
            Guard.AgainstNullArgument("bau", bau);

            ////var trace = this.alreadyInvoked ? null : " (first time)";
            ////log.TraceFormat(CultureInfo.InvariantCulture, "Invoking '{0}'{1}.", this.Name, trace);
            if (this.invoked)
            {
                ////log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", this.Name);
                return;
            }

            this.invoked = true;
            foreach (var dependency in this.dependencies.Select(name => bau.GetTask(name)))
            {
                dependency.Invoke(bau);
            }

            try
            {
                this.Execute();
            }
            catch (Exception ex)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "'{0}' task failed. {1}", this.name, ex.Message);
                throw new InvalidOperationException(message, ex);
            }
        }

        protected virtual void Execute()
        {
            Console.WriteLine("Executing '{0}' Bau task.", this.Name);
            foreach (var action in this.actions)
            {
                action();
            }
        }
    }
}
