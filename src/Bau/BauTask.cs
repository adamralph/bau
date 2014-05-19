// <copyright file="BauTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using ScriptCs.Contracts;

    public class BauTask : ScriptPack<BauTask>, IScriptPackContext, IBauTask
    {
        private readonly List<string> dependencies = new List<string>();
        private readonly List<Action> actions = new List<Action>();

        public IList<string> Dependencies
        {
            get { return this.dependencies; }
        }

        public IList<Action> Actions
        {
            get { return this.actions; }
        }

        public bool Invoked { get; set; }

        public virtual void Execute()
        {
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
