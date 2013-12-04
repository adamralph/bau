// <copyright file="Exec.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Common.Logging;

    public class Exec : Target
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public string Command { get; set; }

        public string[] Parameters { get; set; }

        public override void Execute()
        {
            base.Execute();
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(this.CreateCommand(), string.Join(" ", this.CreateParameters()));
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Process exited with code {0}.",
                    process.ExitCode.ToString(CultureInfo.InvariantCulture));

                throw new InvalidOperationException(message);
            }
        }

        protected virtual string CreateCommand()
        {
            return this.Command;
        }

        protected virtual string[] CreateParameters()
        {
            return this.Parameters;
        }
    }
}
