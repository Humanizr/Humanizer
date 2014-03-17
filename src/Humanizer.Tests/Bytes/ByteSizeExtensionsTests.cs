using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
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
        public void HumanizesTerabytes()
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
        public void HumanizesGigabytes()
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
        public void HumanizesKilobytes()
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
            var size = (2).Bits();
            Assert.Equal(ByteSize.FromBits(2), size);
        }

        [Fact]
        public void HumanizesBits()
        {
            var humanized = (2).Bits().Humanize();
            Assert.Equal("2 b", humanized);
        }
        
        public void HumanizesWithFormat()
        {
            // TODO
        }
    }
}
