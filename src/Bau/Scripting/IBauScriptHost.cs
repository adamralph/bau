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

        void Target<TTarget>(string name) where TTarget : Target, new();

        void Target<TTarget>(string name, Action action) where TTarget : Target, new();

        void Target<TTarget>(string name, Action<TTarget> action) where TTarget : Target, new();

        void Target<TTarget>(string name, string[] prerequisites) where TTarget : Target, new();

        void Target<TTarget>(string name, string[] prerequisites, Action action) where TTarget : Target, new();

        void Target<TTarget>(string name, string[] prerequisites, Action<TTarget> action) where TTarget : Target, new();

        void Target(string name);

        void Target(string name, Action action);

        void Target(string name, Action<Target> action);

        void Target(string name, string[] prerequisites);

        void Target(string name, string[] prerequisites, Action action);

        void Target(string name, string[] prerequisites, Action<Target> action);

        void Exec(string name);

        void Exec(string name, Action action);

        void Exec(string name, Action<Exec> action);

        void Exec(string name, string[] prerequisites);

        void Exec(string name, string[] prerequisites, Action action);

        void Exec(string name, string[] prerequisites, Action<Exec> action);

        void MSBuild(string name);

        void MSBuild(string name, Action action);

        void MSBuild(string name, Action<MSBuild> action);

        void MSBuild(string name, string[] prerequisites);

        void MSBuild(string name, string[] prerequisites, Action action);

        void MSBuild(string name, string[] prerequisites, Action<MSBuild> action);
    }
}
