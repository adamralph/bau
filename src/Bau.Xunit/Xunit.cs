// <copyright file="Xunit.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauXunit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using BauCore;

    public class Xunit : BauTask
    {
        public string Exe { get; set; }

        public IEnumerable<string> Assemblies { get; set; }

        public bool Silent { get; set; }

        public bool ForceTeamCity { get; set; }

        public bool Wait { get; set; }

        public bool NoShadowCopy { get; set; }

        public string XmlFormat { get; set; }

        public string HtmlFormat { get; set; }

        public string NunitFormat { get; set; }

        public string Args { get; set; }

        public string WorkingDirectory { get; set; }

        public Xunit Use(string exe)
        {
            this.Exe = exe;
            return this;
        }

        public Xunit Run(params string[] assemblies)
        {
            this.Assemblies = assemblies;
            return this;
        }

        public Xunit Silence()
        {
            this.Silent = true;
            return this;
        }

        public Xunit Unsilence()
        {
            this.Silent = false;
            return this;
        }

        public Xunit TeamCity()
        {
            this.ForceTeamCity = true;
            return this;
        }

        public Xunit WaitForInput()
        {
            this.Wait = true;
            return this;
        }

        public Xunit DoNotWaitForInput()
        {
            this.Wait = false;
            return this;
        }

        public Xunit NoShadow()
        {
            this.NoShadowCopy = true;
            return this;
        }

        public Xunit Shadow()
        {
            this.NoShadowCopy = false;
            return this;
        }

        public Xunit Xml(string format)
        {
            this.XmlFormat = format;
            return this;
        }

        public Xunit Html(string format)
        {
            this.HtmlFormat = format;
            return this;
        }

        public Xunit Nunit(string format)
        {
            this.NunitFormat = format;
            return this;
        }

        public Xunit Xml()
        {
            return this.Xml("{0}.TestResults.xml");
        }

        public Xunit Html()
        {
            return this.Html("{0}.TestResults.html");
        }

        public Xunit Nunit()
        {
            return this.Nunit("{0}.TestResults.NUnit.xml");
        }

        public Xunit With(string args)
        {
            this.Args = args;
            return this;
        }

        public Xunit In(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
            return this;
        }

        protected override void OnActionsExecuted()
        {
            if (this.Exe == null)
            {
                throw new InvalidOperationException("The exe is null.");
            }

            if (string.IsNullOrWhiteSpace(this.Exe))
            {
                throw new InvalidOperationException("The exe is invalid.");
            }

            string[] assemblyArray;
            if (this.Assemblies == null || (assemblyArray = this.Assemblies.ToArray()).Length == 0)
            {
                this.LogInfo("No assemblies specified.");
                return;
            }

            var options = this.CreateOptions();
            var isMono = Type.GetType("Mono.Runtime") != null;

            foreach (var assembly in assemblyArray)
            {
                var info = this.CreateStartInfo(assembly, options, isMono);

                if (assemblyArray.Length > 1)
                {
                    this.LogInfo(string.Format(CultureInfo.InvariantCulture, "Running '{0}'...", assembly));
                }

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

        protected IEnumerable<string> CreateOptions()
        {
            var args = new List<string>();

            if (this.Silent)
            {
                args.Add("/silent");
            }

            if (this.ForceTeamCity)
            {
                args.Add("/teamcity");
            }

            if (this.Wait)
            {
                args.Add("/wait");
            }

            if (this.NoShadowCopy)
            {
                args.Add("/noshadow");
            }

            if (!string.IsNullOrWhiteSpace(this.Args))
            {
                args.Add(this.Args);
            }

            return args;
        }

        protected ProcessStartInfo CreateStartInfo(string assembly, IEnumerable<string> options, bool isMono)
        {
            var assemblyOptions = new List<string>();

            if (this.XmlFormat != null)
            {
                assemblyOptions.Add("/xml " + string.Format(CultureInfo.InvariantCulture, this.XmlFormat, assembly));
            }

            if (this.HtmlFormat != null)
            {
                assemblyOptions.Add("/html " + string.Format(CultureInfo.InvariantCulture, this.HtmlFormat, assembly));
            }

            if (this.NunitFormat != null)
            {
                assemblyOptions.Add("/nunit " + string.Format(CultureInfo.InvariantCulture, this.NunitFormat, assembly));
            }

            var fileName = isMono ? "mono" : this.Exe;
            var arguments = isMono ? new[] { this.Exe, assembly } : new[] { assembly };

            return new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = string.Join(" ", arguments.Concat(options).Concat(assemblyOptions)),
                WorkingDirectory = this.WorkingDirectory,
                UseShellExecute = false,
            };
        }
    }

    public static class Plugin
    {
        public static ITaskBuilder<Xunit> Xunit(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<Xunit>(builder, name);
        }
    }
}
