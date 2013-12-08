// <copyright file="Extensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using Common.Logging;
    using ServiceStack.Text;

    public static class Extensions
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public static string Truncate(this string text, int maxLength)
        {
            if (text == null || text.Length <= maxLength)
            {
                return text;
            }

            return string.Concat(text.Substring(0, maxLength - 3), "...");
        }

        public static void Execute(this Process process, string command, IEnumerable<string> parameters)
        {
            Guard.AgainstNullArgument("process", process);

            process.StartInfo = new ProcessStartInfo(command);
            if (parameters != null)
            {
                process.StartInfo.Arguments = string.Join(" ", parameters);
            }

            process.StartInfo.UseShellExecute = false;

            log.DebugFormat(
                CultureInfo.InvariantCulture, "Executing {0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

            log.TraceFormat(CultureInfo.InvariantCulture, "Process start info: {0}", process.StartInfo.ToJsv());
            
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "The process exited with code {0}.",
                    process.ExitCode.ToString(CultureInfo.InvariantCulture));

                throw new InvalidOperationException(message);
            }
        }
    }
}
