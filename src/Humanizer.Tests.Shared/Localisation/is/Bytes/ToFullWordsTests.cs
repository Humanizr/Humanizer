using Humanizer.Bytes;

using Xunit;

namespace Humanizer.Tests.Localisation.@is.Bytes
{
    [UseCulture("is")]
    public class ToFullWordsTests
    {
        [Fact]
        public void ReturnsSingularBit()
        {
            Assert.Equal("1 biti", ByteSize.FromBits(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBits()
        {
            Assert.Equal("2 biti", ByteSize.FromBits(2).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularByte()
        {
            Assert.Equal("1 bæti", ByteSize.FromBytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBytes()
        {
            Assert.Equal("10 bæti", ByteSize.FromBytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularKiloByte()
        {
            Assert.Equal("1 kílóbæti", ByteSize.FromKilobytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralKilobytes()
        {
            Assert.Equal("10 kílóbæti", ByteSize.FromKilobytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularMegabyte()
        {
            Assert.Equal("1 megabæti", ByteSize.FromMegabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralMegabytes()
        {
            Assert.Equal("10 megabæti", ByteSize.FromMegabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularGigabyte()
        {
            Assert.Equal("1 gígabæti", ByteSize.FromGigabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralGigabytes()
        {
            Assert.Equal("10 gígabæti", ByteSize.FromGigabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularTerabyte()
        {
            Assert.Equal("1 terabæti", ByteSize.FromTerabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralTerabytes()
        {
            Assert.Equal("10 terabæti", ByteSize.FromTerabytes(10).ToFullWords());
        }

        [Theory]
        [InlineData(229376, "B", "229376 bæti")]
        [InlineData(229376, "# KB", "224 kílóbæti")]
        public void ToFullWordsFormatted(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
        }
    }
}
