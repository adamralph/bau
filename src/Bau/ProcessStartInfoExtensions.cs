// <copyright file="ProcessStartInfoExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public static class ProcessStartInfoExtensions
    {
        public static void Run(this ProcessStartInfo info)
        {
            using (var process = new Process())
            {
                process.StartInfo = info;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    var message = string.Format(
                        CultureInfo.InvariantCulture,
                        "The command exited with code {0}.",
                        process.ExitCode.ToString(CultureInfo.InvariantCulture));

                    throw new InvalidOperationException(message);
                }
            }
        }
    }
}
