// <copyright file="Pack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Exec
{
    using System;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public class Pack : ScriptPack<BauExec>
    {
        public override void Initialize(IScriptPackSession session)
        {
            session.ImportNamespace(this.GetType().Namespace);
            this.Context = new BauExec();
        }
    }

    public class BauExec : IScriptPackContext
    {
    }
}
