// <copyright file="Arguments.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class Arguments
    {
        public string[] Tasks { get; set; }

        public LogLevel LogLevel { get; set; }

        public bool Help { get; set; }

        public static void ShowUsage()
        {
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("Usage: ", ConsoleColor.White),
                new ColorToken("scriptcs ", ConsoleColor.DarkGreen),
                new ColorToken("<", ConsoleColor.Gray),
                new ColorToken("baufile.csx", ConsoleColor.DarkCyan),
                new ColorToken("> ", ConsoleColor.Gray),
                new ColorToken("-- ", ConsoleColor.DarkGreen),
                new ColorToken("[", ConsoleColor.Gray),
                new ColorToken("tasks", ConsoleColor.DarkCyan),
                new ColorToken("] ", ConsoleColor.Gray),
                new ColorToken("[", ConsoleColor.Gray),
                new ColorToken("options", ConsoleColor.DarkCyan),
                new ColorToken("]", ConsoleColor.Gray)));

            ColorConsole.WriteLine(null);
            ColorConsole.WriteLine(new ColorToken("Options:", ConsoleColor.White));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -l", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("-loglevel ", ConsoleColor.DarkGreen),
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
                new ColorToken("*", ConsoleColor.White),
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
                new ColorToken("o", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("off", ConsoleColor.DarkGreen)));
            ColorConsole.WriteLine(new ColorToken("               Set the logging level.", ConsoleColor.Gray));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -t           ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel trace", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -d           ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel debug", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -q           ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel warn", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -qq          ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel error", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -s           ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-loglevel off", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -?", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("-h", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("-help", ConsoleColor.DarkGreen),
                new ColorToken("  Show help.", ConsoleColor.Gray)));

            ColorConsole.WriteLine(null);
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  One and two character option aliases are ", ConsoleColor.DarkYellow),
                new ColorToken("case-sensitive", ConsoleColor.Yellow),
                new ColorToken(".", ConsoleColor.DarkYellow)));

            ColorConsole.WriteLine(null);
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("Examples: ", ConsoleColor.White),
                new ColorToken("scriptcs baufile.csx", ConsoleColor.DarkGreen)));
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("          scriptcs baufile.csx -- task1 task2", ConsoleColor.DarkGreen)));
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("          scriptcs baufile.csx -- -d", ConsoleColor.DarkGreen)));

            ColorConsole.WriteLine(null);
        }

        public static Arguments Parse(IEnumerable<string> args)
        {
            Guard.AgainstNullArgument("args", args);

            var arguments = new Arguments
            {
                LogLevel = LogLevel.Info,
            };

            var tasks = new List<string>();
            foreach (var option in Parse(args, tasks))
            {
                switch (option.Key.ToUpperInvariant())
                {
                    case "LOGLEVEL":
                        var logLevels = option.Value;
                        if (logLevels.Any())
                        {
                            arguments.LogLevel = MapLogLevel(logLevels.First());
                        }

                        break;

                    case "HELP":
                        arguments.Help = true;
                        break;

                    default:
                        ShowUsage();
                        var message = string.Format(
                            CultureInfo.InvariantCulture, "The option '{0}' is not recognised.", option.Key);

                        throw new ArgumentException(message, "args");
                }
            }

            arguments.Tasks = tasks.ToArray();
            return arguments;
        }

        private static Dictionary<string, List<string>> Parse(IEnumerable<string> args, IList<string> tasks)
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
                    ShowUsage();
                    var message = string.Format(
                        CultureInfo.InvariantCulture, "The log level '{0}' is not recognised.", logLevelString);

                    throw new ArgumentException(message, "logLevelString");
            }
        }
    }
}
