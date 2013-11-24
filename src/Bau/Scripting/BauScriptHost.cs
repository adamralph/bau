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
            BauTask.DefineTask(name, null, (BauTask task) => action());
        }

        public void Task(string name, Action<BauTask> action)
        {
            BauTask.DefineTask(name, null, action);
        }

        public void Task(string name, string[] prerequisites)
        {
            BauTask.DefineTask(name, prerequisites, default(Action<BauTask>));
        }

        public void Task(string name, string[] prerequisites, Action action)
        {
            BauTask.DefineTask(name, prerequisites, (BauTask task) => action());
        }

        public void Task(string name, string[] prerequisites, Action<BauTask> action)
        {
            BauTask.DefineTask(name, prerequisites, action);
        }

        public void Exec(string name, Action<Exec> action)
        {
            BauTask.DefineTask(name, null, action);
        }

        public void Exec(string name, string[] prerequisites, Action<Exec> action)
        {
            BauTask.DefineTask(name, prerequisites, action);
        }
    }
}
