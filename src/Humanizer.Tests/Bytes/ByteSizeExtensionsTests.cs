﻿using Humanizer.Bytes;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Bytes
{
    public class ByteSizeExtensionsTests : AmbientCulture
    {
        public ByteSizeExtensionsTests() : base("en") { }

        [Fact]
        public void ByteTerabytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
        }

        [Fact]
        public void SbyteTerabytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
        }

        [Fact]
        public void ShortTerabytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
        }

        [Fact]
        public void UshortTerabytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
        }

        [Fact]
        public void IntTerabytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
        }

        [Fact]
        public void UintTerabytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
        }

        [Fact]
        public void DoubleTerabytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromTerabytes(2), size.Terabytes());
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
        public void ByteGigabytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
        }

        [Fact]
        public void SbyteGigabytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
        }

        [Fact]
        public void ShortGigabytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
        }

        [Fact]
        public void UshortGigabytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
        }

        [Fact]
        public void IntGigabytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
        }

        [Fact]
        public void UintGigabytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
        }

        [Fact]
        public void DoubleGigabytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromGigabytes(2), size.Gigabytes());
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
        public void ByteMegabytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
        }

        [Fact]
        public void SbyteMegabytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
        }

        [Fact]
        public void ShortMegabytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
        }

        [Fact]
        public void UshortMegabytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
        }

        [Fact]
        public void IntMegabytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
        }

        [Fact]
        public void UintMegabytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
        }

        [Fact]
        public void DoubleMegabytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromMegabytes(2), size.Megabytes());
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
        public void ByteKilobytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
        }

        [Fact]
        public void SbyteKilobytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
        }

        [Fact]
        public void ShortKilobytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
        }

        [Fact]
        public void UshortKilobytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
        }

        [Fact]
        public void IntKilobytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
        }

        [Fact]
        public void UintKilobytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
        }

        [Fact]
        public void DoubleKilobytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromKilobytes(2), size.Kilobytes());
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
        public void ByteBytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
        }

        [Fact]
        public void SbyteBytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
        }

        [Fact]
        public void ShortBytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
        }

        [Fact]
        public void UshortBytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
        }

        [Fact]
        public void IntBytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
        }

        [Fact]
        public void UintBytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
        }

        [Fact]
        public void DoubleBytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromBytes(2), size.Bytes());
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
        public void ByteBits()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
        }

        [Fact]
        public void SbyteBits()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
        }

        [Fact]
        public void ShortBits()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
        }

        [Fact]
        public void UshortBits()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
        }

        [Fact]
        public void IntBits()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
        }

        [Fact]
        public void UintBits()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
        }

        [Fact]
        public void LongBits()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromBits(2), size.Bits());
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
