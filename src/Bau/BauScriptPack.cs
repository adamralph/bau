// <copyright file="BauScriptPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Linq;
    using ScriptCs.Contracts;

    public class BauScriptPack : ScriptPack<Bau>
    {
        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.ImportNamespace(this.GetType().Namespace);
            Arguments arguments;
            try
            {
                arguments = Arguments.Parse(session.ScriptArgs ?? Enumerable.Empty<string>());
            }
            catch (Exception)
            {
                Arguments.ShowUsage();
                throw;
            }

            this.Context = new Bau(arguments);
        }
    }
}
