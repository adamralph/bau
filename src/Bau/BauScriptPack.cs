// <copyright file="BauScriptPack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System.Collections.Generic;
    using ScriptCs.Contracts;

    public class BauScriptPack : ScriptPack<Bau>
    {
        public override void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.ImportNamespace(this.GetType().Namespace);

            IList<string> tasks;
            LogLevel logLevel;
            Parser.Parse(session.ScriptArgs, out tasks, out logLevel);

            this.Context = new Bau(logLevel, tasks);
        }
    }
}
