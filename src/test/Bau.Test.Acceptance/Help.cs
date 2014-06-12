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
        public static void SpecifiyingLogLevel(string arg, Baufile baufile, string output)
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
@"
Usage: scriptcs <baufile.csx> -- [tasks] [options]

Options:
  -l|-loglevel a|all|t|trace|d|debug|i*|info|w|warn|e|error|o|off
               Set the logging level.
  -t           Alias for -loglevel trace.
  -d           Alias for -loglevel debug.
  -q           Alias for -loglevel warn.
  -qq          Alias for -loglevel error.
  -s           Alias for -loglevel off.
  -?|-h|-help  Show help.

  One and two character option aliases are case-sensitive.

Examples: scriptcs baufile.csx
          scriptcs baufile.csx -- task1 task2
          scriptcs baufile.csx -- -d

";

                    output.Should().Contain(help);
                });
        }
    }
}
