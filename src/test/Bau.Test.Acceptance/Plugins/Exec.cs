// <copyright file="Exec.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance.Plugins
{
    using System;
    using System.IO;
    using System.Reflection;
    using Bau.Test.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class Exec
    {
        [Scenario]
        public static void ExecutionSucceeds(Baufile baufile, string createdFile, string output)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which succeeds"
                .f(() =>
                {
                    baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Task<Exec>(""default"")
.Do(exec =>
{
    exec.WorkingDirectory = @""working"";
    exec.Command = @""..\Bau.Test.Acceptance.CreateFile.exe"";
    exec.Args = new[] { ""foo.txt"" };
})
.Execute();");

                    var workingFolder = Path.Combine(scenario, "working");
                    FileSystem.CreateDirectory(workingFolder);
                    createdFile = Path.Combine(workingFolder, "foo.txt");
                });

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then the task succeeds"
                .f(() => File.Exists(createdFile).Should().BeTrue());
        }

        [Scenario]
        public static void ExtensionMethod(Baufile baufile, string createdFile, string output)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which uses the Exec extension method"
                .f(() =>
                {
                    baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Exec(""default"")
.Do(exec =>
{
    exec.WorkingDirectory = @""working"";
    exec.Command = @""..\Bau.Test.Acceptance.CreateFile.exe"";
    exec.Args = new[] { ""foo.txt"" };
})
.Execute();");

                    var workingFolder = Path.Combine(scenario, "working");
                    FileSystem.CreateDirectory(workingFolder);
                    createdFile = Path.Combine(workingFolder, "foo.txt");
                });

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then the task succeeds"
                .f(() => File.Exists(createdFile).Should().BeTrue());
        }

        [Scenario]
        public static void ExecutionFails(Baufile baufile, Exception ex)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which fails"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>().Task<Exec>().Do(exec => exec.Command = @""..\Bau.Test.Acceptance.CreateFile.exe"").Execute();"));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Execute()));

            "Then execution fails"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the task failed"
                .f(() => ex.Message.Should().Contain("'default' task failed"));

            "And I am informed that the command exited with a non-zero exit code"
                .f(() =>
                {
                    ex.Message.Should().ContainEquivalentOf("the command exited with code ");
                    ex.Message.Should().NotContainEquivalentOf("the command exited with code 0");
                });
        }
    }
}
