// <copyright file="InputsOutputs.cs" company="Bau contributors">
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

    public static class InputsOutputs
    {
        [Scenario]
        public static void InputDoesNotExistNoOutputs(Baufile baufile, string inputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                {
                    inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
                        @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }

        [Scenario]
        public static void InputExistsNoOutputs(Baufile baufile, string inputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                {
                    inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    File.Create(inputFile).Dispose();
                    baufile = Baufile.Create(scenario).WriteLine(
                        @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile))
                .Teardown(() => File.Delete(inputFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }

        [Scenario]
        public static void NoInputsOutputDoesNotExist(Baufile baufile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                {
                    outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
                        @"Require<Bau>().Task(""default"").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }

        [Scenario]
        public static void NoInputsOutputExists(Baufile baufile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                    {
                        outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        File.Create(outputFile).Dispose();
                        baufile = Baufile.Create(scenario).WriteLine(
                            @"Require<Bau>().Task(""default"").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                    })
                .Teardown(() => File.Delete(tempFile))
                .Teardown(() => File.Delete(outputFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }

        [Scenario]
        public static void InputDoesNotExistOutputDoesNotExist(Baufile baufile, string inputFile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                    {
                        inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        baufile = Baufile.Create(scenario).WriteLine(
                            @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                    })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is NOT executed"
                .f(() => File.Exists(tempFile).Should().BeFalse());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("skipping 'default'"));
        }

        [Scenario]
        public static void InputExistsOutputDoesNotExist(Baufile baufile, string inputFile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                    {
                        inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        File.Create(inputFile).Dispose();
                        baufile = Baufile.Create(scenario).WriteLine(
                            @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                    })
                .Teardown(() => File.Delete(tempFile))
                .Teardown(() => File.Delete(inputFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }

        [Scenario]
        public static void InputDoesNotExistOutputExists(Baufile baufile, string inputFile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                    {
                        inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        File.Create(outputFile).Dispose();
                        baufile = Baufile.Create(scenario).WriteLine(
                            @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                    })
                .Teardown(() => File.Delete(tempFile))
                .Teardown(() => File.Delete(outputFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is NOT executed"
                .f(() => File.Exists(tempFile).Should().BeFalse());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("skipping 'default'"));
        }

        [Scenario]
        public static void InputIsOlderThanOutput(Baufile baufile, string inputFile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                    {
                        inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        File.Create(inputFile).Dispose();
                        File.SetLastWriteTimeUtc(inputFile, DateTime.UtcNow.AddDays(-1));
                        File.Create(outputFile).Dispose();
                        baufile = Baufile.Create(scenario).WriteLine(
                            @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                    })
                .Teardown(() => File.Delete(tempFile))
                .Teardown(() => File.Delete(inputFile))
                .Teardown(() => File.Delete(outputFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is NOT executed"
                .f(() => File.Exists(tempFile).Should().BeFalse());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("skipping 'default'"));
        }

        [Scenario]
        public static void InputIsNewerThanOutput(Baufile baufile, string inputFile, string outputFile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a task that declares an input but no output"
                .f(() =>
                    {
                        inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                        File.Create(inputFile).Dispose();
                        File.Create(outputFile).Dispose();
                        File.SetLastWriteTimeUtc(outputFile, DateTime.UtcNow.AddDays(-1));
                        baufile = Baufile.Create(scenario).WriteLine(
                            @"Require<Bau>().Task(""default"").InputFile(@""" + inputFile + @""").OutputFile(@""" + outputFile + @""").Do(() => File.Create(@""" + tempFile + @""").Dispose()).Run();");
                    })
                .Teardown(() => File.Delete(tempFile))
                .Teardown(() => File.Delete(inputFile))
                .Teardown(() => File.Delete(outputFile));

            "When I execute the task"
                .f(() => output = baufile.Run());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I am informed that the task was started"
                .f(() => output.Should().ContainEquivalentOf("starting 'default'"));
        }
    }
}
