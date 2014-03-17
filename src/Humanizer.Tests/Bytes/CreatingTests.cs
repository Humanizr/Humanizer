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
            Assert.Equal(1073741824, result.KiloBytes);
            Assert.Equal(1048576, result.MegaBytes);
            Assert.Equal(1024, result.GigaBytes);
            Assert.Equal(1, result.TeraBytes);
        }

        [Fact]
        public void FromBitsMethod()
        {
            var result = ByteSize.FromBits(8);

            Assert.Equal(8, result.Bits);
            Assert.Equal(1, result.Bytes);
        }

        [Fact]
        public void FromBytesMethod()
        {
            var result = ByteSize.FromBytes(1.5);

            Assert.Equal(12, result.Bits);
            Assert.Equal(1.5, result.Bytes);
        }

        [Fact]
        public void FromKiloBytesMethod()
        {
            var result = ByteSize.FromKiloBytes(1.5);

            Assert.Equal(1536, result.Bytes);
            Assert.Equal(1.5, result.KiloBytes);
        }

        [Fact]
        public void FromMegaBytesMethod()
        {
            var result = ByteSize.FromMegaBytes(1.5);

            Assert.Equal(1572864, result.Bytes);
            Assert.Equal(1.5, result.MegaBytes);
        }

        [Fact]
        public void FromGigaBytesMethod()
        {
            var result = ByteSize.FromGigaBytes(1.5);

            Assert.Equal(1610612736, result.Bytes);
            Assert.Equal(1.5, result.GigaBytes);
        }

        [Fact]
        public void FromTeraBytesMethod()
        {
            var result = ByteSize.FromTeraBytes(1.5);

            Assert.Equal(1649267441664, result.Bytes);
            Assert.Equal(1.5, result.TeraBytes);
        }
    }
}
