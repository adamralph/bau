// <copyright file="TaskListingDescriptive.cs" company="Bau contributors">
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

    public static class TaskListingDescriptive
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>();"));

            "When I execute the baufile for a descriptive listing"
                .f(() => output = baufile.Run("-T"));

            "Then the output should be empty"
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
.Task(""some-task"");"));

            "When I execute the baufile for a descriptive listing"
                .f(() => output = baufile.Run("-T"));

            "Then the output should be empty"
                .f(() => output.TrimEnd().Should()
                    .EndWith("Terminating packs"));
        }

        [Scenario]
        public static void OneTaskWithAndOneWithoutDescription(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks where one has a description"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task1"").Desc(""Some description."")
.Task(""some-task2"");"));

            "When I execute the baufile for a descriptive listing"
                .f(() => output = baufile.Run("-T"));

            "Then the output should show only one task"
                .f(() => output.TrimEnd().Should()
                    .EndWith("some-task1 # Some description."));
        }

        [Scenario]
        public static void TwoTasksWithDescriptions(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with two tasks with descriptions"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task1"").Desc(""Some description."")
.Task(""some-task2"").Desc(""Another description."");"));

            "When I execute the baufile for a descriptive listing"
                .f(() => output = baufile.Run("-T"));

            "Then the output should show only one task"
                .f(() => output.TrimEnd().Should().EndWith(
                    "some-task1 # Some description."
                    + Environment.NewLine
                    + "some-task2 # Another description."));
        }
    }
}
