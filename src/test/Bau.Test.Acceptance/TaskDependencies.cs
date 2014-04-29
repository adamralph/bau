// <copyright file="TaskDependencies.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Bau.Test.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;

    public static class TaskDependencies
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by xBehave.net.")]
        [Scenario]
        public static void SingleDependency(Baufile baufile, string tempFile, string[] excutedTasks, string output)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given bau is required"
                .f(c => baufile = Baufile.Create(scenario).Using(c).WriteLine(
@"var bau = Require<BauPack>();"));

            "And a non-default task"
                .f(c => baufile.WriteLine(
@"
var executed = new List<string>();

bau
    .Task(""non-default"")
    .Do(() => executed.Add(""non-default""));"));

            "And a default task which depends on the non-default task"
                .f(() =>
            {
                tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                baufile.WriteLine(
@"bau
    .Task(""default"")
    .DependsOn(""non-default"")
    .Do(() =>
    {
        executed.Add(""default"");
        using(var file = File.CreateText(@""" + tempFile + @"""))
        {
            file.Write(string.Join(Environment.NewLine, executed));

        };
    });");
            });

            "And the tasks are executed"
                .f(() => baufile.WriteLine(
@"bau.Execute();"));

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then two tasks are executed"
                .f(() =>
                {
                    File.Exists(tempFile).Should().BeTrue();
                    excutedTasks = File.ReadAllText(tempFile).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    excutedTasks.Length.Should().Be(2);
                });

            "And the non-default task is executed first"
                .f(() => excutedTasks[0].Should().Be("non-default"));

            "And the default task is executed second"
                .f(() => excutedTasks[1].Should().Be("default"));

            "And I am informed that the non-default task was executed"
                .f(() => output.Should().Contain("Executing 'non-default' Bau task."));

            "And I am informed that the default task was executed"
                .f(() => output.Should().Contain("Executing 'default' Bau task."));
        }
    }
}
