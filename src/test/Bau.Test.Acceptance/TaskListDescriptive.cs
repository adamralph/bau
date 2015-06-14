// <copyright file="TaskListDescriptive.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Reflection;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class TaskListDescriptive
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"Require<Bau>().Run();"));

            "When I list the tasks"
                .f(() => output = baufile.Run("-T"));

            "Then the output should end normally"
                .f(() => output.TrimEnd().Should()
                    .EndWith("Terminating packs"));
        }

        [Scenario]
        public static void OneTaskWithoutDescription(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with one task missing a description"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.Run();"));

            "When I list the tasks"
                .f(() => output = baufile.Run("-T"));

            "Then the output should not list the task"
                .f(() => output.TrimEnd().Should()
                    .NotContain("some-task"));
        }

        [Scenario]
        public static void OneTaskWithAndOneWithoutDescription(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks where one has a description"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task1"").Desc(""Some description."")
.Task(""some-task2"")
.Run();"));

            "When I list the tasks"
                .f(() => output = baufile.Run("-T"));

            "Then the output should show the described task with description"
                .f(() => output.Should()
                    .Contain("some-task1 # Some description."));

            "But not show the other task"
                .f(() => output.Should()
                    .NotContain("some-task2"));
        }

        [Scenario]
        public static void TwoTasksWithDescriptions(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks with descriptions"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task1"").Desc(""Some description."")
.Task(""some-task2"").Desc(""Another description."")
.Run();"));

            "When I list the tasks"
                .f(() => output = baufile.Run("-T"));

            "Then the output should show show both tasks with descriptions"
                .f(() => output.Should().Contain(
                    "some-task1 # Some description."
                    + Environment.NewLine
                    + "some-task2 # Another description."));
        }
    }
}
