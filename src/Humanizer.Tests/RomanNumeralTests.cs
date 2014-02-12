using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class RomanNumeralTests
    {
        [Theory]
        [InlineData(1, "I")]
        [InlineData(12, "XII")]
        [InlineData(100, "C")]
        [InlineData(6, "VI")]
        public void CanRomanize(int input, string expected)
        {
            Assert.Equal(input.ToRoman(), expected);
        }

        [Theory]
        [InlineData("D", 500)]
        [InlineData("XX", 20)]
        [InlineData("III", 3)]
        [InlineData("iv", 4)]
        [InlineData("XC", 90)]
        public void CanUnromanize(string input, int expected)
        {
            Assert.Equal(input.FromRoman(), expected);
        }
    }
}
