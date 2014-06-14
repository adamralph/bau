// <copyright file="TaskDependencies.cs" company="Bau contributors">
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

    public static class TaskDependencies
    {
        // happy path
        [Scenario]
        public static void SingleDependency(ITaskBuilder builder, IList<string> executedTasks)
        {
            "Given a non-default task"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default")
                        .Do(() => (executedTasks = new List<string>()).Add("non-default")));

            "And a default task which depends on the non-default task"
                .f(() => builder
                    .Task("default").DependsOn("non-default")
                        .Do(() => executedTasks.Add("default")));

            "When I run the builder"
                .f(() => builder.Run());

            "Then both tasks are executed"
                .f(() => executedTasks.Count.Should().Be(2));

            "And the non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("non-default"));

            "And the default task is executed second"
                .f(() => executedTasks[1].Should().Be("default"));
        }

        [Scenario]
        public static void MultipleDependencies(ITaskBuilder builder, IList<string> executedTasks)
        {
            "Given a non-default task"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default1")
                        .Do(() => (executedTasks = new List<string>()).Add("non-default1")));

            "And a second non-default task"
                .f(() => builder
                    .Task("non-default2")
                        .Do(() => executedTasks.Add("non-default2")));

            "And a default task which depends on both non-default tasks"
                .f(() => builder
                    .Task("default").DependsOn("non-default1", "non-default2")
                        .Do(() => executedTasks.Add("default")));

            "When I run the builder"
                .f(() => builder.Run());

            "Then all three tasks are executed"
                .f(() => executedTasks.Count.Should().Be(3));

            "And the first non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("non-default1"));

            "And the second non-default task is executed second"
                .f(() => executedTasks[1].Should().Be("non-default2"));

            "And the default task is executed third"
                .f(() => executedTasks[2].Should().Be("default"));
        }

        [Scenario]
        public static void NestedDependencies(ITaskBuilder builder, IList<string> executedTasks)
        {
            "Given a non-default task"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default1")
                        .Do(() => (executedTasks = new List<string>()).Add("non-default1")));

            "And a second non-default task which depends on the first non-default task"
                .f(c => builder
                    .Task("non-default2").DependsOn("non-default1")
                        .Do(() => executedTasks.Add("non-default2")));

            "And a default task which depends on the second non-default task"
                .f(() => builder
                    .Task("default").DependsOn("non-default2")
                        .Do(() => executedTasks.Add("default")));

            "When I run the builder"
                .f(() => builder.Run());

            "Then all three tasks are executed"
                .f(() => executedTasks.Count.Should().Be(3));

            "And the first non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("non-default1"));

            "And the second non-default task is executed second"
                .f(() => executedTasks[1].Should().Be("non-default2"));

            "And the default task is executed third"
                .f(() => executedTasks[2].Should().Be("default"));
        }

        [Scenario]
        public static void RepeatedDependency(ITaskBuilder builder, IList<string> executedTasks)
        {
            "Given a non-default task"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default1")
                        .Do(() => (executedTasks = new List<string>()).Add("non-default1")));

            "And a second non-default task which depends on the first non-default task"
                .f(c => builder
                    .Task("non-default2").DependsOn("non-default1")
                        .Do(() => executedTasks.Add("non-default2")));

            "And a default task which depends on both non-default tasks"
                .f(() => builder
                    .Task("default").DependsOn("non-default1", "non-default2")
                        .Do(() => executedTasks.Add("default")));

            "When I run the builder"
                .f(() => builder.Run());

            "Then all three tasks are executed"
                .f(() => executedTasks.Count.Should().Be(3));

            "And the first non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("non-default1"));

            "And the second non-default task is executed second"
                .f(() => executedTasks[1].Should().Be("non-default2"));

            "And the default task is executed third"
                .f(() => executedTasks[2].Should().Be("default"));
        }

        [Scenario]
        public static void CircularDependency(ITaskBuilder builder, IList<string> executedTasks)
        {
            "Given a non-default task which depends on the default task"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default").DependsOn("default")
                        .Do(() => (executedTasks = new List<string>()).Add("non-default")));

            "And a default task which depends on the non-default task"
                .f(c => builder
                    .Task("default").DependsOn("non-default")
                        .Do(() => executedTasks.Add("default")));

            "When I run the builder"
                .f(() => builder.Run());

            "Then both tasks are executed"
                .f(() => executedTasks.Count.Should().Be(2));

            "And the non-default task is executed first"
                .f(() => executedTasks[0].Should().Be("non-default"));

            "And the default task is executed second"
                .f(() => executedTasks[1].Should().Be("default"));
        }

        // sad path
        [Scenario]
        public static void NonexistentDependency(ITaskBuilder builder, Exception ex)
        {
            "Given a default task with a non-existent dependency"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("default").DependsOn("non-existent")
                        .Do(() =>
                        {
                        }));

            "When I run the builder"
                .f(() => ex = Record.Exception(() => builder.Run()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And I am informed that the non-existent task was not found"
                .f(() => ex.Message.Should().Contain("'non-existent' task not found"));
        }

        [Scenario]
        public static void DependencyFails(ITaskBuilder builder, bool defaultExecuted, Exception ex)
        {
            "And a non-default task which fails"
                .f(c => builder = ScriptHost.Require<Bau>()
                    .Task("non-default").DependsOn("default")
                        .Do(() =>
                        {
                            throw new InvalidOperationException();
                        }));

            "And a default task which depends on the non-default task"
                .f(c => builder
                    .Task("default").DependsOn("non-default")
                        .Do(() => defaultExecuted = true));

            "When I run the builder"
                .f(() => ex = Record.Exception(() => builder.Run()));

            "Then execution should fail"
                .f(() => ex.Should().NotBeNull());

            "And the default task is not executed"
                .f(() => defaultExecuted.Should().BeFalse());

            "And I am informed that the non-default task was executed"
                .f(() => ex.Message.Should().Contain("'non-default' task failed."));
        }
    }
}
