// <copyright file="TaskListingPrerequisites.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System.Reflection;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class TaskListingPrerequisites
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>();"));

            "When I execute the baufile for a prereq listing"
                .f(() => output = baufile.Run("-P"));

            "Then the output should be empty"
                .f(() => output.TrimEnd().Should().EndWith("Terminating packs"));
        }

        [Scenario]
        public static void OneTask(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with one task"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"");"));

            "When I execute the baufile for a prereq listing"
                .f(() => output = baufile.Run("-P"));

            "Then the output should end with a task name"
                .f(() => output.TrimEnd().Should().EndWith("some-task"));
        }

        [Scenario]
        public static void TwoTasksLinearDep(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks dependant upon eachother"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.DependsOn(""some - other - task"")
.Task(""some-other-task"");"));

            "When I execute the baufile for a prereq listing"
                .f(() => output = baufile.Run("-P"));

            "Then the output should end with the two tasks where one has a dep"
                .f(() => output.TrimEnd().Should().EndWith(
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
.Task(""some-task2"");"));

            "When I execute the baufile for a prereq listing"
                .f(() => output = baufile.Run("-P"));

            "Then the output should end with the two tasks"
                .f(() => output.TrimEnd().Should().EndWith(
@"some-task1
some-task2"));
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
.Task(""restore"");"));

            "When I execute the baufile for a prereq listing"
                .f(() => output = baufile.Run("-P"));

            "Then the output should look like the sample Adam gave"
                .f(() => output.TrimEnd().Should().EndWith(
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
