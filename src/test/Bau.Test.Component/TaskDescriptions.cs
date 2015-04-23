// <copyright file="TaskDescriptions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component
{
    using System;
    using System.Collections.Generic;
    using BauCore.Test.Component.Support;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class TaskDescriptions
    {
        [Scenario]
        public static void NoDescriptionSet(ITaskBuilder builder, IBauTask task)
        {
            "Given one task with no description"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default")
                        .Do(() =>
                        {
                        }));

            "When I get the current task"
                .f(c => task = builder.CurrentTask);

            "Then the task description should be null"
                .f(c => task.Description.Should().BeNull());
        }

        [Scenario]
        public static void ExplicitNullDescriptionSet(ITaskBuilder builder, IBauTask task)
        {
            "Given one task with a null description"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default")
                        .Desc(null)
                        .Do(() =>
                        {
                        }));

            "When I get the current task"
                .f(c => task = builder.CurrentTask);

            "Then the task description should be null"
                .f(c => task.Description.Should().BeNull());
        }

        [Scenario]
        public static void EmptyDescriptionSet(ITaskBuilder builder, IBauTask task)
        {
            "Given one task with an explicitly set empty description"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default")
                        .Desc(string.Empty)
                        .Do(() =>
                        {
                        }));

            "When I get the current task"
                .f(c => task = builder.CurrentTask);

            "Then the task description should be empty string"
                .f(c => task.Description.Should().Be(string.Empty));
        }

        [Scenario]
        public static void SimpleDescriptionSet(ITaskBuilder builder, IBauTask task)
        {
            "Given one task with a simple description"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default")
                        .Desc("something descriptive")
                        .Do(() =>
                        {
                        }));

            "When I get the current task"
                .f(c => task = builder.CurrentTask);

            "Then the task description should be 'something descriptive'"
                .f(c => task.Description.Should().Be("something descriptive"));
        }
    }
}
