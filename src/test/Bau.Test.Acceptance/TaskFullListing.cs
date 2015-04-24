// <copyright file="TaskFullListing.cs" company="Bau contributors">
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

    public static class TaskFullListing
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>();"));

            "When I execute the baufile for a listing"
                .f(() => output = baufile.Run("-A"));

            "Then the output should be empty"
                .f(() => output.Should().BeEmpty());
        }

        [Scenario]
        public static void OneTask(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with one task"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>()
.Task(""some-task"");"));

            "When I execute the baufile for a listing"
                .f(() => output = baufile.Run("-A"));

            "Then the output should end with a task name"
                .f(() => output.Should().EndWith("some-task"));
        }

        [Scenario]
        public static void OneTaskWithDescription(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a described task"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>()
.Task(""some-task"")
.Desc(""Some long description."");"));

            "When I execute the baufile for a listing"
                .f(() => output = baufile.Run("-A"));

            "Then the output should end with a task name and no description"
                .f(() => output.Should().EndWith("some-task"));
        }

        [Scenario]
        public static void TwoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a couple tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>()
.Task(""task1"")
.Task(""task2"");"));

            "When I execute the baufile for a listing"
                .f(() => output = baufile.Run("-A"));

            "Then the output should end both tasks"
                .f(() => output.Should().EndWith("task1" + Environment.NewLine + "task2"));
        }

        [Scenario]
        public static void TaskWithSpaceInTheName(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with a task named with whitespace"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>()
.Task(""some task"");"));

            "When I execute the baufile for a listing"
                .f(() => output = baufile.Run("-A"));

            "Then the output should end with a quoted task name"
                .f(() => output.Should().EndWith(@"""some task"""));
        }
    }
}
