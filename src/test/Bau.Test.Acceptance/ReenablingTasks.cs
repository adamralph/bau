// <copyright file="ReenablingTasks.cs" company="Bau contributors">
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

    public static class ReenablingTasks
    {
        [Scenario]
        public static void ReenablingATask(Baufile baufile, string tempFile, string[] executedTasks)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given bau is required"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"var bau = Require<Bau>();"));

            "And a non-default task which depends on and reenables another non-default task"
                .f(c => baufile.WriteLine(
@"
var executed = new List<string>();

bau.Task(""non-default"").DependsOn(""other-non-default"")
.Do(() =>
{
    bau.Reenable(""other-non-default"");
    executed.Add(""non-default"");
});"));

            "And another non-default task"
                .f(c => baufile.WriteLine(
@"
bau.Task(""other-non-default"")
.Do(() => executed.Add(""other-non-default""));"));

            "And a default task which depends on both non-default tasks"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile.WriteLine(
@"
bau.Task(""default"")
.DependsOn(""non-default"", ""other-non-default"")
.Do(() =>
{
    executed.Add(""default"");
    using(var file = File.CreateText(@""" + tempFile + @"""))
    {
        file.Write(string.Join(Environment.NewLine, executed));
    };
});");
                })
                .Teardown(() => File.Delete(tempFile));

            "And the tasks are executed"
                .f(() => baufile.WriteLine(
@"
bau.Run();"));

            "When I execute the baufile"
                .f(() => baufile.Run());

            "Then four tasks are executed"
                .f(() =>
                {
                    File.Exists(tempFile).Should().BeTrue();
                    executedTasks = File.ReadAllText(tempFile).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    executedTasks.Length.Should().Be(4);
                });

            "And the other non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("other-non-default"));

            "And the non-default task is executed second"
                .f(() => executedTasks[1].Should().Be("non-default"));

            "And the other non-default task is executed third for the second time"
                .f(() => executedTasks[2].Should().Be("other-non-default"));

            "And the default task is executed fourth"
                .f(() => executedTasks[3].Should().Be("default"));
        }
    }
}
