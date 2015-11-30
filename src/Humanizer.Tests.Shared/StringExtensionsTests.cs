using System;
using System.Globalization;
using Xunit;

namespace Humanizer.Tests
{
    public class StringExtensionsTests
    {
        private const string Format = "This is a format with three numbers: {0}-{1}-{2}.";
        private const string Expected = "This is a format with three numbers: 1-2-3.";

        [Fact]
        public void CanFormatStringWithExactNumberOfArguments()
        {
            Assert.Equal(Expected, Format.FormatWith(1, 2, 3));
        }

        [Fact]
        public void CanFormatStringWithMoreArguments()
        {
            Assert.Equal(Expected, Format.FormatWith(1, 2, 3, 4, 5));
        }

        [Fact]
        public void CannotFormatStringWithLessArguments()
        {
            Assert.Throws<FormatException>(() => Format.FormatWith(1, 2));
        }

        [Fact]
        public void FormatCannotBeNull()
        {
            string format = null;
            Assert.Throws<ArgumentNullException>(() => format.FormatWith(1, 2));
        }

        [Theory]
        [InlineData("en-US", "6,666.66")]
        [InlineData("ru-RU", "6 666,66")]
        public void CanSpecifyCultureExplicitly(string culture, string expected)
        {
            Assert.Equal(expected, "{0:N2}".FormatWith(new CultureInfo(culture), 6666.66));
        }
    }
}