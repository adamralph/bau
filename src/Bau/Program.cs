// <copyright file="Program.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Globalization;
    using System.IO;
    using Bau.Scripting;
    using Common.Logging;
    using Common.Logging.Simple;
    using ScriptCs;
    using ScriptCs.Contracts;

    internal static class Program
    {
        public static int Main(string[] args)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(Common.Logging.LogLevel.Trace, true, true, true, "u");

            var log = LogManager.GetCurrentClassLogger();

            var filename = "baufile.csx";
            var directory = Directory.GetCurrentDirectory();
            while (!File.Exists(filename))
            {
                var parent = Directory.GetParent(directory);
                if (parent == null)
                {
                    throw new InvalidOperationException("baufile.csx not found.");
                }

                Directory.SetCurrentDirectory(parent.FullName);
            }

            var fileSystem = new FileSystem { CurrentDirectory = Directory.GetCurrentDirectory() };

            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", fileSystem.CurrentDirectory);
            log.DebugFormat(CultureInfo.InvariantCulture, "Executing '{0}'", fileSystem.GetFullPath(filename));
            using (var executor = new BauScriptExecutor(fileSystem))
            {
                executor.AddReferenceAndImportNamespaces(new[] { typeof(Program) });
                executor.Initialize(new string[0], new IScriptPack[0]);
                executor.Execute(filename);
            }

            Target.InvokeTargets(args);
            return 0;
        }
    }
}
