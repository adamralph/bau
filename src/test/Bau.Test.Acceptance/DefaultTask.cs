// <copyright file="DefaultTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Bau.Test.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class DefaultTask
    {
        [Scenario]
        public static void DefaultTaskExists(Baufile baufile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a default task"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
                        @"Require<Bau>().Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the baufile"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the default task and dependencies are being run"
                .f(() => output.Should().ContainEquivalentOf("Running 'default' and dependencies"));

            "And I am informed that the default task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));

            "And I am informed that the default task was finished after a period of time"
                .f(() => output.Should().ContainEquivalentOf("finished 'default' after "));

            "And I am informed that the default task and dependencies were completed after a period of time"
                .f(() => output.Should().ContainEquivalentOf("Completed 'default' and dependencies in "));
        }

        [Scenario]
        [Example("NoTask", @"Require<Bau>().Run();")]
        [Example("SomeOtherTask", @"Require<Bau>().Task(""foo"").Do(() => { }).Run();")]
        public static void DefaultTaskDoesNotExist(string tag, string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile containing {0}"
                .f(() => baufile = Baufile.Create(string.Concat(scenario, ".", tag)).WriteLine(code));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Run()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the default task was not found"
                .f(() => ex.Message.Should().ContainEquivalentOf("'default' task not found"));
        }
    }
}
