// <copyright file="Help.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System.Reflection;
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
                    var help =
@"Copyright (c) Bau contributors (baubuildch@gmail.com)

Usage: scriptcs <filename> -- [tasks|default*] [options]

Options:
  -T|-tasklist          Display the list of tasks
                        (d*|descriptive|a|all|p|prerequisites|j|json).
  -A                    Alias for -tasklist all.
  -P                    Alias for -tasklist prerequisites.
  -J                    Alias for -tasklist json.
  -l|-loglevel <level>  Log at the specified level
                        (a|all|t|trace|d|debug|i*|info|w|warn|e|error|f|fatal|o|off).
  -t                    Alias for -loglevel trace.
  -d                    Alias for -loglevel debug.
  -q                    Alias for -loglevel warn.
  -qq                   Alias for -loglevel error.
  -s                    Alias for -loglevel off.
  -?|-h|-help           Show help.

  One and two character option aliases are case-sensitive.

Examples:
  scriptcs baufile.csx                Run the 'default' task.
  scriptcs baufile.csx -- build test  Run the 'build' and 'test' tasks.
  scriptcs baufile.csx -- -l d        Run the 'default' task and log at debug level.
  scriptcs baufile.csx -- -T          Display the list of tasks with descriptions.
  scriptcs baufile.csx -- -T p        Display the list of tasks and prerequisites.

* Default value.

";

                    output.Should().Contain(help);
                });
        }
    }
}
