// <copyright file="DoubleExtensionsTheories.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauCore.Test.Unit
{
    using BauCore;
    using FakeItEasy;
    using FluentAssertions;
    using ScriptCs.Contracts;
    using Xunit;
    using Xunit.Extensions;

    public static class DoubleExtensionsTheories
    {
        // arrange
        [Theory]
        [InlineData(000000000.000500d, "500 ns")]
        [InlineData(000000000.005000d, "5 µs")]
        [InlineData(000000000.050000d, "50 µs")]
        [InlineData(000000000.500000d, "500 µs")]
        [InlineData(000000005.000000d, "5 ms")]
        [InlineData(000000050.000000d, "50 ms")]
        [InlineData(000000500.000000d, "500 ms")]
        [InlineData(000005000.000000d, "5 s")]
        [InlineData(000050000.000000d, "50 s")]
        [InlineData(000500000.000000d, "8 min 20 s")]
        [InlineData(005000000.000000d, "83 min")]
        [InlineData(050000000.000000d, "833 min")]
        [InlineData(500000000.000000d, "8,333 min")]

        [InlineData(000000000.000567d, "567 ns")]
        [InlineData(000000000.005670d, "5.67 µs")]
        [InlineData(000000000.056700d, "56.7 µs")]
        [InlineData(000000000.567000d, "567 µs")]
        [InlineData(000000005.670000d, "5.67 ms")]
        [InlineData(000000056.700000d, "56.7 ms")]
        [InlineData(000000567.000000d, "567 ms")]
        [InlineData(000005670.000000d, "5.67 s")]
        [InlineData(000056700.000000d, "56.7 s")]
        [InlineData(000567000.000000d, "9 min 27 s")]
        [InlineData(005670000.000000d, "95 min")]
        [InlineData(056700000.000000d, "945 min")]
        [InlineData(567000000.000000d, "9,450 min")]

        [InlineData(000480000.000000d, "8 min")]
        public static void FractionalMillisecondsCanBeHumanized(double milliseconds, string expected)
        {
            // act
            var actual = milliseconds.ToStringFromMilliseconds();

            // assert
            actual.Should().Be(expected);
        }
    }
}
