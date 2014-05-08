// <copyright file="BauScriptPackFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace Bau.Test.Component
{
    using BauCore;
    using FakeItEasy;
    using FluentAssertions;
    using ScriptCs.Contracts;
    using Xunit;

    public static class BauScriptPackFacts
    {
        [Fact]
        public static void CanInitializeWhenScriptArgsAreNull()
        {
            // arrange
            var session = A.Fake<IScriptPackSession>();
            A.CallTo(() => session.ScriptArgs).Returns(null);
            var sut = new BauScriptPack();

            // act
            var ex = Record.Exception(() => sut.Initialize(session));

            // assert
            ex.Should().BeNull();
        }
    }
}
