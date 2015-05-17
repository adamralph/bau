// <copyright file="TaskListingJson.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System.Reflection;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class TaskListingJson
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>(); bau.Run();"));

            "When I execute the baufile for a JSON listing"
                .f(() => output = baufile.Run("-J"));

            "Then the output should be an empty task array"
                .f(() => output.TrimEnd().Should().EndWith(
@"{
    ""tasks"": [
    ]
}"));
        }

        [Scenario]
        public static void OneTaskWithoutDescriptionOrDep(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.Run();"));

            "When I execute the baufile for a JSON listing"
                .f(() => output = baufile.Run("-J"));

            "Then the output should contain information for that task"
                .f(() => output.TrimEnd().Should().EndWith(
@"{
    ""tasks"": [
        {
            ""name"": ""some-task"",
            ""description"": null,
            ""dependencies"": [
            ]
        }
    ]
}"));
        }

        [Scenario]
        public static void TwoWithRelationship(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task2"")
.Desc(""Second task"")
.DependsOn(""some-task1"")
.Task(""some-task1"")
.Desc(""First task"")
.Run();"));

            "When I execute the baufile for a JSON listing"
                .f(() => output = baufile.Run("-J"));

            "Then the output should have the two tasks described correctly"
                .f(() => output.TrimEnd().Should().EndWith(
@"{
    ""tasks"": [
        {
            ""name"": ""some-task1"",
            ""description"": ""First task"",
            ""dependencies"": [
            ]
        },
        {
            ""name"": ""some-task2"",
            ""description"": ""Second task"",
            ""dependencies"": [
                ""some-task1""
            ]
        }
    ]
}"));
        }
    }
}
