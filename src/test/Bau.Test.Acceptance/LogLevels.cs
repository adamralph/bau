// <copyright file="LogLevels.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
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
                .f(() => output = baufile.Run());

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
    }
}
