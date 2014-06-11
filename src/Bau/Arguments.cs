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
        public LogLevel LogLevel { get; set; }

        public string[] Tasks { get; set; }

        public static Arguments Parse(IEnumerable<string> args)
        {
            Guard.AgainstNullArgument("args", args);

            var logLevel = LogLevel.Info;
            var tasks = new List<string>();
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

                    default:
                        var message = string.Format(
                            CultureInfo.InvariantCulture, "The option '{0}' is not recognised.", option.Key);

                        throw new ArgumentException(message, "args");
                }
            }

            return new Arguments
            {
                LogLevel = logLevel,
                Tasks = tasks.ToArray(),
            };
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
            }

            switch (optionName.ToUpperInvariant())
            {
                case "TRACE":
                    impliedValue = "TRACE";
                    return "LOGLEVEL";
                case "DEBUG":
                    impliedValue = "DEBUG";
                    return "LOGLEVEL";
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
                case "Q":
                case "QUIET":
                    return LogLevel.Warn;
                case "E":
                case "ERROR":
                case "QQ":
                    return LogLevel.Error;
                case "F":
                case "FATAL":
                    return LogLevel.Fatal;
                case "O":
                case "OFF":
                case "N":
                case "NONE":
                case "S":
                case "SILENT":
                    return LogLevel.Off;
                default:
                    var message = string.Format(
                        CultureInfo.InvariantCulture, "The log level '{0}' is not recognised.", logLevelString);

                    throw new ArgumentException(message, "logLevelString");
            }
        }
    }
}
