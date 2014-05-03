// <copyright file="BauScriptPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using ScriptCs.Contracts;

    public class BauScriptPack : ScriptPack<BauPack>
    {
        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.ImportNamespace(this.GetType().Namespace);
            this.Context = new BauPack();
        }
    }
}
