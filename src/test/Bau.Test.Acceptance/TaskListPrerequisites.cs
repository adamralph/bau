// <copyright file="TaskListPrerequisites.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System.Reflection;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class TaskListPrerequisites
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>(); bau.Run();"));

            "When I list the tasks including prerequisites"
                .f(() => output = baufile.Run("-P"));

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

            "When I list the tasks including prerequisites"
                .f(() => output = baufile.Run("-P"));

            "Then the output should end with a task name"
                .f(() => output.Should().Contain("some-task"));
        }

        [Scenario]
        public static void TwoTasksLinearDep(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks with a relationship"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.DependsOn(""some-other-task"")
.Task(""some-other-task"")
.Run();"));

            "When I list the tasks including prerequisites"
                .f(() => output = baufile.Run("-P"));

            "Then the output should contain the two tasks where one has a dep"
                .f(() => output.Should().Contain(
@"some-other-task
some-task
    some-other-task"));
        }

        [Scenario]
        public static void TwoTasksWithNoDep(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task1"")
.Task(""some-task2"")
.Run();"));

            "When I list the tasks including prerequisites"
                .f(() => output = baufile.Run("-P"));

            "Then the output should show the two tasks"
                .f(() => output.Should().Contain(
@"some-task1
some-task2"));
        }

        [Scenario]
        public static void TasksWithDescriptions(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks with a relationship and descriptions"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.Desc(""Some description."")
.DependsOn(""some-other-task"")
.Task(""some-other-task"")
.Desc(""Some other description."")
.Run();"));

            "When I list the tasks including prerequisites"
                .f(() => output = baufile.Run("-P"));

            "Then the output should contain the two described tasks where one has a dep"
                .f(() => output.Should().Contain(
@"some-other-task # Some other description.
some-task       # Some description.
    some-other-task"));
        }

        [Scenario]
        public static void LargeishSampleFromAdam(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a bunch of tasks from xbehave"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""accept"")
    .DependsOn(""build"")
.Task(""artifacts"")
.Task(""artifacts/logs"")
.Task(""artifacts/output"")
.Task(""build"")
    .DependsOn(""clean"")
    .DependsOn(""restore"")
    .DependsOn(""artifacts/logs"")
.Task(""clean"")
    .DependsOn(""artifacts/logs"")
.Task(""component"")
    .DependsOn(""build"")
.Task(""default"")
    .DependsOn(""component"")
    .DependsOn(""accept"")
    .DependsOn(""pack"")
.Task(""pack"")
    .DependsOn(""build"")
    .DependsOn(""artifacts/output"")
.Task(""restore"")
.Run();"));

            "When I list the tasks including prerequisites"
                .f(() => output = baufile.Run("-P"));

            "Then the output should look like the sample Adam gave"
                .f(() => output.Should().Contain(
@"accept
    build
artifacts
artifacts/logs
artifacts/output
build
    clean
    restore
    artifacts/logs
clean
    artifacts/logs
component
    build
default
    component
    accept
    pack
pack
    build
    artifacts/output
restore"));
        }
    }
}
