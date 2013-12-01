// <copyright file="Program.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Globalization;
    using System.Linq;
    using CommandLine;
    using Common.Logging;
    using Common.Logging.Simple;
    using ServiceStack.Text;

    internal static class Program
    {
        public static int Main(string[] args)
        {
            Guard.AgainstNullArgument("args", args);

            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(Common.Logging.LogLevel.Trace, true, true, true, "u");
            var log = LogManager.GetCurrentClassLogger();

            AppDomain.CurrentDomain.FirstChanceException +=
                (object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e) =>
                log.Trace("First chance exception.", e.Exception);

            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
                log.Fatal("Unhandled exception.", (Exception)e.ExceptionObject);

            log.TraceFormat(CultureInfo.InvariantCulture, "Arguments: {0}", args.ToJsv());

            var arguments = new Arguments();
            if (!Parser.Default.ParseArguments(args, arguments))
            {
                return 1;
            }

            log.TraceFormat(CultureInfo.InvariantCulture, "Parsed arguments: {0}", arguments.ToJsv());

            var application = ApplicationFactory.Create(BaufileFinder.Find());
            foreach (var target in (arguments.TargetNames.Count == 0 ? new[] { "default" } : arguments.TargetNames)
                .Select(name => application.GetTarget(name)))
            {
                target.Invoke(application);
            }

            return 0;
        }
    }
}
