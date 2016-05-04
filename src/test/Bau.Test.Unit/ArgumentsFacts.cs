// <copyright file="ArgumentsFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Unit
{
    using BauCore;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

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

        [Theory]
        [InlineData("-A", null)]
        [InlineData("-T", "a")]
        [InlineData("-T", "all")]
        public static void CanParseTaskListTypeAll(string arg1, string arg2)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.All);
        }

        [Theory]
        [InlineData("-T", null)]
        [InlineData("-T", "d")]
        [InlineData("-T", "descriptive")]
        public static void CanParseTaskListTypeDefault(string arg1, string arg2)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.Descriptive);
        }

        [Theory]
        [InlineData("-P", null)]
        [InlineData("-T", "p")]
        [InlineData("-T", "prerequisites")]
        public static void CanParseTaskListTypePrerequisites(string arg1, string arg2)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.Prerequisites);
        }

        [Theory]
        [InlineData("-J", null)]
        [InlineData("-T", "j")]
        [InlineData("-T", "json")]
        public static void CanParseTaskListTypeJson(string arg1, string arg2)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.TaskListType.Should().Be(TaskListType.Json);
        }

        [Theory]
        [InlineData("-p", "FirstName=John")]
        public static void CanParseParameter(string arg1, string arg2)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.NamedParameters.Should().NotBeNull();            
        }

        [Theory]
        [InlineData("-p", "FirstName=John")]
        public static void CanParseSingleParameter(string arg1, string arg2)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.NamedParameters.Count.Should().Be(1);    
        }

        [Theory]
        [InlineData("-p", "FirstName=John", "-p", "LastName=Doe")]
        public static void CanParseTwoParameters(string arg1, string arg2, string arg3, string arg4)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2, arg3, arg4 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            arguments.NamedParameters.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("-p", "FirstName=John", "-p", "LastName=Doe")]
        public static void CanCreateParameterContextCount(string arg1, string arg2, string arg3, string arg4)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2, arg3, arg4 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            var parameterContext = new BauScriptParameterContext(arguments.NamedParameters);
            parameterContext.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("-p", "FirstName=John", "-p", "LastName=Doe")]
        public static void CanCreateParameterContext(string arg1, string arg2, string arg3, string arg4)
        {
            // arrange
            var rawArgs = new[] { arg1, arg2, arg3, arg4 };

            // act
            var arguments = Arguments.Parse(rawArgs);

            // assert
            var parameterContext = new BauScriptParameterContext(arguments.NamedParameters);
            var firstName = (string) parameterContext["FirstName"];
            firstName.Should().Be("John");
            var lastName = (string) parameterContext["LastName"];
            lastName.Should().Be("Doe");
        }
    }
}
