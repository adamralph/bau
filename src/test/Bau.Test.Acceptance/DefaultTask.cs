// <copyright file="DefaultTask.cs" company="Bau contributors">
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
    using Xunit;

    public static class DefaultTask
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by xBehave.net.")]
        [Scenario]
        public static void DefaultTaskExists(Baufile baufile, string file, string output)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile with a default task"
                .f(c =>
                {
                    file = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).Using(c).WriteLine(
@"var bau = Require<BauPack>();
bau
    .Task(""default"")
    .Do(() => File.Create(@""" + file + @""").Dispose());

bau.Execute();");
                })
                .Teardown(() => File.Delete(file));

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then the task is executed"
                .f(() => File.Exists(file).Should().BeTrue());

            "And I am informed that the default task was executed"
                .f(() => output.Should().Contain("Executing 'default' Bau task."));
        }

        [Scenario]
        [Example(
@"Require<BauPack>().Execute();")]
        [Example(
@"var bau = Require<BauPack>();
bau.Task(""foo"").Do(() => { });
bau.Execute();")]
        public static void DefaultTaskDoesNotExist(string code, Baufile baufile, Exception ex)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile containing: {0}"
                .f(c => baufile = Baufile.Create(scenario).Using(c).WriteLine(code));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Execute()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the default task was not found"
                .f(() => ex.Message.Should().Contain("'default' task not found"));
        }
    }
}
