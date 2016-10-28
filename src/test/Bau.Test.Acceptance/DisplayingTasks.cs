// <copyright file="DisplayingTasks.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Reflection;
    using System.Text;
    using FluentAssertions;
    using Support;
    using Xbehave;

    public static class DisplayingTasks
    {
        [Scenario]
        public static void DisplayingAllTasks(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given two tasks where only one has a description"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some task1"").Desc(""Some description."")
.Task(""some-task2"")
.Run();"));

            "When I display all tasks"
                .f(() => output = baufile.Run("-A"));

            "Then the output should contain both tasks"
                .f(() => output.Should()
                    .Contain("\"some task1\" # Some description." + Environment.NewLine + "some-task2"));
        }

        [Scenario]
        public static void DisplayingTasksWithDescriptions(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given three tasks where one does not have a description"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some task1"").Desc(""Some description."")
.Task(""some-task2"").Desc(""Some description."")
.Task(""some-task3"")
.Run();"));

            "When I display the tasks with descriptions"
                .f(() => output = baufile.Run("-T"));

            "Then the output should contain the first described task with its description"
                .f(() => output.Should()
                    .Contain("\"some task1\" # Some description."));

            "And the output should contain the second described task with its description"
                .f(() => output.Should()
                    .Contain("some-task2   # Some description."));

            "But the output should not contain the task with no description"
                .f(() => output.Should()
                    .NotContain("some-task3"));
        }

        [Scenario]
        public static void DisplayingNoTasksAsJson(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given no tasks"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"var bau = Require<Bau>(); bau.Run();"));

            "When I display the tasks as JSON"
                .f(() => output = baufile.Run("-J"));

            "Then the output should contain an empty task array"
                .f(() =>
                {
                    var expectedOutput = new StringBuilder()
                        .AppendLine("{")
                        .AppendLine("    \"tasks\": [")
                        .AppendLine("    ]")
                        .AppendLine("}")
                        .ToString();

                    output.Should().Contain(expectedOutput);
                });
        }

        [Scenario]
        public static void DisplayingTasksAsJson(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given two described tasks where on depends on the other"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""some-task2"")
    .Desc(""Second task"")
    .DependsOn(""some task1"")
.Task(""some task1"")
    .Desc(""First task"")
.Run();"));

            "When I display the tasks as JSON"
                .f(() => output = baufile.Run("-J"));

            "Then the output should contain the two tasks as JSON"
                .f(() =>
                {
                    var expectedOutput = new StringBuilder()
                        .AppendLine("{")
                        .AppendLine("    \"tasks\": [")
                        .AppendLine("        {")
                        .AppendLine("            \"name\": \"some task1\",")
                        .AppendLine("            \"description\": \"First task\",")
                        .AppendLine("            \"dependencies\": [")
                        .AppendLine("            ]")
                        .AppendLine("        },")
                        .AppendLine("        {")
                        .AppendLine("            \"name\": \"some-task2\",")
                        .AppendLine("            \"description\": \"Second task\",")
                        .AppendLine("            \"dependencies\": [")
                        .AppendLine("                \"some task1\"")
                        .AppendLine("            ]")
                        .AppendLine("        }")
                        .AppendLine("    ]")
                        .AppendLine("}")
                        .ToString();

                    output.Should().Contain(expectedOutput);
                });
        }

        [Scenario]
        public static void DisplayTasksAndPrequisites(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a task tree"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task(""accept"")
    .DependsOn(""build"")
.Task(""artifacts"")
.Task(""artifacts/logs"")
.Task(""artifacts/output"")
.Task(""build"")
    .DependsOn(""clean"")
    .DependsOn(""restore packages"")
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
.Task(""restore packages"")
.Run();"));

            "When I display the tasks with prerequisites"
                .f(() => output = baufile.Run("-P"));

            "Then the tasks should be listed with prerequisites"
                .f(() =>
                {
                    var expectedOutput = new StringBuilder()
                        .AppendLine("accept")
                        .AppendLine("    build")
                        .AppendLine("artifacts")
                        .AppendLine("artifacts/logs")
                        .AppendLine("artifacts/output")
                        .AppendLine("build")
                        .AppendLine("    clean")
                        .AppendLine("    restore packages")
                        .AppendLine("    artifacts/logs")
                        .AppendLine("clean")
                        .AppendLine("    artifacts/logs")
                        .AppendLine("component")
                        .AppendLine("    build")
                        .AppendLine("default")
                        .AppendLine("    component")
                        .AppendLine("    accept")
                        .AppendLine("    pack")
                        .AppendLine("pack")
                        .AppendLine("    build")
                        .AppendLine("    artifacts/output")
                        .AppendLine("\"restore packages\"")
                        .ToString();

                    output.Should().Contain(expectedOutput);
                });
        }
    }
}
