// <copyright file="ArgumentsFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Unit
{
    using BauCore;
    using FluentAssertions;
    using Xunit;

    public static class ArgumentsFacts
    {
        [Fact]
        public static void TaskWriterIsNullWhenNoArguments()
        {
            // arrange
            var rawArgs = new string[0];

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListingKind.Should().Be(TaskListingKind.None);
        }

        [Fact]
        public static void CanParseTaskWriterForAll()
        {
            // arrange
            var rawArgs = new[] { "-A" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListingKind.Should().Be(TaskListingKind.TextAll);
        }

        [Fact]
        public static void CanParseTaskWriterForDescribedTasks()
        {
            // arrange
            var rawArgs = new[] { "-T" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListingKind.Should().Be(TaskListingKind.TextDescribed);
        }

        [Fact]
        public static void CanParseTaskWriterForPrerequisites()
        {
            // arrange
            var rawArgs = new[] { "-P" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListingKind.Should().Be(TaskListingKind.TextPrereq);
        }

        [Fact]
        public static void CanParseTaskWriterForJson()
        {
            // arrange
            var rawArgs = new[] { "-J" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListingKind.Should().Be(TaskListingKind.Json);
        }
    }
}
