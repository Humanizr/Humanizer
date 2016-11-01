using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    [UseCulture("en")]
    public class ByteSizeExtensionsTests
    {

        #region SI-unit extensions (kilo, mega, giga, etc.)
        [Fact]
        public void ByteTerabytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void SbyteTerabytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void ShortTerabytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void UshortTerabytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void IntTerabytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void UintTerabytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void DoubleTerabytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Fact]
        public void LongTerabytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromTerabytes(size), size.Terabytes());
        }

        [Theory]
        [InlineData(2, null, "2 TB")]
        [InlineData(2, "GB", "2000 GB")]
        [InlineData(2.123, "#.#", "2.1 TB")]
        public void HumanizesTerabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Terabytes().Humanize(format));
        }

        [Fact]
        public void ByteGigabytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void SbyteGigabytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void ShortGigabytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void UshortGigabytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void IntGigabytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void UintGigabytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void DoubleGigabytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Fact]
        public void LongGigabytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromGigabytes(size), size.Gigabytes());
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "GB", "0 GB")]
        [InlineData(2, null, "2 GB")]
        [InlineData(2, "MB", "2000 MB")]
        [InlineData(2.123, "#.##", "2.12 GB")]
        public void HumanizesGigabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Gigabytes().Humanize(format));
        }

        [Fact]
        public void ByteMegabytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void SbyteMegabytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void ShortMegabytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void UshortMegabytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void IntMegabytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void UintMegabytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void DoubleMegabytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Fact]
        public void LongMegabytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromMegabytes(size), size.Megabytes());
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "MB", "0 MB")]
        [InlineData(2, null, "2 MB")]
        [InlineData(2, "kB", "2000 kB")]
        [InlineData(2.123, "#", "2 MB")]
        public void HumanizesMegabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Megabytes().Humanize(format));
        }

        [Fact]
        public void ByteKilobytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void SbyteKilobytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void ShortKilobytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void UshortKilobytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void IntKilobytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void UintKilobytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void DoubleKilobytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Fact]
        public void LongKilobytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromKilobytes(size), size.Kilobytes());
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "kB", "0 kB")]
        [InlineData(2, null, "2 kB")]
        [InlineData(2, "B", "2000 B")]
        [InlineData(2.123, "#.####", "2.123 kB")]
        public void HumanizesKilobytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Kilobytes().Humanize(format));
        }
        #endregion

        #region IEC-unit extensions (kibi, mebi, gibi, etc.)
        [Fact]
        public void ByteTebibytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void SbyteTebibytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void ShortTebibytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void UshortTebibytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void IntTebibytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void UintTebibytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void DoubleTebibytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void LongTebibytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromTebibytes(size), size.Tebibytes());
        }

        [Fact]
        public void ByteGibibytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void SbyteGibibytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void ShortGibibytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void UshortGibibytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void IntGibibytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void UintGibibytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void DoubleGibibytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void LongGibibytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromGibibytes(size), size.Gibibytes());
        }

        [Fact]
        public void ByteMebibytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void SbyteMebibytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void ShortMebibytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void UshortMebibytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void IntMebibytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void UintMebibytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void DoubleMebibytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void LongMebibytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromMebibytes(size), size.Mebibytes());
        }

        [Fact]
        public void ByteKibibytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void SbyteKibibytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void ShortKibibytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void UshortKibibytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void IntKibibytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void UintKibibytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void DoubleKibibytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }

        [Fact]
        public void LongKibibytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromKibibytes(size), size.Kibibytes());
        }
        #endregion

        [Fact]
        public void ByteBytes()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void SbyteBytes()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void ShortBytes()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void UshortBytes()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void IntBytes()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void UintBytes()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void DoubleBytes()
        {
            const double size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Fact]
        public void LongBytes()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromBytes(size), size.Bytes());
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "#.##", "0 b")]
        [InlineData(0, "B", "0 B")]
        [InlineData(2, null, "2 B")]
        [InlineData(2000, "kB", "2 kB")]
        [InlineData(2123, "#.##", "2.12 kB")]
        [InlineData(10000000, "kB", "10000 kB")]
        [InlineData(10000000, "#,##0 kB", "10,000 kB")]
        [InlineData(10000700, "#,##0.# kB", "10,000.7 kB")]
        public void HumanizesBytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Bytes().Humanize(format));
        }

        [Fact]
        public void ByteBits()
        {
            const byte size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Fact]
        public void SbyteBits()
        {
            const sbyte size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Fact]
        public void ShortBits()
        {
            const short size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Fact]
        public void UshortBits()
        {
            const ushort size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Fact]
        public void IntBits()
        {
            const int size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Fact]
        public void UintBits()
        {
            const uint size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Fact]
        public void LongBits()
        {
            const long size = 2;
            Assert.Equal(ByteSize.FromBits(size), size.Bits());
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "b", "0 b")]
        [InlineData(2, null, "2 b")]
        [InlineData(12, "B", "1.5 B")]
        [InlineData(10000, "#.# kB", "1.3 kB")]
        public void HumanizesBits(long input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Bits().Humanize(format));
        }
    }
}
