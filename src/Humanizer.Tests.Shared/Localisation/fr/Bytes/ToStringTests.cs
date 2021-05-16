using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Localisation.fr.Bytes
{
    [UseCulture("fr-FR")]
    public class ToStringTests
    {
        [Fact]
        public void ReturnsLargestMetricSuffix()
        {
            Assert.Equal("10,5 Ko", ByteSize.FromKilobytes(10.5).ToString());
        }

        [Fact]
        public void ReturnsDefaultNumberFormat()
        {
            Assert.Equal("10,5 Ko", ByteSize.FromKilobytes(10.5).ToString("KB"));
        }

        [Fact]
        public void ReturnsProvidedNumberFormat()
        {
            Assert.Equal("10,1234 Ko", ByteSize.FromKilobytes(10.1234).ToString("#.#### KB"));
        }

        [Fact]
        public void ReturnsBits()
        {
            Assert.Equal("10 b", ByteSize.FromBits(10).ToString("##.#### b"));
        }

        [Fact]
        public void ReturnsBytes()
        {
            Assert.Equal("10 o", ByteSize.FromBytes(10).ToString("##.#### B"));
        }

        [Fact]
        public void ReturnsKilobytes()
        {
            Assert.Equal("10 Ko", ByteSize.FromKilobytes(10).ToString("##.#### KB"));
        }

        [Fact]
        public void ReturnsMegabytes()
        {
            Assert.Equal("10 Mo", ByteSize.FromMegabytes(10).ToString("##.#### MB"));
        }

        [Fact]
        public void ReturnsGigabytes()
        {
            Assert.Equal("10 Go", ByteSize.FromGigabytes(10).ToString("##.#### GB"));
        }

        [Fact]
        public void ReturnsTerabytes()
        {
            Assert.Equal("10 To", ByteSize.FromTerabytes(10).ToString("##.#### TB"));
        }

        [Fact]
        public void ReturnsSelectedFormat()
        {
            Assert.Equal("10,0 To", ByteSize.FromTerabytes(10).ToString("0.0 TB"));
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZero()
        {
            Assert.Equal("512 Ko", ByteSize.FromMegabytes(.5).ToString("#.#"));
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues()
        {
            Assert.Equal("-512 Ko", ByteSize.FromMegabytes(-.5).ToString("#.#"));
        }

        [Fact]
        public void ReturnsBytesViaGeneralFormat()
        {
            Assert.Equal("10 o", $"{ByteSize.FromBytes(10)}");
        }
    }
}
