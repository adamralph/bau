// <copyright file="BauScriptExecutor.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public sealed class BauScriptExecutor : ScriptExecutor, IDisposable
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly ILog scriptCsLog = LogManager.GetLogger("ScriptCs");
        private bool isInitialized;

        public BauScriptExecutor(Application application, IFileSystem fileSystem)
            : base(
                fileSystem,
                new FilePreProcessor(fileSystem, scriptCsLog, new ILineProcessor[] { new LoadLineProcessor(fileSystem) }),
                new BauScriptEngine(new BauScriptHostFactory(application), scriptCsLog),
                scriptCsLog)
        {
        }

        public override void Initialize(IEnumerable<string> paths, IEnumerable<IScriptPack> scriptPacks, params string[] scriptArgs)
        {
            base.Initialize(paths, scriptPacks, scriptArgs);
            this.ScriptEngine.BaseDirectory = this.FileSystem.CurrentDirectory; // NOTE (adamralph): set to bin subfolder in base.Initialize()!
            this.isInitialized = true;
        }

        public override ScriptResult ExecuteScript(string script, params string[] scriptArgs)
        {
            var result = base.ExecuteScript(script, scriptArgs);
            RethrowExceptionIfAny(result, script);
            return result;
        }

        public override ScriptResult Execute(string script, string[] scriptArgs)
        {
            var result = base.Execute(script, scriptArgs);
            RethrowExceptionIfAny(result, script);
            return result;
        }

        public void Dispose()
        {
            if (this.isInitialized)
            {
                this.Terminate();
                this.isInitialized = false;
            }
        }

        private static void RethrowExceptionIfAny(ScriptResult result, string script)
        {
            if (result.CompileExceptionInfo != null)
            {
                result.CompileExceptionInfo.Throw();
            }

            if (result.ExecuteExceptionInfo != null)
            {
                result.ExecuteExceptionInfo.Throw();
            }
        }
    }
}
