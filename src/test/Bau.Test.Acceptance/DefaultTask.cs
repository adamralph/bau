// <copyright file="DefaultTask.cs" company="Bau contributors">
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

    public static class DefaultTask
    {
        [Scenario]
        public static void DefaultTaskExists(BauFile file, string tempFile, string output)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile with a default task which creates a file"
                .f(c =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    file = BauFile.Create(
                        @"Require<BauPack>().Task(""default"", () => File.Create(@""" + tempFile + @""").Dispose());",
                        scenario).Using(c);
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the baufile"
                .f(() => output = file.Execute());

            "Then I am informed that the default task is being executed"
                .f(() => output.Should().Contain("Executing 'default' Bau task."));

            "And the temporary file exists"
                .f(() => File.Exists(tempFile).Should().BeTrue());
        }

        [Scenario]
        [Example(@"Require<BauPack>();")]
        [Example(@"Require<BauPack>().Task(""foo"", () => { });")]
        public static void DefaultTaskDoesNotExist(string code, BauFile file, string bauFilePath, Exception ex)
        {
            var scenario = MethodInfo.GetCurrentMethod().GetFullName();

            "Given a baufile in the folder containing: {0}"
                .f(() => file = BauFile.Create("Require<BauPack>();", scenario));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => file.Execute()));

            "Then I am informed that the default task was not found"
                .f(() => ex.Should().NotBeNull().And.Subject.As<Exception>().Message.Should().Contain("'default' task not found"));
        }
    }
}
