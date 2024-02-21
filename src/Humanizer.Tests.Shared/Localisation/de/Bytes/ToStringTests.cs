namespace Humanizer.Tests.Localisation.de.Bytes
{
    [UseCulture("de-DE")]
    public class ToStringTests
    {
        [Fact]
        public void ReturnsLargestMetricSuffix() =>
            Assert.Equal("10,5 kB", ByteSize.FromKilobytes(10.5).ToString());

        [Fact]
        public void ReturnsDefaultNumberFormat() =>
            Assert.Equal("10,5 kB", ByteSize.FromKilobytes(10.5).ToString("KB"));

        [Fact]
        public void ReturnsProvidedNumberFormat() =>
            Assert.Equal("10,1234 kB", ByteSize.FromKilobytes(10.1234).ToString("#.#### KB"));

        [Fact]
        public void ReturnsBits() =>
            Assert.Equal("10 bit", ByteSize.FromBits(10).ToString("##.#### b"));

        [Fact]
        public void ReturnsBytes() =>
            Assert.Equal("10 B", ByteSize.FromBytes(10).ToString("##.#### B"));

        [Fact]
        public void ReturnsKilobytes() =>
            Assert.Equal("10 kB", ByteSize.FromKilobytes(10).ToString("##.#### KB"));

        [Fact]
        public void ReturnsKibibytes() =>
            Assert.Equal("10 KiB", ByteSize.FromKibibytes(10).ToString("##.#### KiB"));

        [Fact]
        public void ReturnsMegabytes() =>
            Assert.Equal("10 MB", ByteSize.FromMegabytes(10).ToString("##.#### MB"));

        [Fact]
        public void ReturnsMebibytes() =>
            Assert.Equal("10 MiB", ByteSize.FromMebibytes(10).ToString("##.#### MiB"));

        [Fact]
        public void ReturnsGigabytes() =>
            Assert.Equal("10 GB", ByteSize.FromGigabytes(10).ToString("##.#### GB"));

        [Fact]
        public void ReturnsGibibytes() =>
            Assert.Equal("10 GiB", ByteSize.FromGibibytes(10).ToString("##.#### GiB"));

        [Fact]
        public void ReturnsTerabytes() =>
            Assert.Equal("10 TB", ByteSize.FromTerabytes(10).ToString("##.#### TB"));

        [Fact]
        public void ReturnsTebibytes() =>
            Assert.Equal("10 TiB", ByteSize.FromTebibytes(10).ToString("##.#### TiB"));

        [Fact]
        public void ReturnsSelectedFormat() =>
            Assert.Equal("10,0 TB", ByteSize.FromTerabytes(10).ToString("0.0 TB"));

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZero() =>
            Assert.Equal("500 kB", ByteSize.FromMegabytes(.5).ToString("#.#"));

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues() =>
            Assert.Equal("-500 kB", ByteSize.FromMegabytes(-.5).ToString("#.#"));

        [Fact]
        public void ReturnsBytesViaGeneralFormat() =>
            Assert.Equal("10 B", $"{ByteSize.FromBytes(10)}");
    }
}
