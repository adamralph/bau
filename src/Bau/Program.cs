// <copyright file="Program.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Bau.Scripting;
    using CommandLine;
    using Common.Logging;
    using Common.Logging.Simple;
    using ScriptCs;
    using ScriptCs.Contracts;
    using ServiceStack.Text;

    internal static class Program
    {
        public static int Main(string[] args)
        {
            Guard.AgainstNullArgument("args", args);

            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(Common.Logging.LogLevel.Trace, true, true, true, "u");
            var log = LogManager.GetCurrentClassLogger();
            log.TraceFormat(CultureInfo.InvariantCulture, "Arguments: {0}", args.ToJsv());

            var arguments = new Arguments();
            if (!Parser.Default.ParseArguments(args, arguments))
            {
                return 1;
            }

            log.TraceFormat(CultureInfo.InvariantCulture, "Parsed arguments: {0}", arguments.ToJsv());

            // TODO (adamralph): 
            ////'rakefile',
            ////'Rakefile',
            ////'rakefile.rb',
            ////'Rakefile.rb'
            var filename = "baufile.csx";
            while (!File.Exists(filename))
            {
                var directory = Directory.GetCurrentDirectory();
                log.TraceFormat(CultureInfo.InvariantCulture, "No Baufile found in directory '{0}'.", directory);

                var parent = Directory.GetParent(directory);
                if (parent == null)
                {
                    throw new InvalidOperationException("No Baufile found (looking for baufile.csx).");
                }

                Directory.SetCurrentDirectory(parent.FullName);
            }

            log.TraceFormat(CultureInfo.InvariantCulture, "The current directory is '{0}'.", Directory.GetCurrentDirectory());
            log.DebugFormat(CultureInfo.InvariantCulture, "Executing '{0}'...", Path.GetFullPath(filename));

            var application = new Application();

            // TODO (adamralph): move out for easily swappable scripting solutions
            var fileSystem = new FileSystem { CurrentDirectory = Directory.GetCurrentDirectory() };
            using (var executor = new BauScriptExecutor(application, fileSystem))
            {
                executor.AddReferenceAndImportNamespaces(new[] { typeof(Target) });
                executor.Initialize(new string[0], new IScriptPack[0]);
                executor.Execute(filename);
            }

            if (arguments.TargetNames.Count == 0)
            {
                arguments.TargetNames.Add("default");
            }

            foreach (var target in arguments.TargetNames.Select(name => application.GetTarget(name)))
            {
                target.Invoke(application);
            }

            return 0;
        }
    }
}
