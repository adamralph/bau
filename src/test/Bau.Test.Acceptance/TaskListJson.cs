// <copyright file="TaskListJson.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System.Reflection;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class TaskListJson
    {
        [Scenario]
        public static void NoTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>(); bau.Run();"));

            "When I list the tasks as JSON"
                .f(() => output = baufile.Run("-J"));

            "Then the output should be an empty task array"
                .f(() => output.Should().Contain(
@"{
    ""tasks"": [
    ]
}"));
        }

        [Scenario]
        public static void OneTaskWithoutDescriptionOrDep(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required with one simple task"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task"")
.Run();"));

            "When I list the tasks as JSON"
                .f(() => output = baufile.Run("-J"));

            "Then the output should contain information for that task"
                .f(() => output.Should().Contain(
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

            "Given bau is required with two described tasks having a relationship"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task2"")
.Desc(""Second task"")
.DependsOn(""some-task1"")
.Task(""some-task1"")
.Desc(""First task"")
.Run();"));

            "When I list the tasks as JSON"
                .f(() => output = baufile.Run("-J"));

            "Then the output should have the two tasks described correctly showing a dependency"
                .f(() => output.Should().Contain(
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
