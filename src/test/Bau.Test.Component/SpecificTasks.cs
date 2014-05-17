// <copyright file="SpecificTasks.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component
{
    using System;
    using BauCore.Test.Component.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class SpecificTasks
    {
        [Scenario]
        public static void SingleTask(ITaskBuilder builder, bool executed)
        {
            "Given a non-default task is specified"
                .f(() => builder = ScriptCs.Require<Bau>(new[] { "non-default" }));

            "And a non-default task"
                .f(() => builder.Task("non-default").Do(() => executed = true));

            "When I run the builder"
                .f(() => builder.Run());

            "Then the task is executed"
                .f(() => executed.Should().BeTrue());
        }

        [Scenario]
        public static void MultipleTasks(string[] args, ITaskBuilder builder, bool executed1, bool executed2)
        {
            "Given arguments containing 2 non-default tasks"
                .f(() => builder = ScriptCs.Require<Bau>(new[] { "non-default1", "non-default2" }));

            "And a non-default task"
                .f(() => builder.Task("non-default1").Do(() => executed1 = true));

            "And another non-default task"
                .f(() => builder.Task("non-default2").Do(() => executed2 = true));

            "When I run the builder"
                .f(() => builder.Run());

            "Then the first task is executed"
                .f(() => executed1.Should().BeTrue());

            "And the second task is executed"
                .f(() => executed2.Should().BeTrue());
        }

        [Scenario]
        public static void NoTasksExist(ITaskBuilder builder, Exception ex)
        {
            "Given a non-existent task is specified"
                .f(() => builder = ScriptCs.Require<Bau>(new[] { "non-existent" }));

            "And no tasks"
                .f(() => { });

            "When I run the builder"
                .f(() => ex = Record.Exception(() => builder.Run()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the non-existent task was not found"
                .f(() => ex.Message.Should().Contain("'non-existent' task not found"));
        }

        [Scenario]
        public static void AnotherTaskExists(ITaskBuilder builder, Exception ex)
        {
            "Given a non-existent task is specified"
                .f(() => builder = ScriptCs.Require<Bau>(new[] { "non-existent" }));

            "And another task exists"
                .f(() => builder.Task("foo").Do(() => { }));

            "When I run the builder"
                .f(() => ex = Record.Exception(() => builder.Run()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the non-existent task was not found"
                .f(() => ex.Message.Should().Contain("'non-existent' task not found"));
        }
    }
}
