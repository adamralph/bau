// <copyright file="BauTaskExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;

    public static class BauTaskExtensions
    {
        public static LogLevel LogLevel { get; set; }

        public static void LogFatal(this IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (LogLevel != LogLevel.Off)
            {
                BauConsole.WriteTaskMessage(task.Name, "FATAL: " + message, ConsoleColor.Red);
            }
        }

        public static void LogError(this IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (LogLevel != LogLevel.Off &&
                LogLevel != LogLevel.Fatal)
            {
                BauConsole.WriteTaskMessage(task.Name, "ERROR: " + message, ConsoleColor.DarkRed);
            }
        }

        public static void LogWarn(this IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (LogLevel != LogLevel.Off &&
                LogLevel != LogLevel.Fatal &&
                LogLevel != LogLevel.Error)
            {
                BauConsole.WriteTaskMessage(task.Name, "WARN: " + message, ConsoleColor.DarkYellow);
            }
        }

        public static void LogInfo(this IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (LogLevel != LogLevel.Off &&
                LogLevel != LogLevel.Fatal &&
                LogLevel != LogLevel.Error &&
                LogLevel != LogLevel.Warn)
            {
                BauConsole.WriteTaskMessage(task.Name, message, ConsoleColor.Gray);
            }
        }

        public static void LogDebug(this IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (LogLevel != LogLevel.Off &&
                LogLevel != LogLevel.Fatal &&
                LogLevel != LogLevel.Error &&
                LogLevel != LogLevel.Warn &&
                LogLevel != LogLevel.Info)
            {
                BauConsole.WriteTaskMessage(task.Name, "DEBUG: " + message, ConsoleColor.DarkGray);
            }
        }

        public static void LogTrace(this IBauTask task, string message)
        {
            Guard.AgainstNullArgument("task", task);

            if (LogLevel != LogLevel.Off &&
                LogLevel != LogLevel.Fatal &&
                LogLevel != LogLevel.Error &&
                LogLevel != LogLevel.Warn &&
                LogLevel != LogLevel.Info &&
                LogLevel != LogLevel.Debug)
            {
                BauConsole.WriteTaskMessage(task.Name, "TRACE: " + message, ConsoleColor.DarkMagenta);
            }
        }
    }
}
