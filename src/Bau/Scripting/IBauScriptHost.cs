// <copyright file="IBauScriptHost.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Scripting
{
    using System;
    using ScriptCs;

    [CLSCompliant(false)]
    public interface IBauScriptHost : IScriptHost
    {
        void Desc(string description);

        void Task(string name, Action action);

        void Task(string name, string[] prerequisites);
        
        void Task(string name, string[] prerequisites, Action action);
    }
}
