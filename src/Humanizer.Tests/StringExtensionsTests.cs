using System;
using Xunit;

namespace Humanizer.Tests
{
    public class StringExtensionsTests
    {
        private const string format = "This is a format with three numbers: {0}-{1}-{2}.";
        private const string expected = "This is a format with three numbers: 1-2-3.";

        [Fact]
        public void CanFormatStringWithExactNumberOfArguments()
        {
            string actual = format.FormatWith(1, 2, 3);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFormatStringWithMoreArguments()
        {
            string actual = format.FormatWith(1, 2, 3, 4, 5);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotFormatStringWithLessArguments()
        {
            Assert.Throws<FormatException>(() => format.FormatWith(1, 2));
        }

        [Fact]
        public void FormatCannotBeNull()
        {
            string format = null;
            Assert.Throws<ArgumentNullException>(() => format.FormatWith(1, 2));
        }
    }
}
