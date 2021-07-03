using Humanizer.Bytes;

using Xunit;

namespace Humanizer.Tests.Localisation.de.Bytes
{
    [UseCulture("de-DE")]
    public class ToFullWordsTests
    {
        [Fact]
        public void ReturnsSingularBit()
        {
            Assert.Equal("1 Bit", ByteSize.FromBits(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBits()
        {
            Assert.Equal("2 Bits", ByteSize.FromBits(2).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularByte()
        {
            Assert.Equal("1 Byte", ByteSize.FromBytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBytes()
        {
            Assert.Equal("10 Bytes", ByteSize.FromBytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularKiloByte()
        {
            Assert.Equal("1 Kilobyte", ByteSize.FromKilobytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralKilobytes()
        {
            Assert.Equal("10 Kilobytes", ByteSize.FromKilobytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularMegabyte()
        {
            Assert.Equal("1 Megabyte", ByteSize.FromMegabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralMegabytes()
        {
            Assert.Equal("10 Megabytes", ByteSize.FromMegabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularGigabyte()
        {
            Assert.Equal("1 Gigabyte", ByteSize.FromGigabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralGigabytes()
        {
            Assert.Equal("10 Gigabytes", ByteSize.FromGigabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularTerabyte()
        {
            Assert.Equal("1 Terabyte", ByteSize.FromTerabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralTerabytes()
        {
            Assert.Equal("10 Terabytes", ByteSize.FromTerabytes(10).ToFullWords());
        }

        [Theory]
        [InlineData(229376, "B", "229376 Bytes")]
        [InlineData(229376, "# KB", "224 Kilobytes")]
        public void ToFullWordsFormatted(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
        }
    }
}
