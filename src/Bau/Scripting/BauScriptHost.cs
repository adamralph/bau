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

        public void Target<TTarget>(string name) where TTarget : Target, new()
        {
            this.application.DefineTarget(name, null, default(Action<Target>));
        }

        public void Target<TTarget>(string name, Action action) where TTarget : Target, new()
        {
            this.application.DefineTarget(name, null, (TTarget target) => action());
        }

        public void Target<TTarget>(string name, Action<TTarget> action) where TTarget : Target, new()
        {
            this.application.DefineTarget(name, null, action);
        }

        public void Target<TTarget>(string name, string[] prerequisites) where TTarget : Target, new()
        {
            this.application.DefineTarget(name, prerequisites, default(Action<Target>));
        }

        public void Target<TTarget>(string name, string[] prerequisites, Action action) where TTarget : Target, new()
        {
            this.application.DefineTarget(name, prerequisites, (Target target) => action());
        }

        public void Target<TTarget>(string name, string[] prerequisites, Action<TTarget> action) where TTarget : Target, new()
        {
            this.application.DefineTarget(name, prerequisites, action);
        }

        public void Target(string name)
        {
            this.Target<Target>(name);
        }

        public void Target(string name, Action action)
        {
            this.Target<Target>(name, action);
        }

        public void Target(string name, Action<Target> action)
        {
            this.Target<Target>(name, action);
        }

        public void Target(string name, string[] prerequisites)
        {
            this.Target<Target>(name, prerequisites);
        }

        public void Target(string name, string[] prerequisites, Action action)
        {
            this.Target<Target>(name, prerequisites, action);
        }

        public void Target(string name, string[] prerequisites, Action<Target> action)
        {
            this.Target<Target>(name, prerequisites, action);
        }

        public void Exec(string name)
        {
            this.Target<Exec>(name);
        }

        public void Exec(string name, Action action)
        {
            this.Target<Exec>(name, action);
        }

        public void Exec(string name, Action<Exec> action)
        {
            this.Target<Exec>(name, action);
        }

        public void Exec(string name, string[] prerequisites)
        {
            this.Target<Exec>(name, prerequisites);
        }

        public void Exec(string name, string[] prerequisites, Action action)
        {
            this.Target<Exec>(name, prerequisites, action);
        }

        public void Exec(string name, string[] prerequisites, Action<Exec> action)
        {
            this.Target<Exec>(name, prerequisites, action);
        }

        public void MSBuild(string name)
        {
            this.Target<MSBuild>(name);
        }

        public void MSBuild(string name, Action action)
        {
            this.Target<MSBuild>(name, action);
        }

        public void MSBuild(string name, Action<MSBuild> action)
        {
            this.Target<MSBuild>(name, action);
        }

        public void MSBuild(string name, string[] prerequisites)
        {
            this.Target<MSBuild>(name, prerequisites);
        }

        public void MSBuild(string name, string[] prerequisites, Action action)
        {
            this.Target<MSBuild>(name, prerequisites, action);
        }

        public void MSBuild(string name, string[] prerequisites, Action<MSBuild> action)
        {
            this.Target<MSBuild>(name, prerequisites, action);
        }
    }
}
