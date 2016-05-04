// <copyright file="Arguments.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    public class Arguments
    {
        private readonly ReadOnlyCollection<string> tasks;
        private readonly LogLevel logLevel;
        private readonly TaskListType? taskListType;
        private readonly bool help;
        private readonly List<string> namedParameters; 

        public Arguments(
            IEnumerable<string> tasks,
            LogLevel logLevel,
            TaskListType? taskListType,
            bool help,
            List<string> namedParameters)
        {
            Guard.AgainstNullArgument("tasks", tasks);

            this.tasks = new ReadOnlyCollection<string>(tasks.ToList());
            this.logLevel = logLevel;
            this.taskListType = taskListType;
            this.help = help;
            this.namedParameters = namedParameters;
        }

        public IEnumerable<string> Tasks
        {
            get { return this.tasks; }
        }

        public LogLevel LogLevel
        {
            get { return this.logLevel; }
        }

        public TaskListType? TaskListType
        {
            get { return this.taskListType; }
        }
        
        public List<string> NamedParameters
        {
            get { return namedParameters; }
        }

        public bool Help
        {
            get { return this.help; }
        }

        public static void ShowUsage(ColorText header)
        {
            ColorConsole.WriteLine(header);
            ColorConsole.WriteLine(null);
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

            ColorConsole.WriteLine(null);
            ColorConsole.WriteLine(new ColorToken("Options:", ConsoleColor.White));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -T", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.DarkGray),
                new ColorToken("-tasklist", ConsoleColor.DarkGreen),
                new ColorToken("          Display the list of tasks", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("                        (", ConsoleColor.Gray),
                new ColorToken("d", ConsoleColor.DarkGreen),
                new ColorToken("*", ConsoleColor.Gray),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("descriptive", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("a", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("all", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("p", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("prerequisites", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("j", ConsoleColor.DarkGreen),
                new ColorToken("|", ConsoleColor.Gray),
                new ColorToken("json", ConsoleColor.DarkGreen),
                new ColorToken(").", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -A                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-tasklist all", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -P                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-tasklist prerequisites", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  -J                    ", ConsoleColor.DarkGreen),
                new ColorToken("Alias for ", ConsoleColor.Gray),
                new ColorToken("-tasklist json", ConsoleColor.DarkGreen),
                new ColorToken(".", ConsoleColor.Gray)));

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

            ColorConsole.WriteLine(null);
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  One and two character option aliases are ", ConsoleColor.Gray),
                new ColorToken("case-sensitive", ConsoleColor.White),
                new ColorToken(".", ConsoleColor.Gray)));

            ColorConsole.WriteLine(null);
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

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  scriptcs baufile.csx -- -T          ", ConsoleColor.DarkGreen),
                new ColorToken("Display the list of tasks with descriptions.", ConsoleColor.Gray)));

            ColorConsole.WriteLine(new ColorText(
                new ColorToken("  scriptcs baufile.csx -- -T p        ", ConsoleColor.DarkGreen),
                new ColorToken("Display the list of tasks and prerequisites.", ConsoleColor.Gray)));

            ColorConsole.WriteLine(null);
            ColorConsole.WriteLine(new ColorText(
                new ColorToken("* Default value.", ConsoleColor.Gray)));

            ColorConsole.WriteLine(null);
        }

        public static Arguments Parse(IEnumerable<string> args)
        {
            Guard.AgainstNullArgument("args", args);

            var tasks = new List<string>();
            var logLevel = LogLevel.Info;
            var help = false;
            var taskListType = default(TaskListType?);
            var namedParameters = new List<string>();
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
                    case "CONFIG":
                        namedParameters = option.Value?? new List<string>();
                        break;
                    case "TASKLIST":
                        var taskListTypes = option.Value;
                        taskListType = taskListTypes.Any()
                            ? MapTaskListType(taskListTypes.First())
                            : BauCore.TaskListType.Descriptive;

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

            return new Arguments(tasks, logLevel, taskListType, help, namedParameters);
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
                case "TASKLIST":
                case "T":
                    return "TASKLIST";
                case "A":
                    impliedValue = "ALL";
                    return "TASKLIST";
                case "P":
                    impliedValue = "PREREQUISITES";
                    return "TASKLIST";
                case "J":
                    impliedValue = "JSON";
                    return "TASKLIST";
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
                case "p":
                    impliedValue = "";
                    return "CONFIG";
                case "h":
                case "?":
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

        private static TaskListType MapTaskListType(string taskListTypeString)
        {
            switch (taskListTypeString.ToUpperInvariant())
            {
                case "DESCRIPTIVE":
                case "D":
                    return BauCore.TaskListType.Descriptive;
                case "ALL":
                case "A":
                    return BauCore.TaskListType.All;
                case "PREREQUISITES":
                case "P":
                    return BauCore.TaskListType.Prerequisites;
                case "JSON":
                case "J":
                    return BauCore.TaskListType.Json;
                default:
                    var message = string.Format(
                        CultureInfo.InvariantCulture, "The task list type '{0}' is not recognised.", taskListTypeString);

                    throw new ArgumentException(message, taskListTypeString);
            }
        }
    }
}
