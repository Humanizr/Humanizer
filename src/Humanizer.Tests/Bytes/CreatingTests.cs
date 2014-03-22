using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    public class CreatingTests
    {
        [Fact]
        public void Constructor()
        {
            var result = new ByteSize(1099511627776);
            
            Assert.Equal(8.796093022208e12, result.Bits);
            Assert.Equal(1099511627776, result.Bytes);
            Assert.Equal(1073741824, result.Kilobytes);
            Assert.Equal(1048576, result.Megabytes);
            Assert.Equal(1024, result.Gigabytes);
            Assert.Equal(1, result.Terabytes);
        }

        [Fact]
        public void FromBits()
        {
            var result = ByteSize.FromBits(8);

            Assert.Equal(8, result.Bits);
            Assert.Equal(1, result.Bytes);
        }

        [Fact]
        public void FromBytes()
        {
            var result = ByteSize.FromBytes(1.5);

            Assert.Equal(12, result.Bits);
            Assert.Equal(1.5, result.Bytes);
        }

        [Fact]
        public void FromKilobytes()
        {
            var result = ByteSize.FromKilobytes(1.5);

            Assert.Equal(1536, result.Bytes);
            Assert.Equal(1.5, result.Kilobytes);
        }

        [Fact]
        public void FromMegabytes()
        {
            var result = ByteSize.FromMegabytes(1.5);

            Assert.Equal(1572864, result.Bytes);
            Assert.Equal(1.5, result.Megabytes);
        }

        [Fact]
        public void FromGigabytes()
        {
            var result = ByteSize.FromGigabytes(1.5);

            Assert.Equal(1610612736, result.Bytes);
            Assert.Equal(1.5, result.Gigabytes);
        }

        [Fact]
        public void FromTerabytes()
        {
            var result = ByteSize.FromTerabytes(1.5);

            Assert.Equal(1649267441664, result.Bytes);
            Assert.Equal(1.5, result.Terabytes);
        }
    }
}
