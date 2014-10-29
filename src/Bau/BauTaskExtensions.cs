// <copyright file="BauTaskExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    public static class BauTaskExtensions
    {
        public static void LogFatal(this IBauTask task, string message)
        {
            Log.Fatal(task, message);
        }

        public static void LogError(this IBauTask task, string message)
        {
            Log.Error(task, message);
        }

        public static void LogWarn(this IBauTask task, string message)
        {
            Log.Warn(task, message);
        }

        public static void LogInfo(this IBauTask task, string message)
        {
            Log.Info(task, message);
        }

        public static void LogDebug(this IBauTask task, string message)
        {
            Log.Debug(task, message);
        }

        public static void LogTrace(this IBauTask task, string message)
        {
            Log.Trace(task, message);
        }
    }
}
