// <copyright file="BauScriptHostFactory.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Scripting
{
    using System;
    using ScriptCs.Contracts;

    public class BauScriptHostFactory : IBauScriptHostFactory
    {
        private readonly Application application;

        public BauScriptHostFactory(Application application)
        {
            this.application = application;
        }

        [CLSCompliant(false)]
        public IBauScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            return new BauScriptHost(this.application, scriptPackManager, scriptArgs);
        }
    }
}
