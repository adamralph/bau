// <copyright file="ScriptCs.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component.Support
{
    using global::ScriptCs.Contracts;

    public static class ScriptCs
    {
        public static T Require<T>(params string[] scriptArgs) where T : Bau
        {
            var scriptPack = new BauScriptPack();
            scriptPack.Initialize(new ScriptPackSession(scriptArgs));
            return (T)scriptPack.Context;
        }

        public class ScriptPackSession : IScriptPackSession
        {
            private readonly string[] scriptArgs;

            public ScriptPackSession(params string[] scriptArgs)
            {
                this.scriptArgs = scriptArgs;
            }

            public string[] ScriptArgs
            {
                get { return this.scriptArgs; }
            }

            public void AddReference(string assemblyDisplayNameOrPath)
            {
            }

            public void ImportNamespace(string @namespace)
            {
            }
        }
    }
}
