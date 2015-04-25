// <copyright file="ArgumentsFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Unit
{
    using BauCore;
    using FakeItEasy;
    using FluentAssertions;
    using ScriptCs.Contracts;
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
            arguments.TaskListWriter.Should().BeNull();
        }

        [Fact]
        public static void CanParseTaskWriterForAll()
        {
            // arrange
            var rawArgs = new[] { "-A" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListWriter.Should().NotBeNull();
            arguments.TaskListWriter.FormatAsJson.Should().BeFalse();
            arguments.TaskListWriter.RequireDescription.Should().BeFalse();
            arguments.TaskListWriter.ShowDescription.Should().BeFalse();
            arguments.TaskListWriter.ShowPrerequisites.Should().BeFalse();
        }

        [Fact]
        public static void CanParseTaskWriterForDescribedTasks()
        {
            // arrange
            var rawArgs = new[] { "-T" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListWriter.Should().NotBeNull();
            arguments.TaskListWriter.FormatAsJson.Should().BeFalse();
            arguments.TaskListWriter.RequireDescription.Should().BeTrue();
            arguments.TaskListWriter.ShowDescription.Should().BeTrue();
            arguments.TaskListWriter.ShowPrerequisites.Should().BeFalse();
        }

        [Fact]
        public static void CanParseTaskWriterForPrerequisites()
        {
            // arrange
            var rawArgs = new[] { "-P" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListWriter.Should().NotBeNull();
            arguments.TaskListWriter.FormatAsJson.Should().BeFalse();
            arguments.TaskListWriter.RequireDescription.Should().BeFalse();
            arguments.TaskListWriter.ShowDescription.Should().BeFalse();
            arguments.TaskListWriter.ShowPrerequisites.Should().BeTrue();
        }

        [Fact]
        public static void CanParseTaskWriterForJson()
        {
            // arrange
            var rawArgs = new[] { "-J" };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListWriter.Should().NotBeNull();
            arguments.TaskListWriter.FormatAsJson.Should().BeTrue();
            arguments.TaskListWriter.RequireDescription.Should().BeFalse();
            arguments.TaskListWriter.ShowDescription.Should().BeTrue();
            arguments.TaskListWriter.ShowPrerequisites.Should().BeTrue();
        }
    }
}
