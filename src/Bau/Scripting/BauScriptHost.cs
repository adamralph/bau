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
        public BauScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs)
            : base(scriptPackManager, scriptArgs)
        {
        }

        public void Desc(string description)
        {
            Application.DescribeNextTarget(description);
        }

        public void Target(string name, Action action)
        {
            Application.DefineTarget(name, null, (Target target) => action());
        }

        public void Target(string name, Action<Target> action)
        {
            Application.DefineTarget(name, null, action);
        }

        public void Target(string name, string[] prerequisites)
        {
            Application.DefineTarget(name, prerequisites, default(Action<Target>));
        }

        public void Target(string name, string[] prerequisites, Action action)
        {
            Application.DefineTarget(name, prerequisites, (Target target) => action());
        }

        public void Target(string name, string[] prerequisites, Action<Target> action)
        {
            Application.DefineTarget(name, prerequisites, action);
        }

        public void Exec(string name, Action<Exec> action)
        {
            Application.DefineTarget(name, null, action);
        }

        public void Exec(string name, string[] prerequisites, Action<Exec> action)
        {
            Application.DefineTarget(name, prerequisites, action);
        }
    }
}
