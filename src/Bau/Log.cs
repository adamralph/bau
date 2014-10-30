// <copyright file="Log.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using ColoredConsole;
    using System;

    public static class Log
    {
        private static readonly ColorText prefix = new ColorText(
            new ColorToken("[", ConsoleColor.Gray),
            new ColorToken("Bau", ConsoleColor.DarkGreen),
            new ColorToken("] ", ConsoleColor.Gray));

        public static LogLevel LogLevel { get; set; }

        public static ConsoleColor TaskColor
        {
            get { return ConsoleColor.DarkCyan; }
        }

        public static void Fatal(IBauTask task, ColorText message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Fatal))
            {
                ColorConsole.WriteLine(GetPrefix(task).Concat("FATAL: ").Concat(message).Coalesce(ConsoleColor.Red));
            }
        }

        public static void Error(IBauTask task, ColorText message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Error))
            {
                ColorConsole.WriteLine(GetPrefix(task).Concat("ERROR: ").Concat(message).Coalesce(ConsoleColor.DarkRed));
            }
        }

        public static void Warn(IBauTask task, ColorText message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Warn))
            {
                ColorConsole.WriteLine(GetPrefix(task).Concat("WARN: ").Concat(message).Coalesce(ConsoleColor.DarkYellow));
            }
        }

        public static void Info(IBauTask task, ColorText message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Info))
            {
                ColorConsole.WriteLine(GetPrefix(task).Concat(message).Coalesce(ConsoleColor.Gray));
            }
        }

        public static void Debug(IBauTask task, ColorText message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Debug))
            {
                ColorConsole.WriteLine(GetPrefix(task).Concat("DEBUG: ").Concat(message).Coalesce(ConsoleColor.DarkGray));
            }
        }

        public static void Trace(IBauTask task, ColorText message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Trace))
            {
                ColorConsole.WriteLine(GetPrefix(task).Concat("TRACE: ").Concat(message).Coalesce(ConsoleColor.DarkMagenta));
            }
        }

        public static void Fatal(ColorText message)
        {
            if (IsEnabled(LogLevel.Fatal))
            {
                ColorConsole.WriteLine(prefix.Concat("FATAL: ").Concat(message).Coalesce(ConsoleColor.Red));
            }
        }

        public static void Error(ColorText message)
        {
            if (IsEnabled(LogLevel.Error))
            {
                ColorConsole.WriteLine(prefix.Concat("ERROR: ").Concat(message).Coalesce(ConsoleColor.DarkRed));
            }
        }

        public static void Warn(ColorText message)
        {
            if (IsEnabled(LogLevel.Warn))
            {
                ColorConsole.WriteLine(prefix.Concat("WARN: ").Concat(message).Coalesce(ConsoleColor.DarkYellow));
            }
        }

        public static void Info(ColorText message)
        {
            if (IsEnabled(LogLevel.Info))
            {
                ColorConsole.WriteLine(prefix.Concat(message).Coalesce(ConsoleColor.Gray));
            }
        }

        public static void Debug(ColorText message)
        {
            if (IsEnabled(LogLevel.Debug))
            {
                ColorConsole.WriteLine(prefix.Concat("DEBUG: ").Concat(message).Coalesce(ConsoleColor.DarkGray));
            }
        }

        public static void Trace(ColorText message)
        {
            if (IsEnabled(LogLevel.Trace))
            {
                ColorConsole.WriteLine(prefix.Concat("TRACE: ").Concat(message).Coalesce(ConsoleColor.DarkMagenta));
            }
        }

        private static bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Fatal:
                    return
                        LogLevel != LogLevel.Off;
                case LogLevel.Error:
                    return
                        LogLevel != LogLevel.Off &&
                        LogLevel != LogLevel.Fatal;
                case LogLevel.Warn:
                    return
                        LogLevel != LogLevel.Off &&
                        LogLevel != LogLevel.Fatal &&
                        LogLevel != LogLevel.Error;
                case LogLevel.Info:
                    return
                        LogLevel != LogLevel.Off &&
                        LogLevel != LogLevel.Fatal &&
                        LogLevel != LogLevel.Error &&
                        LogLevel != LogLevel.Warn;
                case LogLevel.Debug:
                    return
                        LogLevel != LogLevel.Off &&
                        LogLevel != LogLevel.Fatal &&
                        LogLevel != LogLevel.Error &&
                        LogLevel != LogLevel.Warn &&
                        LogLevel != LogLevel.Info;
                case LogLevel.Trace:
                    return
                        LogLevel != LogLevel.Off &&
                        LogLevel != LogLevel.Fatal &&
                        LogLevel != LogLevel.Error &&
                        LogLevel != LogLevel.Warn &&
                        LogLevel != LogLevel.Info &&
                        LogLevel != LogLevel.Debug;
                default:
                    return false;
            }
        }

        private static ColorText GetPrefix(IBauTask task)
        {
            Guard.AgainstNullArgument("task", task);

            var taskPrefix = new ColorText(
                new ColorToken("[", ConsoleColor.Gray),
                new ColorToken(task.Name, Log.TaskColor),
                new ColorToken("] ", ConsoleColor.Gray));

            return prefix.Concat(taskPrefix);
        }
    }
}
