// <copyright file="Arguments.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau
{
    using System;
    using System.Collections.Generic;
    using CommandLine;
    using CommandLine.Text;

    public class Arguments
    {
        [ParserState]
        public IParserState LastParserState { get; set; }

        [ValueList(typeof(List<string>))]
        public IList<string> Targets { get; set; }

        [Option('d', "debug", HelpText = "Turn on debugging.")]
        public bool Debug { get; set; }

        [Option('D', "describe", HelpText = " Describe the targets, then exit.")]
        public bool DescribeTargets { get; set; }

        [Option('P', "prereqs", HelpText = "Display the targets and dependencies, then exit.")]
        public bool DisplayPrerequisites { get; set; }

        [Option('t', "trace", HelpText = "Turn on tracing.")]
        public bool Trace { get; set; }

        [Option('T', "targets", HelpText = "Display the targets with descriptions, then exit.")]
        public bool DisplayTargets { get; set; }

        [HelpOption('h', "help")]
        public string GetUsage()
        {
            var help = new HelpText();
            help.Heading = "Usage: bau {options} targets...";

            if (this.LastParserState != null && this.LastParserState.Errors.Count > 0)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }

            help.AddPreOptionsLine("Options are...");
            help.AddDashesToOption = true;
            help.AddOptions(this);
            return help;
        }
    }
}
