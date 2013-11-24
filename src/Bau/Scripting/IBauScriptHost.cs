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

        void Target(string name, Action action);

        void Target(string name, Action<Target> action);

        void Target(string name, string[] prerequisites);
        
        void Target(string name, string[] prerequisites, Action action);

        void Target(string name, string[] prerequisites, Action<Target> action);

        void Exec(string name, Action<Exec> action);

        void Exec(string name, string[] prerequisites, Action<Exec> action);
    }
}
