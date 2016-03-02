// <copyright file="TaskAliases.cs" company="Bau contributors">
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

    public static class TaskAliases
    {
        [Scenario]
        public static void AliasedTask(Baufile baufile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a non-default task with an alias"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>().Task(""non-default"").WithAliases(""nd"").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the non-default task using the alias"
                .f(() => output = baufile.Run("nd"));

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task and dependencies are being run"
                .f(() => output.Should().ContainEquivalentOf("Running 'nd' and dependencies"));

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'nd' ('non-default')"));

            "And I am informed that the task was finished after a period of time"
                .f(() => output.Should().ContainEquivalentOf("finished 'nd' ('non-default') after "));

            "And I am informed that the task and dependencies were completed after a period of time"
                .f(() => output.Should().ContainEquivalentOf("Completed 'nd' and dependencies in "));
        }

        [Scenario]
        public static void DuplicateTaskAliases(string tag, string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with two tasks with the same alias"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>().Task(""foo"").WithAliases(""nd"").Do(() => { }).Task(""bar"").WithAliases(""nd"").Do(() => { }).Run();"));

            "When I try and execute a task using that alias"
                .f(() => ex = Record.Exception(() => baufile.Run("nd")));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the alias was used more than once"
                .f(() => ex.Message.Should().ContainEquivalentOf(
                    "The 'nd' task alias cannot be used for 'bar' because it has already been used for 'foo'"));
        }
    }
}
