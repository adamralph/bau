// <copyright file="ApplicationFactory.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System.IO;
    using Bau.Scripting;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;

    internal static class ApplicationFactory
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public static Application Create(string filename)
        {
            var application = new Application();
            var fileSystem = new FileSystem { CurrentDirectory = Path.GetDirectoryName(filename) };
            using (var executor = new BauScriptExecutor(application, fileSystem))
            {
                executor.AddReferenceAndImportNamespaces(new[] { typeof(Target) });
                executor.Initialize(new string[0], new IScriptPack[0]);
                executor.Execute(filename);
            }

            return application;
        }
    }
}
