// <copyright file="Log.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;

    internal static class Log
    {
        public static LogLevel LogLevel { get; set; }

        public static void Fatal(IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Fatal))
            {
                BauConsole.WriteTaskMessage(task.Name, message, "FATAL: ", ConsoleColor.Red);
            }
        }

        public static void Error(IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Error))
            {
                BauConsole.WriteTaskMessage(task.Name, message, "ERROR: ", ConsoleColor.DarkRed);
            }
        }

        public static void Warn(IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Warn))
            {
                BauConsole.WriteTaskMessage(task.Name, message, "WARN: ", ConsoleColor.DarkYellow);
            }
        }

        public static void Info(IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Info))
            {
                BauConsole.WriteTaskMessage(task.Name, message, null, ConsoleColor.Gray);
            }
        }

        public static void Debug(IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Debug))
            {
                BauConsole.WriteTaskMessage(task.Name, message, "DEBUG: ", ConsoleColor.DarkGray);
            }
        }

        public static void Trace(IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (IsEnabled(LogLevel.Trace))
            {
                BauConsole.WriteTaskMessage(task.Name, message, "TRACE: ", ConsoleColor.DarkMagenta);
            }
        }

        internal static void InfoHeader()
        {
            if (IsEnabled(LogLevel.Info))
            {
                BauConsole.WriteHeader(null,ConsoleColor.Gray);
            }
        }

        internal static void WarnExecuteDeprecated()
        {
            if (IsEnabled(LogLevel.Warn))
            {
                BauConsole.WriteExecuteDeprecated("WARN: ", ConsoleColor.DarkYellow);
            }
        }

        internal static void ErrorInvalidTaskName(string task)
        {
            if (IsEnabled(LogLevel.Error))
            {
                BauConsole.WriteInvalidTaskName(task, "ERROR: ", ConsoleColor.DarkRed);
            }
        }

        internal static void ErrorTasksAlreadyExists(string name, string type)
        {
            if (IsEnabled(LogLevel.Error))
            {
                BauConsole.WriteTasksAlreadyExists(name, type, "ERROR: ", ConsoleColor.DarkRed);
            }
        }

        internal static void InfoTaskStarting(string task)
        {
            if (IsEnabled(LogLevel.Info))
            {
                BauConsole.WriteTaskStarting(task, null, ConsoleColor.Gray);
            }
        }

        internal static void InfoTaskFinished(string task, double milliseconds)
        {
            if (IsEnabled(LogLevel.Info))
            {
                BauConsole.WriteTaskFinished(task, milliseconds, null, ConsoleColor.Gray);
            }
        }

        internal static void ErrorTaskNotFound(string task)
        {
            if (IsEnabled(LogLevel.Error))
            {
                BauConsole.WriteTaskNotFound(task, "ERROR: ", ConsoleColor.DarkRed);
            }
        }

        internal static void ErrorTaskFailed(string task, double milliseconds, string exceptionMessage)
        {
            if (IsEnabled(LogLevel.Error))
            {
                BauConsole.WriteTaskFailed(task, milliseconds, exceptionMessage, "ERROR: ", ConsoleColor.DarkRed);
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
    }
}
