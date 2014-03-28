using Humanizer.Bytes;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Bytes
{
    public class ByteSizeExtensionsTests : AmbientCulture
    {
        public ByteSizeExtensionsTests() : base("en") { }

        [Fact]
        public void Terabytes()
        {
            Assert.Equal(ByteSize.FromTerabytes(2), (2.0).Terabytes());
        }

        [Theory]
        [InlineData(2, null, "2 TB")]
        [InlineData(2, "GB", "2048 GB")]
        [InlineData(2.123, "#.#", "2.1 TB")]
        public void HumanizesTerabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Terabytes().Humanize(format));
        }

        [Fact]
        public void Gigabytes()
        {
            Assert.Equal(ByteSize.FromGigabytes(2), (2.0).Gigabytes());
        }

        [Theory]
        [InlineData(2, null, "2 GB")]
        [InlineData(2, "MB", "2048 MB")]
        [InlineData(2.123, "#.##", "2.12 GB")]
        public void HumanizesGigabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Gigabytes().Humanize(format));
        }

        [Fact]
        public void Megabytes()
        {
            Assert.Equal(ByteSize.FromMegabytes(2), (2.0).Megabytes());
        }

        [Theory]
        [InlineData(2, null, "2 MB")]
        [InlineData(2, "KB", "2048 KB")]
        [InlineData(2.123, "#", "2 MB")]
        public void HumanizesMegabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Megabytes().Humanize(format));
        }

        [Fact]
        public void Kilobytes()
        {
            Assert.Equal(ByteSize.FromKilobytes(2), (2.0).Kilobytes());
        }

        [Theory]
        [InlineData(2, null, "2 KB")]
        [InlineData(2, "B", "2048 B")]
        [InlineData(2.123, "#.####", "2.123 KB")]
        public void HumanizesKilobytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Kilobytes().Humanize(format));
        }

        [Fact]
        public void Bytes()
        {
            Assert.Equal(ByteSize.FromBytes(2), (2.0).Bytes());
        }

        [Theory]
        [InlineData(2, null, "2 B")]
        [InlineData(2000, "KB", "1.95 KB")]
        [InlineData(2123, "#.##", "2.07 KB")]
        public void HumanizesBytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Bytes().Humanize(format));
        }

        [Fact]
        public void Bits()
        {
            Assert.Equal(ByteSize.FromBits(2), (2).Bits());
        }

        [Theory]
        [InlineData(2, null, "2 b")]
        [InlineData(12, "B", "1.5 B")]
        [InlineData(10000, "#.# KB", "1.2 KB")]
        public void HumanizesBits(long input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Bits().Humanize(format));
        }
    }
}
