// <copyright file="SpecificTasks.cs" company="Bau contributors">
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

    public static class SpecificTasks
    {
        [Scenario]
        public static void SingleTask(Baufile baufile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a non-default task"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>().Task(""non-default"").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Execute();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the non-default task"
                .f(() => output = baufile.Execute("non-default"));

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was executed"
                .f(() => output.Should().ContainEquivalentOf("executing 'non-default'"));
        }

        [Scenario]
        public static void MultipleTasks(Baufile baufile, string tempFile1, string tempFile2, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>();"));

            "And a non-default task"
                .f(() =>
                {
                    tempFile1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile.WriteLine(
@"
bau.Task(""non-default1"").Do(() => File.Create(@""" + tempFile1 + @""").Dispose());");
                })
                .Teardown(() => File.Delete(tempFile1));

            "And another non-default task"
                .f(() =>
                {
                    tempFile2 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile.WriteLine(
@"
bau.Task(""non-default2"").Do(() => File.Create(@""" + tempFile2 + @""").Dispose());");
                })
                .Teardown(() => File.Delete(tempFile2));
            "And the tasks are executed"
                .f(() => baufile.WriteLine(
@"
bau.Execute();"));

            "When I execute both non-default tasks"
                .f(() => output = baufile.Execute("non-default1", "non-default2"));

            "Then the first task is executed"
                .f(() => File.Exists(tempFile1).Should().BeTrue());

            "And the second task is executed"
                .f(() => File.Exists(tempFile2).Should().BeTrue());

            "And I am informed that the first task was executed"
                .f(() => output.Should().ContainEquivalentOf("executing 'non-default1'"));

            "And I am informed that the second task was executed"
                .f(() => output.Should().ContainEquivalentOf("executing 'non-default2'"));
        }

        [Scenario]
        [Example("NoTask", @"Require<Bau>().Execute();")]
        [Example("SomeOtherTask", @"Require<Bau>().Task(""foo"").Do(() => { }).Execute();")]
        public static void NonexistentTask(string tag, string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile containing {0}"
                .f(() => baufile = Baufile.Create(string.Concat(scenario, ".", tag)).WriteLine(code));

            "When I execute a non-existent task"
                .f(() => ex = Record.Exception(() => baufile.Execute("non-existent")));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the non-existent task was not found"
                .f(() => ex.Message.Should().ContainEquivalentOf("'non-existent' task not found"));
        }
    }
}
