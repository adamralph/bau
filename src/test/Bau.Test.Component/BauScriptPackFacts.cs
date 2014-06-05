// <copyright file="BauScriptPackFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Component
{
    using BauCore;
    using FakeItEasy;
    using FluentAssertions;
    using ScriptCs.Contracts;
    using Xunit.Extensions;
    using LogLevel = BauCore.LogLevel;

    public static class BauScriptPackFacts
    {
        [Theory]
        [InlineData(null, null, LogLevel.Info)]
        [InlineData("-loglevel", null, LogLevel.Info)]

        [InlineData("-loglevel", "trace", LogLevel.Trace)]
        [InlineData("-LOGlevel", "trace", LogLevel.Trace)]
        [InlineData("-l", "trace", LogLevel.Trace)]
        [InlineData("-l", "t", LogLevel.Trace)]
        [InlineData("-l", "T", LogLevel.Trace)]
        [InlineData("-l", "trace", LogLevel.Trace)]
        [InlineData("-l", "TRace", LogLevel.Trace)]
        [InlineData("-trace", null, LogLevel.Trace)]
        [InlineData("-TRace", null, LogLevel.Trace)]
        [InlineData("-t", null, LogLevel.Trace)]
        [InlineData("-t", "debug", LogLevel.Trace)]

        [InlineData("-l", "d", LogLevel.Debug)]
        [InlineData("-l", "debug", LogLevel.Debug)]
        [InlineData("-debug", null, LogLevel.Debug)]
        [InlineData("-DEbug", null, LogLevel.Debug)]
        [InlineData("-d", null, LogLevel.Debug)]
        [InlineData("-d", "info", LogLevel.Debug)]

        [InlineData("-l", "i", LogLevel.Info)]
        [InlineData("-l", "info", LogLevel.Info)]

        [InlineData("-l", "w", LogLevel.Warn)]
        [InlineData("-l", "warn", LogLevel.Warn)]
        [InlineData("-l", "q", LogLevel.Warn)]
        [InlineData("-l", "quiet", LogLevel.Warn)]
        [InlineData("-q", null, LogLevel.Warn)]
        [InlineData("-q", "error", LogLevel.Warn)]

        [InlineData("-l", "e", LogLevel.Error)]
        [InlineData("-l", "error", LogLevel.Error)]
        [InlineData("-l", "qq", LogLevel.Error)]
        [InlineData("-qq", null, LogLevel.Error)]
        [InlineData("-qq", "fatal", LogLevel.Error)]

        [InlineData("-l", "f", LogLevel.Fatal)]
        [InlineData("-l", "fatal", LogLevel.Fatal)]

        [InlineData("-l", "a", LogLevel.All)]
        [InlineData("-l", "all", LogLevel.All)]

        [InlineData("-l", "o", LogLevel.Off)]
        [InlineData("-l", "off", LogLevel.Off)]
        [InlineData("-l", "n", LogLevel.Off)]
        [InlineData("-l", "none", LogLevel.Off)]
        [InlineData("-l", "s", LogLevel.Off)]
        [InlineData("-l", "silent", LogLevel.Off)]
        [InlineData("-s", null, LogLevel.Off)]
        [InlineData("-s", "all", LogLevel.Off)]
        public static void SetsLogLevel(string arg0, string arg1, LogLevel logLevel)
        {
            // arrange
            var session = A.Fake<IScriptPackSession>();
            A.CallTo(() => session.ScriptArgs).Returns(new[] { arg0, arg1 });
            var sut = new BauScriptPack();

            // act
            sut.Initialize(session);

            // assert
            sut.Context.LogLevel.Should().Be(logLevel);
        }
    }
}
