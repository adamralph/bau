// <copyright file="DefaultTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component
{
    using System;
    using BauCore.Test.Component.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class DefaultTask
    {
        [Scenario]
        public static void DefaultTaskExists(ITaskBuilder builder, bool executed)
        {
            "Given a default task"
                .f(() => builder = ScriptCs.Require<Bau>().Do(() => executed = true));

            "When I execute"
                .f(() => builder.Execute());

            "Then the task is executed"
                .f(() => executed.Should().BeTrue());
        }

        [Scenario]
        public static void NoTasksExist(ITaskBuilder builder, Exception ex)
        {
            "Given no tasks"
                .f(() => builder = ScriptCs.Require<Bau>());

            "When I execute"
                .f(() => ex = Record.Exception(() => builder.Execute()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the default task was not found"
                .f(() => ex.Message.Should().Contain("'default' task not found"));
        }

        [Scenario]
        public static void ANonDefaultTaskExists(ITaskBuilder builder, Exception ex)
        {
            "Given no tasks"
                .f(() => builder = ScriptCs.Require<Bau>().Task("foo").Do(() => { }));

            "When I execute"
                .f(() => ex = Record.Exception(() => builder.Execute()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the default task was not found"
                .f(() => ex.Message.Should().Contain("'default' task not found"));
        }
    }
}
