// <copyright file="LogLevels.cs" company="Bau contributors">
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

    public static class LogLevels
    {
        [Scenario]
        public static void LoggingAtAllLevels(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a LoggingAtAllLevels baufile"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>();

bau.Do(() =>
{
    bau.CurrentTask.LogFatal(""fatal"");
    bau.CurrentTask.LogError(""error"");
    bau.CurrentTask.LogWarn(""warn"");
    bau.CurrentTask.LogInfo(""info"");
    bau.CurrentTask.LogDebug(""debug"");
    bau.CurrentTask.LogTrace(""trace"");
});

bau.Run();"));

            "When I execute the baufile"
                .f(() => output = baufile.Run("-trace"));

            "Then the fatal message should be logged"
                .f(() => output.Should().MatchEquivalentOf("*[default] *FATAL: fatal*"));

            "And the error message should be logged"
                .f(() => output.Should().MatchEquivalentOf("*[default] *ERROR: error*"));

            "And the warn message should be logged"
                .f(() => output.Should().MatchEquivalentOf("*[default] *WARN: warn*"));

            "And the info message should be logged"
                .f(() => output.Should().MatchEquivalentOf("*[default] *info*"));

            "And the debug message should be logged"
                .f(() => output.Should().MatchEquivalentOf("*[default] *DEBUG: debug*"));

            "And the trace message should be logged"
                .f(() => output.Should().MatchEquivalentOf("*[default] *TRACE: trace*"));
        }

        [Scenario]
        [Example(null, null, "info", true, true)]
        [Example("-l", null, "info", true, true)]

        [Example("-l", "a", "trace", true, true)]
        [Example("-l", "t", "trace", true, true)]
        [Example("-l", "d", "debug", true, true)]
        [Example("-l", "i", "info", true, true)]
        [Example("-l", "w", "warn", false, true)]
        [Example("-l", "e", "error", false, false)]
        [Example("-l", "o", null, false, false)]

        [Example("-t", null, "trace", true, true)]
        [Example("-d", null, "debug", true, true)]
        [Example("-q", null, "warn", false, true)]
        [Example("-qq", null, "error", false, false)]
        [Example("-s", null, null, false, false)]

        [Example("-l", "trace", "trace", true, true)]
        [Example("-loglevel", "trace", "trace", true, true)]
        [Example("-LOGlevel", "trace", "trace", true, true)]
        public static void SpecifiyingLogLevel(
            string arg0,
            string arg1,
            string expectedLevel,
            bool infoMessageExpected,
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

            ("Then " + expectedLevel ?? "no" + " task messages should be logged")
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

            ("And info messages should " + (infoMessageExpected ? null : "not") + " be logged")
                .f(() => output.Contains("Starting 'default'").Should().Be(infoMessageExpected));

            ("And warning messages should " + (infoMessageExpected ? null : "not") + " be logged")
                .f(() => output
                    .Contains("WARN: Bau.Execute() (with no parameters) will be removed shortly. Use Bau.Run() instead.")
                    .Should().Be(warnMessageExpected));
        }
    }
}
