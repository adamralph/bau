// <copyright file="Task.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ScriptCs.Contracts;

    public class Task : ScriptPack<Task>, IScriptPackContext
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

        public virtual void Invoke(Bau bau)
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

        public virtual void Execute()
        {
            Console.WriteLine("Executing '{0}' Bau task.", this.Name);
            foreach (var action in this.actions)
            {
                action();
            }

            this.OnActionsExecuted();
        }

        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            base.Initialize(session);
            session.ImportNamespace(this.GetType().Namespace);
        }

        public override IScriptPackContext GetContext()
        {
            return this;
        }

        protected virtual void OnActionsExecuted()
        {
        }
    }
}
