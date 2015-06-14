// <copyright file="TaskListAll.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Reflection;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class TaskListAll
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"Require<Bau>().Run();"));

            "When I list all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should end normally"
                .f(() => output.TrimEnd().Should().EndWith("Terminating packs"));
        }

        [Scenario]
        public static void OneTask(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with one task"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.Run();"));

            "When I list all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should show a task name"
                .f(() => output.Should().Contain("some-task"));
        }

        [Scenario]
        public static void OneTaskWithDescription(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a described task"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.Desc(""Some long description."")
.Run();"));

            "When I list all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should show the task and description"
                .f(() => output.Should().Contain("some-task # Some long description."));
        }

        [Scenario]
        public static void TwoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a couple tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""task1"")
.Task(""task2"")
.Run();"));

            "When I list all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should contain both tasks"
                .f(() => output.Should()
                    .Contain("task1" + Environment.NewLine + "task2"));
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

            "When I list all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should end both tasks"
                .f(() => output.Should()
                    .Contain("some-task1 # Some description." + Environment.NewLine + "some-task2"));
        }

        [Scenario]
        public static void TaskWithSpaceInTheName(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a task named with whitespace"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some task"")
.Run();"));

            "When I list all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should contain the quoted task name"
                .f(() => output.Should().Contain(@"""some task"""));
        }
    }
}
