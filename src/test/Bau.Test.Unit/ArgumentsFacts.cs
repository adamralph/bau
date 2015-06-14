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
        public static void TaskListTypeIsNullWhenNoArguments()
        {
            // arrange
            var rawArgs = new string[0];

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().BeNull();
        }

        [Fact]
        public static void CanParseTaskListTypeAll()
        {
            // arrange
            var rawArgs = new[] { "-A" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.All);
        }

        [Fact]
        public static void CanParseTaskListTypeDefault()
        {
            // arrange
            var rawArgs = new[] { "-T" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.Descriptive);
        }

        [Fact]
        public static void CanParseTaskListTypePrerequisites()
        {
            // arrange
            var rawArgs = new[] { "-P" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.Prerequisites);
        }

        [Fact]
        public static void CanParseTaskListTypeJson()
        {
            // arrange
            var rawArgs = new[] { "-J" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.Json);
        }
    }
}
