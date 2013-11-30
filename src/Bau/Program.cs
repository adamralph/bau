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

            // TODO (adamralph): 
            ////'rakefile',
            ////'Rakefile',
            ////'rakefile.rb',
            ////'Rakefile.rb'
            var filename = "baufile.csx";
            while (!File.Exists(filename))
            {
                var directory = Directory.GetCurrentDirectory();
                log.TraceFormat(CultureInfo.InvariantCulture, "No baufile found in directory '{0}'.", directory);
                
                var parent = Directory.GetParent(directory);
                if (parent == null)
                {
                    throw new InvalidOperationException("baufile.csx not found.");
                }

                Directory.SetCurrentDirectory(parent.FullName);
            }

            log.TraceFormat(CultureInfo.InvariantCulture, "The current directory is '{0}'.", Directory.GetCurrentDirectory());
            log.DebugFormat(CultureInfo.InvariantCulture, "Executing '{0}'...", Path.GetFullPath(filename));

            // TODO (adamralph): move out for easily swappable scripting solutions
            var fileSystem = new FileSystem { CurrentDirectory = Directory.GetCurrentDirectory() };
            using (var executor = new BauScriptExecutor(fileSystem))
            {
                executor.AddReferenceAndImportNamespaces(new[] { typeof(Target) });
                executor.Initialize(new string[0], new IScriptPack[0]);
                executor.Execute(filename);
            }

            Application.InvokeTargets(args);
            return 0;
        }
    }
}
