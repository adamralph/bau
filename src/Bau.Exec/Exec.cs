// <copyright file="Exec.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauExec
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using BauCore;

    public class Exec : Task
    {
        public string Command { get; set; }

        public IEnumerable<string> Args { get; set; }

        public string WorkingDirectory { get; set; }

        protected override void OnActionsExecuted()
        {
            if (this.Command == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "The '{0}' exec task has no command'.", this.Name);
                throw new InvalidOperationException(message);
            }

            if (string.IsNullOrWhiteSpace(this.Command))
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture, "The '{0}' exec task has an invalid command'.", this.Name);
                throw new InvalidOperationException(message);
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = this.Command;
                if (this.Args != null)
                {
                    process.StartInfo.Arguments = string.Join(" ", this.Args);
                }

                if (this.WorkingDirectory != null)
                {
                    process.StartInfo.WorkingDirectory = this.WorkingDirectory;
                }

                var argString = string.IsNullOrEmpty(process.StartInfo.Arguments)
                        ? string.Empty
                        : " " + string.Join(" ", process.StartInfo.Arguments);

                var workingDirectoryString = process.StartInfo.WorkingDirectory == null
                        ? string.Empty
                        : string.Format(CultureInfo.InvariantCulture, " with working directory '{0}'", process.StartInfo.WorkingDirectory);

                Console.WriteLine("Executing '{0}{1}'{2}...", process.StartInfo.FileName, argString, workingDirectoryString);

                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    var message = string.Format(
                        CultureInfo.InvariantCulture, "The command exited with code {0}.", process.ExitCode.ToString(CultureInfo.InvariantCulture));

                    throw new InvalidOperationException(message);
                }
            }
        }
    }

    public static class Plugin
    {
        public static IBau<Exec> Exec(this IBau bau, string name = Bau.DefaultTask)
        {
            return new Bau<Exec>(bau, name);
        }
    }
}
