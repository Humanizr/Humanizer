using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests
{
    public class FluentMethods
    {
        [Fact]
        public void Terabytes()
        {
            var size = (2.0).Terabytes();
            Assert.Equal(ByteSize.FromTeraBytes(2), size);
        }

        [Fact]
        public void HumanizesTB()
        {
            var humanized = (2.0).Terabytes().Humanize();
            Assert.Equal("2 TB", humanized);
        }

        [Fact]
        public void Gigabytes()
        {
            var size = (2.0).Gigabytes();
            Assert.Equal(ByteSize.FromGigaBytes(2), size);
        }

        [Fact]
        public void HumanizesGB()
        {
            var humanized = (2.0).Gigabytes().Humanize();
            Assert.Equal("2 GB", humanized);
        }

        [Fact]
        public void Kilobytes()
        {
            var size = (2.0).Kilobytes();
            Assert.Equal(ByteSize.FromKiloBytes(2), size);
        }

        [Fact]
        public void HumanizesKB()
        {
            var humanized = (2.0).Kilobytes().Humanize();
            Assert.Equal("2 KB", humanized);
        }

        [Fact]
        public void Bytes()
        {
            var size = (2.0).Bytes();
            Assert.Equal(ByteSize.FromBytes(2), size);
        }

        [Fact]
        public void HumanizesBytes()
        {
            var humanized = (2.0).Bytes().Humanize();
            Assert.Equal("2 B", humanized);
        }

        [Fact]
        public void Bits()
        {
            var size = ((long)2).Bits();
            Assert.Equal(ByteSize.FromBits(2), size);
        }

        [Fact]
        public void HumanizesBits()
        {
            var humanized = ((long)2).Bits().Humanize();
            Assert.Equal("2 b", humanized);
        }
        [Fact]
        public void HumanizesWithFormat()
        {
            // TODO
        }

        [Fact]
        public void Dehumanizes()
        {
            // TODO
        }
    }
}
