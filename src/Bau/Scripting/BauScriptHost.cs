// <copyright file="BauScriptHost.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Scripting
{
    using System;
    using ScriptCs;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public class BauScriptHost : ScriptHost, IBauScriptHost
    {
        private readonly Application application;

        public BauScriptHost(Application application, IScriptPackManager scriptPackManager, string[] scriptArgs)
            : base(scriptPackManager, scriptArgs)
        {
            Guard.AgainstNullArgument("application", application);

            this.application = application;
        }

        public void Desc(string description)
        {
            this.application.DescribeNextTarget(description);
        }

        public void Target(string name, Action action)
        {
            this.application.DefineTarget(name, null, (Target target) => action());
        }

        public void Target(string name, Action<Target> action)
        {
            this.application.DefineTarget(name, null, action);
        }

        public void Target(string name, string[] prerequisites)
        {
            this.application.DefineTarget(name, prerequisites, default(Action<Target>));
        }

        public void Target(string name, string[] prerequisites, Action action)
        {
            this.application.DefineTarget(name, prerequisites, (Target target) => action());
        }

        public void Target(string name, string[] prerequisites, Action<Target> action)
        {
            this.application.DefineTarget(name, prerequisites, action);
        }

        public void Exec(string name, Action<Exec> action)
        {
            this.application.DefineTarget(name, null, action);
        }

        public void Exec(string name, string[] prerequisites, Action<Exec> action)
        {
            this.application.DefineTarget(name, prerequisites, action);
        }
    }
}
