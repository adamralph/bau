// <copyright file="Exec.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauExec
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using BauCore;

    public class Exec : BauTask
    {
        public string Command { get; set; }

        public IEnumerable<string> Args { get; set; }

        public string WorkingDirectory { get; set; }

        public Exec Run(string command)
        {
            this.Command = command;
            return this;
        }

        public Exec With(params string[] args)
        {
            this.Args = args;
            return this;
        }

        public Exec In(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
            return this;
        }

        protected override void OnActionsExecuted()
        {
            if (this.Command == null)
            {
                throw new InvalidOperationException("The command is null.");
            }

            if (string.IsNullOrWhiteSpace(this.Command))
            {
                throw new InvalidOperationException("The command is invalid.");
            }

            var info = new ProcessStartInfo
            {
                FileName = this.Command,
                Arguments = this.Args == null
                    ? null
                    : string.Join(" ", this.Args.Where(arg => !string.IsNullOrWhiteSpace(arg))),

                WorkingDirectory = this.WorkingDirectory,
                UseShellExecute = false,
            };

            var argString = string.IsNullOrEmpty(info.Arguments)
                ? string.Empty
                : " " + string.Join(" ", info.Arguments);

            var workingDirectoryString = string.IsNullOrEmpty(info.WorkingDirectory)
                ? string.Empty
                : string.Format(CultureInfo.InvariantCulture, " with working directory '{0}'", info.WorkingDirectory);

            var message = string.Format(
                CultureInfo.InvariantCulture,
                "Executing '{0}{1}'{2}...",
                info.FileName,
                argString,
                workingDirectoryString);

            this.LogDebug(message);
            info.Run();
        }
    }

    public static class Plugin
    {
        public static ITaskBuilder<Exec> Exec(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<Exec>(builder, name);
        }
    }
}
