using Xunit;
using Xunit.Extensions;
using Humanizer;

namespace Humanizer.Tests
{
    public class RomanNumeralTests
    {
        [Theory]
        [InlineData(1, "I")]
        [InlineData(2, "II")]
        [InlineData(3, "III")]
        [InlineData(4, "IV")]
        [InlineData(5, "V")]
        [InlineData(6, "VI")]
        [InlineData(7, "VII")]
        [InlineData(8, "VIII")]
        [InlineData(9, "IX")]
        [InlineData(10, "X")]
        [InlineData(11, "XI")]
        [InlineData(12, "XII")]
        [InlineData(100, "C")]
        [InlineData(3999, "MMMCMXCIX")]
        public void CanRomanize(int input, string expected)
        {
            Assert.Equal(expected, input.ToRoman());
        }

        [Theory]
        [InlineData(1, "I")]
        [InlineData(2, "II")]
        [InlineData(3, "III")]
        [InlineData(4, "IV")]
        [InlineData(5, "V")]
        [InlineData(6, "VI")]
        [InlineData(7, "VII")]
        [InlineData(8, "VIII")]
        [InlineData(9, "IX")]
        [InlineData(10, "X")]
        [InlineData(11, "XI")]
        [InlineData(12, "XII")]
        [InlineData(100, "C")]
        [InlineData(3999, "MMMCMXCIX")]
        public void CanUnromanize(int expected, string input)
        {
            Assert.Equal(expected, input.FromRoman());
        }
    }
}
