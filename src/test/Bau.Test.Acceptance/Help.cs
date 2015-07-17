// <copyright file="Help.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System.Reflection;
    using System.Text;
    using Bau.Test.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;

    public static class Help
    {
        [Scenario]
        [Example("-?")]
        [Example("-h")]
        [Example("-help")]
        [Example("-Help")]
        public static void GettingHelp(string arg, Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a no-op baufile"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"Require<Bau>().Run();"));

            "When I execute the baufile with argument '{0}'"
                .f(() => output = baufile.Run(arg));

            "Then help should be displayed"
                .f(() =>
                {
                    var help = new StringBuilder()
                        .AppendLine(@"Copyright (c) Bau contributors (baubuildch@gmail.com)")
                        .AppendLine()
                        .AppendLine("Usage: scriptcs <filename> -- [tasks|default*] [options]")
                        .AppendLine()
                        .AppendLine("Options:")
                        .AppendLine("  -T|-tasklist          Display the list of tasks")
                        .AppendLine("                        (d*|descriptive|a|all|p|prerequisites|j|json).")
                        .AppendLine("  -A                    Alias for -tasklist all.")
                        .AppendLine("  -P                    Alias for -tasklist prerequisites.")
                        .AppendLine("  -J                    Alias for -tasklist json.")
                        .AppendLine("  -l|-loglevel <level>  Log at the specified level")
                        .AppendLine("                        (a|all|t|trace|d|debug|i*|info|w|warn|e|error|f|fatal|o|off).")
                        .AppendLine("  -t                    Alias for -loglevel trace.")
                        .AppendLine("  -d                    Alias for -loglevel debug.")
                        .AppendLine("  -q                    Alias for -loglevel warn.")
                        .AppendLine("  -qq                   Alias for -loglevel error.")
                        .AppendLine("  -s                    Alias for -loglevel off.")
                        .AppendLine("  -?|-h|-help           Show help.")
                        .AppendLine()
                        .AppendLine("  One and two character option aliases are case-sensitive.")
                        .AppendLine()
                        .AppendLine("Examples:")
                        .AppendLine("  scriptcs baufile.csx                Run the 'default' task.")
                        .AppendLine("  scriptcs baufile.csx -- build test  Run the 'build' and 'test' tasks.")
                        .AppendLine("  scriptcs baufile.csx -- -l d        Run the 'default' task and log at debug level.")
                        .AppendLine("  scriptcs baufile.csx -- -T          Display the list of tasks with descriptions.")
                        .AppendLine("  scriptcs baufile.csx -- -T p        Display the list of tasks and prerequisites.")
                        .AppendLine()
                        .AppendLine("* Default value.")
                        .AppendLine()
                        .ToString();

                    output.Should().Contain(help);
                });
        }
    }
}
