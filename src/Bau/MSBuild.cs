// <copyright file="MSBuild.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using Common.Logging;

    public class MSBuild : Target
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        public MSBuild()
        {
            this.NetVersion = "net45";
        }

        public string NetVersion { get; set; }

        public string Solution { get; set; }

        public string Verbosity { get; set; }

        public string Logger { get; set; }

        public int? MaxCpuCount { get; set; }

        public bool NoLogo { get; set; }

        public string[] Targets { get; set; }

        public object Properties { get; set; }

        public object OtherSwitches { get; set; }

        public override void Execute()
        {
            base.Execute();

            var parameters = new List<string>();
            parameters.Add(this.Solution);
            MSBuild.AddNamedParameter(parameters, "verbosity", this.Verbosity);
            MSBuild.AddNamedParameter(parameters, "logger", this.Logger);
            MSBuild.AddNamedParameter(parameters, "maxcpucount", this.MaxCpuCount);
            if (this.NoLogo)
            {
                parameters.Add("/nologo");
            }

            if (this.Targets != null)
            {
                MSBuild.AddNamedParameter(parameters, "target", string.Join(";", this.Targets));
            }

            if (this.Properties != null)
            {
                foreach (var property in this.Properties.GetType().GetProperties())
                {
                    MSBuild.AddNamedParameter(parameters, "property", string.Concat(property.Name, "=", property.GetValue(this.Properties)));
                }
            }

            if (this.OtherSwitches != null)
            {
                foreach (var property in this.OtherSwitches.GetType().GetProperties())
                {
                    var value = property.GetValue(this.OtherSwitches);
                    var text = value as string;
                    if (text != null && text.Length == 0)
                    {
                        parameters.Add(string.Concat("/", property.Name));
                    }
                    else
                    {
                        MSBuild.AddNamedParameter(parameters, property.Name, value);
                    }
                }
            }

            new Process().Execute(this.GetPath(), parameters);
        }

        private static void AddNamedParameter<TValue>(List<string> parameters, string name, TValue value)
        {
            if (value != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "/{0}:{1}", name, value));
            }
        }

        private string GetPath()
        {
            var windowsFolder =
                Environment.GetEnvironmentVariable("windir") ??
                Environment.GetEnvironmentVariable("WINDIR") ??
                Path.Combine("C:", "Windows");

            string version;
            switch (this.NetVersion)
            {
                case "net2":
                case "net20":
                case "net3":
                case "net30":
                    version = "v2.0.50727";
                    break;

                case "net35":
                    version = "v3.5";
                    break;

                case "net4":
                case "net40":
                case "net45":
                    version = "v4.0.30319";
                    break;

                default:
                    var message = string.Format(
                        CultureInfo.InvariantCulture, "The .NET Framework {0} is not supported.", this.NetVersion);

                    throw new InvalidOperationException(message);
            }

            return Path.Combine(windowsFolder, "Microsoft.NET", "Framework", version, "MSBuild.exe");
        }
    }
}
