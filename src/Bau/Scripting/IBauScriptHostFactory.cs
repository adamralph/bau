// <copyright file="IBauScriptHostFactory.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Scripting
{
    using System;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public interface IBauScriptHostFactory
    {
        IBauScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs);
    }
}
