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
@"Require<Bau>().Task(""non-default"").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the non-default task"
                .f(() => output = baufile.Run("non-default"));

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task and dependencies are being run"
                .f(() => output.Should().ContainEquivalentOf("Running 'non-default' and dependencies"));

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'non-default'"));

            "And I am informed that the task was finished after a period of time"
                .f(() => output.Should().ContainEquivalentOf("finished 'non-default' after "));

            "And I am informed that the task and dependencies were completed after a period of time"
                .f(() => output.Should().ContainEquivalentOf("Completed 'non-default' and dependencies in "));
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
bau.Run();"));

            "When I execute both non-default tasks"
                .f(() => output = baufile.Run("non-default1", "non-default2"));

            "Then the first task is executed"
                .f(() => File.Exists(tempFile1).Should().BeTrue());

            "And the second task is executed"
                .f(() => File.Exists(tempFile2).Should().BeTrue());

            "And I am informed that the tasks and dependencies are being run"
                .f(() => output.Should().ContainEquivalentOf("Running 'non-default1', 'non-default2' and dependencies"));

            "And I am informed that the first task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'non-default1'"));

            "And I am informed that the second task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'non-default2'"));

            "And I am informed that the first task was finished after a period of time"
                .f(() => output.Should().ContainEquivalentOf("finished 'non-default1' after "));

            "And I am informed that the second task was finished after a period of time"
                .f(() => output.Should().ContainEquivalentOf("finished 'non-default2' after "));

            "And I am informed that the tasks and dependencies were completed after a period of time"
                .f(() => output.Should().ContainEquivalentOf("Completed 'non-default1', 'non-default2' and dependencies in "));
        }

        [Scenario]
        [Example("NoTask", @"Require<Bau>().Run();")]
        [Example("SomeOtherTask", @"Require<Bau>().Task(""foo"").Do(() => { }).Run();")]
        public static void NonexistentTask(string tag, string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile containing {0}"
                .f(() => baufile = Baufile.Create(string.Concat(scenario, ".", tag)).WriteLine(code));

            "When I execute a non-existent task"
                .f(() => ex = Record.Exception(() => baufile.Run("non-existent")));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the non-existent task was not found"
                .f(() => ex.Message.Should().ContainEquivalentOf("'non-existent' task not found"));
        }

        [Scenario]
        public static void AliasedTask(Baufile baufile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a non-default task with an alias set as nd"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
                    @"Require<Bau>().Task(""non-default"").WithAliases(""nd"").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the non-default task using an alias"
                .f(() => output = baufile.Run("nd"));

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task and dependencies are being run"
                .f(() => output.Should().ContainEquivalentOf("Running 'nd' and dependencies"));

            "And I am informed that I ran the task using an alias"
                .f(() => output.Should().ContainEquivalentOf("Actual task name 'non-default'"));

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'nd'"));

            "And I am informed that the task was finished after a period of time"
                .f(() => output.Should().ContainEquivalentOf("finished 'nd' after "));

            "And I am informed that the task and dependencies were completed after a period of time"
                .f(() => output.Should().ContainEquivalentOf("Completed 'nd' and dependencies in "));
        }

        [Scenario]
        [Example("MultipleTasksWithSameAlias", @"Require<Bau>().Task(""foo"")
                                                  .WithAliases(""nd"").Do(() => { })
                                                  .Task(""bar"")
                                                  .WithAliases(""nd"").Do(() => { })
                                                  .Run();")]
        public static void MultipleMatchingTasksWithanAlias(string tag, string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile containing {0}"
                .f(() => baufile = Baufile.Create(string.Concat(scenario, ".", tag)).WriteLine(code));

            "When I execute a task with an alias which is also used for another task"
                .f(() => ex = Record.Exception(() => baufile.Run("nd")));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the non-existent task was not found"
                .f(() => ex.Message.Should().ContainEquivalentOf("'nd' alias was assigned for multiple tasks"));
        }
    }
}
