// <copyright file="ExecuteATaskFromATask.cs" company="Bau contributors">
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

    public static class ExecuteATaskFromATask
    {
        // happy path
        [Scenario]
        public static void ExecutingATaskFromAnotherTask(Baufile baufile, string tempFile, string[] executedTasks, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var executed = new List<string>();

var bau = Require<Bau>();"));

            "And a non-default task"
                .f(c => baufile.WriteLine(
@"bau.Task(""non-default"")
.Do(() => executed.Add(""non-default""))"));

            "And a default task which executes the non-default task"
                .f(() =>
            {
                tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                baufile.WriteLine(
@".Task(""default"")
.Do(() =>
{
    bau.Execute(""non-default"");
    executed.Add(""default"");
    using(var file = File.CreateText(@""" + tempFile + @"""))
    {
        file.Write(string.Join(Environment.NewLine, executed));
    };
})");
            })
            .Teardown(() => File.Delete(tempFile));

            "And the tasks are executed"
                .f(() => baufile.WriteLine(
@".Execute();"));

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then two tasks are executed"
                .f(() =>
                {
                    File.Exists(tempFile).Should().BeTrue();
                    executedTasks = File.ReadAllText(tempFile).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    executedTasks.Length.Should().Be(2);
                });

            "And the non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("non-default"));

            "And the default task is executed second"
                .f(() => executedTasks[1].Should().Be("default"));

            "And I am informed that the non-default task was executed"
                .f(() => output.Should().ContainEquivalentOf("starting 'non-default'"));

            "And I am informed that the default task was executed"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }

        [Scenario]
        public static void ExecutingATaskFromAnotherTaskWithDependencies(Baufile baufile, string tempFile, string[] executedTasks, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var executed = new List<string>();

var bau = Require<Bau>();"));

            "And a non-default task"
                .f(c => baufile.WriteLine(
@"bau.Task(""first-non-default"")
.DependsOn(""second-non-default"")
.Do(() => executed.Add(""first-non-default""))
.Task(""second-non-default"")
.Do(() => executed.Add(""second-non-default""))"));

            "And a default task which executes the first-non-default task"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile.WriteLine(
    @".Task(""default"")
.Do(() =>
{
    bau.Invoke(""first-non-default"");
    executed.Add(""default"");
    using(var file = File.CreateText(@""" + tempFile + @"""))
    {
        file.Write(string.Join(Environment.NewLine, executed));
    };
})");
                })
            .Teardown(() => File.Delete(tempFile));

            "And the tasks are executed"
                .f(() => baufile.WriteLine(
@".Execute();"));

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then three tasks are executed"
                .f(() =>
                {
                    File.Exists(tempFile).Should().BeTrue();
                    executedTasks = File.ReadAllText(tempFile).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    executedTasks.Length.Should().Be(3);
                });

            "And the second-non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("second-non-default"));

            "And the first-non-default task is executed second"
                .f(() => executedTasks[1].Should().Be("first-non-default"));

            "And the default task is executed third"
                .f(() => executedTasks[2].Should().Be("default"));

            "And I am informed that the second-non-default task was executed"
               .f(() => output.Should().ContainEquivalentOf("starting 'second-non-default'"));

            "And I am informed that the first-non-default task was executed"
                .f(() => output.Should().ContainEquivalentOf("starting 'first-non-default'"));

            "And I am informed that the default task was executed"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }
    }
}
