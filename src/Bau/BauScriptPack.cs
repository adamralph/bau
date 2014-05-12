// <copyright file="BauScriptPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using ScriptCs.Contracts;

    public class BauScriptPack : ScriptPack<Bau>
    {
        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.ImportNamespace(this.GetType().Namespace);
            this.Context = new Bau(session.ScriptArgs ?? new string[0]);
        }
    }
}
