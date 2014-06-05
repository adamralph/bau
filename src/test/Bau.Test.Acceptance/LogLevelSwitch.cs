// <copyright file="LogLevelSwitch.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Bau.Test.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;

    public static class LogLevelSwitch
    {
        [Scenario]
        [Example(null, null, "info")]
        [Example("-loglevel", "trace", "trace")]
        [Example("-l", "trace", "trace")]
        [Example("-trace", null, "trace")]
        [Example("-t", null, "trace")]
        [Example("-l", "o", null)]
        public static void LoggingAtAllLevels(
            string arg0, string arg1, string expectedLevel, string delimiter0, string delimiter1, Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a LoggingAtAllLevels baufile"
                .f(() =>
                {
                    delimiter0 = Guid.NewGuid().ToString();
                    delimiter1 = Guid.NewGuid().ToString();
                    baufile = Baufile.Create(scenario).WriteLine(   
@"var bau = Require<Bau>();

bau.Do(() =>
{
    bau.CurrentTask.LogFatal(""" + delimiter0 + @"fatal" + delimiter1 + @""");
    bau.CurrentTask.LogError(""" + delimiter0 + @"error" + delimiter1 + @""");
    bau.CurrentTask.LogWarn(""" + delimiter0 + @"warn" + delimiter1 + @""");
    bau.CurrentTask.LogInfo(""" + delimiter0 + @"info" + delimiter1 + @""");
    bau.CurrentTask.LogDebug(""" + delimiter0 + @"debug" + delimiter1 + @""");
    bau.CurrentTask.LogTrace(""" + delimiter0 + @"trace" + delimiter1 + @""");
});

bau.Run();");
                });

            "When I execute the baufile with arguments '{0}' and '{1}'"
                .f(() => output = baufile.Run(arg0, arg1));

            "Then the expected messages should be logged"
                .f(() =>
                {
                    var levels = new[] { "fatal", "error", "warn", "info", "debug", "trace", };
                    var expectedLevels = levels
                        .Reverse()
                        .SkipWhile(level => level != (expectedLevel == "all" ? "trace" : expectedLevel))
                        .Reverse();

                    var actualLevels = output.Split(new[] { delimiter0 }, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(1)
                        .Select(x => x.Split(new[] { delimiter1 }, StringSplitOptions.RemoveEmptyEntries).First());

                    actualLevels.Should().Equal(expectedLevels);
                });
        }
    }
}
