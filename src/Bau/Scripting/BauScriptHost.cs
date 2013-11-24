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
            BauTask.DescribeNextTask(description);
        }

        public void Task(string name, Action action)
        {
            BauTask.DefineTask(name, action);
        }

        public void Task(string name, string[] prerequisites)
        {
            BauTask.DefineTask(name, prerequisites);
        }

        public void Task(string name, string[] prerequisites, Action action)
        {
            BauTask.DefineTask(name, prerequisites, action);
        }
    }
}
