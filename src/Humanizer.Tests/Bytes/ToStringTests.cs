using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    public class ToStringTests
    {
        [Fact]
        public void ReturnsLargestMetricSuffix()
        {
            Assert.Equal("10.5 KB", ByteSize.FromKiloBytes(10.5).ToString());
        }

        [Fact]
        public void ReturnsDefaultNumberFormat()
        {
            Assert.Equal("10.5 KB", ByteSize.FromKiloBytes(10.5).ToString("KB"));
        }

        [Fact]
        public void ReturnsProvidedNumberFormat()
        {
            Assert.Equal("10.1234 KB", ByteSize.FromKiloBytes(10.1234).ToString("#.#### KB"));
        }

        [Fact]
        public void ReturnsBits()
        {
            Assert.Equal("10 b", ByteSize.FromBits(10).ToString("##.#### b"));
        }

        [Fact]
        public void ReturnsBytes()
        {
            Assert.Equal("10 B", ByteSize.FromBytes(10).ToString("##.#### B"));
        }

        [Fact]
        public void ReturnsKiloBytes()
        {
            Assert.Equal("10 KB", ByteSize.FromKiloBytes(10).ToString("##.#### KB"));
        }

        [Fact]
        public void ReturnsMegaBytes()
        {
            Assert.Equal("10 MB", ByteSize.FromMegaBytes(10).ToString("##.#### MB"));
        }

        [Fact]
        public void ReturnsGigaBytes()
        {
            Assert.Equal("10 GB", ByteSize.FromGigaBytes(10).ToString("##.#### GB"));
        }

        [Fact]
        public void ReturnsTeraBytes()
        {
            Assert.Equal("10 TB", ByteSize.FromTeraBytes(10).ToString("##.#### TB"));
        }

        [Fact]
        public void ReturnsSelectedFormat()
        {
            Assert.Equal("10.0 TB", ByteSize.FromTeraBytes(10).ToString("0.0 TB"));
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZero()
        {
            Assert.Equal("512 KB", ByteSize.FromMegaBytes(.5).ToString("#.#"));
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues()
        {
            Assert.Equal("-512 KB", ByteSize.FromMegaBytes(-.5).ToString("#.#"));
        }
    }
}
