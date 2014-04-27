// <copyright file="BauScriptPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public class BauScriptPack : ScriptPack<BauPack>
    {
        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.ImportNamespace("Bau");
            this.Context = new BauPack(null);
        }

        public override void Terminate()
        {
            this.Context.Execute();
        }
    }
}
