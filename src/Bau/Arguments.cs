// <copyright file="Arguments.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System.Collections.Generic;
    using CommandLine;
    using CommandLine.Text;

    public class Arguments
    {
        [ValueList(typeof(List<string>))]
        public IList<string> TargetNames { get; set; }

        [Option('T', "targets", HelpText = "Display the tasks with descriptions, then exit.")]
        public bool Targets { get; set; }

        [Option('t', "trace", HelpText = "Turn on tracing.")]
        public bool Trace { get; set; }

        [HelpOption('h', "help")]
        public string GetUsage()
        {
            var help = new HelpText();
            help.Heading = "Usage: bau {options} targets...";
            help.AddPreOptionsLine(" ");
            help.AddPreOptionsLine("Options are...");
            help.AddDashesToOption = true;
            help.AddOptions(this);
            return help;
        }
    }
}
