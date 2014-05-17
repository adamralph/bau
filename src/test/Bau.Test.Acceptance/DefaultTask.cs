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
                        @"Require<Bau>().Do(() => File.Create(@""" + tempFile + @""").Dispose()).Execute();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the default task was executed"
                .f(() => output.Should().ContainEquivalentOf("executing 'default'"));

            "And I am informed about execution time"
                .f(() => output.Should().ContainEquivalentOf("finished 'default' after "));
        }

        [Scenario]
        [Example("NoTask", @"Require<Bau>().Execute();")]
        [Example("SomeOtherTask", @"Require<Bau>().Task(""foo"").Do(() => { }).Execute();")]
        public static void DefaultTaskDoesNotExist(string tag, string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile containing {0}"
                .f(() => baufile = Baufile.Create(string.Concat(scenario, ".", tag)).WriteLine(code));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Execute()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the default task was not found"
                .f(() => ex.Message.Should().ContainEquivalentOf("'default' task not found"));
        }
    }
}
