// <copyright file="Arguments.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using ColoredConsole;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    public class Arguments
    {
        private readonly ReadOnlyCollection<string> tasks;
        private readonly LogLevel logLevel;
        private readonly bool help;

        public Arguments(
            IEnumerable<string> tasks,
            LogLevel logLevel,
            bool help)
        {
            Guard.AgainstNullArgument("tasks", tasks);

            this.tasks = new ReadOnlyCollection<string>(tasks.ToList());
            this.logLevel = logLevel;
            this.help = help;
        }

        public IEnumerable<string> Tasks
        {
            get { return this.tasks; }
        }

        public LogLevel LogLevel
        {
            get { return this.logLevel; }
        }

        public bool Help
        {
            get { return this.help; }
        }

        public static void ShowUsage(ColorText header)
        {
            ColorConsole.WriteLine(header);
            ColorConsole.WriteLine();
            ShowUsage();
        }

        public static void ShowUsage()
        {
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("Usage: ", ConsoleColor.White),
                new ColorToken("scriptcs ", ConsoleColor.DarkGreen),
                new ColorToken("<", ConsoleColor.Gray),
                new ColorToken("filename", ConsoleColor.DarkCyan),
                new ColorToken("> ", ConsoleColor.Gray),
                new ColorToken("-- ", ConsoleColor.DarkGreen),
                new ColorToken("[", ConsoleColor.Gray),
                new ColorToken("tasks", ConsoleColor.DarkCyan),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("default", ConsoleColor.DarkGreen),
                new ColorToken("*] ", ConsoleColor.Gray),
                new ColorToken("[", ConsoleColor.Gray),
                new ColorToken("options", ConsoleColor.DarkCyan),
                new ColorToken("]", ConsoleColor.Gray)));

            ColorConsole.WriteLine();
            ColorConsole.WriteLine(new ColorToken("Options:", ConsoleColor.White));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -l", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("-loglevel ", ConsoleColor.DarkGreen),
                new ColorToken("<", ConsoleColor.Gray),
                new ColorToken("level", ConsoleColor.DarkCyan),
                new ColorToken(">", ConsoleColor.Gray),
                new ColorToken("  Log at the specified level", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("                        (", ConsoleColor.Gray),
                new ColorToken("a", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("all", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("t", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("trace", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("d", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("debug", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("i", ConsoleColor.DarkGreen),
                new ColorToken("*", ConsoleColor.Gray),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("info", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("w", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("warn", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("e", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("error", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("f", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("fatal", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("o", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("off", ConsoleColor.DarkGreen),
                new ColorToken(").", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -t                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel trace", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -d                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel debug", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -q                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel warn", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -qq                   ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel error", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -s                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel off", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -?", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("-h", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("-help", ConsoleColor.DarkGreen),
                new ColorToken("           Show help.", ConsoleColor.Gray)));

            ColorConsole.WriteLine();
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  One and two character option aliases are ", ConsoleColor.Gray),
                new ColorToken("case-sensitive", ConsoleColor.White),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine();
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("Examples:", ConsoleColor.White)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  scriptcs baufile.csx                ", ConsoleColor.DarkGreen),
                new ColorToken("Run the '", ConsoleColor.Gray),
                new ColorToken("default", ConsoleColor.DarkCyan),
                new ColorToken("' task.", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  scriptcs baufile.csx -- build test  ", ConsoleColor.DarkGreen),
                new ColorToken("Run the '", ConsoleColor.Gray),
                new ColorToken("build", ConsoleColor.DarkCyan),
                new ColorToken("' and '", ConsoleColor.Gray),
                new ColorToken("test", ConsoleColor.DarkCyan),
                new ColorToken("' tasks.", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  scriptcs baufile.csx -- -l d        ", ConsoleColor.DarkGreen),
                new ColorToken("Run the '", ConsoleColor.Gray),
                new ColorToken("default", ConsoleColor.DarkCyan),
                new ColorToken("' task and log at debug level.", ConsoleColor.Gray)));

            ColorConsole.WriteLine();
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("* Default value.", ConsoleColor.Gray)));

            ColorConsole.WriteLine();
        }

        public static Arguments Parse(IEnumerable<string> args)
        {
            Guard.AgainstNullArgument("args", args);

            var tasks = new List<string>();
            var logLevel = LogLevel.Info;
            var help = false;
            foreach (var option in Parse(args, tasks))
            {
                switch (option.Key.ToUpperInvariant())
                {
                    case "LOGLEVEL":
                        var logLevels = option.Value;
                        if (logLevels.Any())
                        {
                            logLevel = MapLogLevel(logLevels.First());
                        }

                        break;

                    case "HELP":
                        help = true;
                        break;

                    default:
                        var message = string.Format(
                            CultureInfo.InvariantCulture, "The option '{0}' is not recognised.", option.Key);

                        throw new ArgumentException(message, "args");
                }
            }

            return new Arguments(tasks, logLevel, help);
        }

        private static Dictionary<string, List<string>> Parse(IEnumerable<string> args, ICollection<string> tasks)
        {
            var options = new Dictionary<string, List<string>>(StringComparer.Create(CultureInfo.InvariantCulture, true));
            string currentName = null;
            foreach (var arg in args
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim()))
            {
                if (!arg.StartsWith("-", StringComparison.Ordinal))
                {
                    if (currentName == null)
                    {
                        tasks.Add(arg);
                        continue;
                    }

                    options[currentName].Add(arg);
                    continue;
                }

                string impliedValue;
                var name = Map(arg.TrimStart('-').Trim(), out impliedValue);

                if (name.Length > 0)
                {
                    currentName = name;
                    if (!options.ContainsKey(name))
                    {
                        var values = new List<string>();
                        if (!string.IsNullOrWhiteSpace(impliedValue))
                        {
                            values.Add(impliedValue);
                        }

                        options.Add(currentName, values);
                    }
                }
            }

            return options;
        }

        private static string Map(string optionName, out string impliedValue)
        {
            impliedValue = null;
            switch (optionName)
            {
                case "l":
                    return "LOGLEVEL";
                case "t":
                    impliedValue = "TRACE";
                    return "LOGLEVEL";
                case "d":
                    impliedValue = "DEBUG";
                    return "LOGLEVEL";
                case "q":
                    impliedValue = "WARN";
                    return "LOGLEVEL";
                case "qq":
                    impliedValue = "ERROR";
                    return "LOGLEVEL";
                case "s":
                    impliedValue = "OFF";
                    return "LOGLEVEL";
                case "h":
                case "?":
                    impliedValue = "OFF";
                    return "HELP";
                default:
                    return optionName;
            }
        }

        private static LogLevel MapLogLevel(string logLevelString)
        {
            switch (logLevelString.ToUpperInvariant())
            {
                case "A":
                case "ALL":
                    return LogLevel.All;
                case "T":
                case "TRACE":
                    return LogLevel.Trace;
                case "D":
                case "DEBUG":
                    return LogLevel.Debug;
                case "I":
                case "INFO":
                    return LogLevel.Info;
                case "W":
                case "WARN":
                    return LogLevel.Warn;
                case "E":
                case "ERROR":
                    return LogLevel.Error;
                case "F":
                case "FATAL":
                    return LogLevel.Fatal;
                case "O":
                case "OFF":
                    return LogLevel.Off;
                default:
                    var message = string.Format(
                        CultureInfo.InvariantCulture, "The log level '{0}' is not recognised.", logLevelString);

                    throw new ArgumentException(message, "logLevelString");
            }
        }
    }
}
