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
        [Example(null, null, "info", true, true)]
        [Example("-loglevel", "trace", "trace", true, true)]
        [Example("-l", "trace", "trace", true, true)]
        [Example("-trace", null, "trace", true, true)]
        [Example("-t", null, "trace", true, true)]
        [Example("-q", null, "warn", false, true)]
        [Example("-l", "o", null, false, false)]
        public static void LoggingAtAllLevels(
            string arg0,
            string arg1,
            string expectedLevel,
            bool taskMessageExpected,
            bool warnMessageExpected,
            string delimiter0,
            string delimiter1,
            Baufile baufile,
            string output)
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

bau.Execute();");
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

                    output.Contains("Starting 'default'").Should().Be(taskMessageExpected);
                    output.Contains("Finished 'default'").Should().Be(taskMessageExpected);
                    output
                        .Contains("WARN: Bau.Execute() (with no parameters) will be removed shortly. Use Bau.Run() instead.")
                        .Should().Be(warnMessageExpected);
                });
        }
    }
}
