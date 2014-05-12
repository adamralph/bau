// <copyright file="ScriptExecution.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Acceptance
{
    using System;
    using System.Reflection;
    using Bau.Test.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class ScriptExecution
    {
        [Scenario]
        public static void CompilationFails(Baufile baufile, Exception ex)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile containing an unknown name"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(@"x"));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Execute()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that member could not be found"
                .f(() => ex.Message.Should().Contain("The name 'x' does not exist in the current context"));
        }

        [Scenario]
        public static void ExecutionFails(Baufile baufile, Exception ex, string message)
        {
            var scenario = MethodBase.GetCurrentMethod().GetFullName();

            "Given a baufile which throws an exception"
                .f(() => baufile = Baufile.Create(scenario).WriteLine(
                    @"throw new Exception(""" + (message = Guid.NewGuid().ToString()) + @""");"));

            "When I execute the baufile"
                .f(() => ex = Record.Exception(() => baufile.Execute()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I see details of the exception"
                .f(() => ex.Message.Should().Contain(message));
        }
    }
}
