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
        public static void ExecutingACommand(Baufile baufile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which succeeds"
                .f(() => baufile = Baufile.Create(scenario, true).WriteLine(
@"Require<Bau>()
.Task<Exec>(""default"")
.Do(exec =>
{
    exec.Command = @""..\Bau.Test.Acceptance.CreateFile.exe"";
    exec.Args = new[] { ""foo.txt"" };
    exec.WorkingDirectory = @""" + scenario + @""";
})
.Run();"));

            "When I execute the baufile with the debug option"
                .f(() => output = baufile.Run("-d"));

            "Then the task succeeds"
                .f(() => File.Exists(Path.Combine(Baufile.Directory, scenario, "foo.txt")).Should().BeTrue());

            "And I the command details are logged at debug level"
                .f(() => output.Should().MatchEquivalentOf(
                    @"*[default] *DEBUG: *'..\Bau.Test.Acceptance.CreateFile.exe foo.txt' * 'Bau.Test.Acceptance.Plugins.Exec.ExecutingACommand'*"));
        }

        [Scenario]
        public static void ExecutingACommandUsingTheExtensionMethod(Baufile baufile)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which uses the Exec extension method"
                .f(() => baufile = Baufile.Create(scenario, true).WriteLine(
@"Require<Bau>()
.Exec(""default"")
.Do(exec =>
{
    exec.Command = @""..\Bau.Test.Acceptance.CreateFile.exe"";
    exec.Args = new[] { ""foo.txt"" };
    exec.WorkingDirectory = @""" + scenario + @""";
})
.Run();"));

            "When I execute the baufile"
                .f(() => baufile.Run());

            "Then the task succeeds"
                .f(() => File.Exists(Path.Combine(Baufile.Directory, scenario, "foo.txt")).Should().BeTrue());
        }

        [Scenario]
        public static void ExecutingACommandUsingFluentSyntax(Baufile baufile)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which uses the Exec extension method"
                .f(() => baufile = Baufile.Create(scenario, true).WriteLine(
@"Require<Bau>()
.Exec(""default"")
.Do(exec => exec
    .Run(@""..\Bau.Test.Acceptance.CreateFile.exe"")
    .With(""foo.txt"")
    .In(@""" + scenario + @"""))
.Run();"));

            "When I execute the baufile"
                .f(() => baufile.Run());

            "Then the task succeeds"
                .f(() => File.Exists(Path.Combine(Baufile.Directory, scenario, "foo.txt")).Should().BeTrue());
        }

        [Scenario]
        public static void ExecutionFails(Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with an exec task which fails"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Exec(""default"")
.Do(exec => exec.Command = @""..\Bau.Test.Acceptance.CreateFile.exe"")
.Run();"));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Run()));

            "Then execution fails"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the task failed"
                .f(() => ex.Message.Should().ContainEquivalentOf("'default' task failed"));

            "And I am informed that the command exited with a non-zero exit code"
                .f(() =>
                {
                    ex.Message.Should().ContainEquivalentOf("the command exited with code ");
                    ex.Message.Should().NotContainEquivalentOf("the command exited with code 0");
                });
        }
    }
}
