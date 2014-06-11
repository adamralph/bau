// <copyright file="BauScriptPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System.Linq;
    using ScriptCs.Contracts;

    public class BauScriptPack : ScriptPack<Bau>
    {
        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.ImportNamespace(this.GetType().Namespace);
            var arguments = Arguments.Parse(session.ScriptArgs ?? Enumerable.Empty<string>());
            this.Context = new Bau(arguments.LogLevel, arguments.Tasks);
        }
    }
}
