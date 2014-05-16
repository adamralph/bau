// <copyright file="InlineTasks.cs" company="Bau contributors">
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

    public static class InlineTasks
    {
        [Scenario]
        public static void ExecutingAnInlineTask(Baufile baufile, string tempFile, string output)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile with a default task with an inline task"
                .f(() =>
                {
                    tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
                    baufile = Baufile.Create(scenario).WriteLine(
@"Require<Bau>()
.Do(() =>
{
    var task = new BauCore.Task();
    task.Actions.Add(() => File.Create(@""" + tempFile + @""").Dispose());
    task.Execute();
})
.Execute();");
                })
                .Teardown(() => File.Delete(tempFile));

            "When I execute the baufile"
                .f(() => output = baufile.Execute());

            "Then the task is executed"
                .f(() => File.Exists(tempFile).Should().BeTrue());

            "And I should not be informed that a task with no name was executed"
                .f(() => output.Should().NotContain("Executing '' Bau task."));
        }
    }
}
