using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Localisation.fr.Bytes
{
    [UseCulture("fr-FR")]
    public class ToFullWordsTests
    {
        [Fact]
        public void ReturnsSingularBit()
        {
            Assert.Equal("1 bit", ByteSize.FromBits(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBits()
        {
            Assert.Equal("2 bits", ByteSize.FromBits(2).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularByte()
        {
            Assert.Equal("1 octet", ByteSize.FromBytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBytes()
        {
            Assert.Equal("10 octets", ByteSize.FromBytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularKiloByte()
        {
            Assert.Equal("1 kilooctet", ByteSize.FromKilobytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralKilobytes()
        {
            Assert.Equal("10 kilooctets", ByteSize.FromKilobytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularMegabyte()
        {
            Assert.Equal("1 mégaoctet", ByteSize.FromMegabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralMegabytes()
        {
            Assert.Equal("10 mégaoctets", ByteSize.FromMegabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularGigabyte()
        {
            Assert.Equal("1 gigaoctet", ByteSize.FromGigabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralGigabytes()
        {
            Assert.Equal("10 gigaoctets", ByteSize.FromGigabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularTerabyte()
        {
            Assert.Equal("1 téraoctet", ByteSize.FromTerabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralTerabytes()
        {
            Assert.Equal("10 téraoctets", ByteSize.FromTerabytes(10).ToFullWords());
        }

        [Theory]
        [InlineData(229376, "B", "229376 octets")]
        [InlineData(229376, "# KB", "224 kilooctets")]
        public void ToFullWordsFormatted(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
        }
    }
}
