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
        private readonly List<string> prerequisites = new List<string>();
        private readonly List<Action> actions = new List<Action>();

        private string name;
        private bool alreadyInvoked;

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

        public IList<Action> Actions
        {
            get { return this.actions; }
        }

        public Task DependsOn(params string[] tasks)
        {
            foreach (var task in tasks.Where(p => !this.prerequisites.Contains(p)))
            {
                if (string.IsNullOrWhiteSpace(task))
                {
                    var message = string.Format(CultureInfo.InvariantCulture, "Invalid task name '{0}'.", task);
                    throw new ArgumentException(message, "tasks");
                }
                
                this.prerequisites.Add(task);
            }

            return this;
        }

        public Task Do(Action action)
        {
            if (action != null)
            {
                this.actions.Add(action);
            }

            return this;
        }

        public virtual void Invoke(BauPack application)
        {
            Guard.AgainstNullArgument("application", application);

            ////var trace = this.alreadyInvoked ? null : " (first time)";
            ////log.TraceFormat(CultureInfo.InvariantCulture, "Invoking '{0}'{1}.", this.Name, trace);
            if (this.alreadyInvoked)
            {
                ////log.TraceFormat(CultureInfo.InvariantCulture, "Already invoked '{0}'. Ignoring invocation.", this.Name);
                return;
            }

            this.alreadyInvoked = true;
            foreach (var prerequisite in this.prerequisites.Select(name => application.GetTask(name)))
            {
                prerequisite.Invoke(application);
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
