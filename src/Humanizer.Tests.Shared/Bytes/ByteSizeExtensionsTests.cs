namespace Humanizer.Tests.Bytes
{
    [UseCulture("en")]
    public class ByteSizeExtensionsTests
    {
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
        [InlineData(2, null, "en", "2 TB")]
        [InlineData(2, null, "fr", "2 To")]
        [InlineData(2, "GB", "en", "2000 GB")]
        [InlineData(2.1, null, "en", "2.1 TB")]
        [InlineData(2.123, "#.#", "en", "2.1 TB")]
        [InlineData(2.1, null, "ru-RU", "2,1 TB")]
        [InlineData(2.123, "#.#", "ru-RU", "2,1 TB")]
        public void HumanizesTerabytes(double input, string format, string cultureName, string expectedValue)
        {
            var culture = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Terabytes().Humanize(format, culture));
        }

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

        [Theory]
        [InlineData(2, null, "en", "2.2 TB")]
        [InlineData(2, null, "fr", "2,2 To")]
        [InlineData(2, "TiB", "en", "2 TiB")]
        [InlineData(2, "TiB", "fr", "2 Tio")]
        [InlineData(2, "GiB", "en", "2048 GiB")]
        [InlineData(2.1, null, "en", "2.31 TB")]
        [InlineData(2.1, "TiB", "en", "2.1 TiB")]
        [InlineData(2.123, "#.#", "en", "2.3 TB")]
        [InlineData(2.123, "#.# TiB", "en", "2.1 TiB")]
        [InlineData(2.1, null, "ru-RU", "2,31 TB")]
        [InlineData(2.123, "#.# TiB", "ru-RU", "2,1 TiB")]
        public void HumanizesTebibytes(double input, string format, string cultureName, string expectedValue)
        {
            var culture = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Tebibytes().Humanize(format, culture));
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
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "GB", "en", "0 GB")]
        [InlineData(2, null, "en", "2 GB")]
        [InlineData(2, null, "fr", "2 Go")]
        [InlineData(2, "MB", "en", "2000 MB")]
        [InlineData(2.123, "#.##", "en", "2.12 GB")]
        public void HumanizesGigabytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Gigabytes().Humanize(format, cultureInfo));
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

        [Theory]
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "GiB", "en", "0 GiB")]
        [InlineData(2, null, "en", "2.15 GB")]
        [InlineData(2, "GiB", "en", "2 GiB")]
        [InlineData(2, null, "fr", "2,15 Go")]
        [InlineData(2, "GiB", "fr", "2 Gio")]
        [InlineData(2, "MiB", "en", "2048 MiB")]
        [InlineData(2.123, "#.##", "en", "2.28 GB")]
        [InlineData(2.123, "#.## GiB", "en", "2.12 GiB")]
        public void HumanizesGibibytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Gibibytes().Humanize(format, cultureInfo));
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
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "MB", "en", "0 MB")]
        [InlineData(2, null, "en", "2 MB")]
        [InlineData(2, null, "fr", "2 Mo")]
        [InlineData(2, "KB", "en", "2000 KB")]
        [InlineData(2.123, "#", "en", "2 MB")]
        public void HumanizesMegabytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Megabytes().Humanize(format, cultureInfo));
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

        [Theory]
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "MiB", "en", "0 MiB")]
        [InlineData(2, null, "en", "2.1 MB")]
        [InlineData(2, "MiB", "en", "2 MiB")]
        [InlineData(2, null, "fr", "2,1 Mo")]
        [InlineData(2, "MiB", "fr", "2 Mio")]
        [InlineData(2, "KiB", "en", "2048 KiB")]
        [InlineData(2.123, "#", "en", "2 MB")]
        [InlineData(2.123, "# MiB", "en", "2 MiB")]
        public void HumanizesMebibytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Mebibytes().Humanize(format, cultureInfo));
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
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "KB", "en", "0 KB")]
        [InlineData(2, null, "en", "2 KB")]
        [InlineData(2, null, "fr", "2 Ko")]
        [InlineData(2, "B", "en", "2000 B")]
        [InlineData(2.123, "#.####", "en", "2.123 KB")]
        public void HumanizesKilobytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Kilobytes().Humanize(format, cultureInfo));
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

        [Theory]
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "KiB", "en", "0 KiB")]
        [InlineData(2, null, "en", "2.05 KB")]
        [InlineData(2, "KiB", "en", "2 KiB")]
        [InlineData(2, null, "fr", "2,05 Ko")]
        [InlineData(2, "KiB", "fr", "2 Kio")]
        [InlineData(2, "B", "en", "2048 B")]
        [InlineData(2.123, "#.####", "en", "2.174 KB")]
        [InlineData(2.123, "#.#### KiB", "en", "2.123 KiB")]
        public void HumanizesKibibytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Kibibytes().Humanize(format, cultureInfo));
        }

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
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "#.##", "en", "0 b")]
        [InlineData(0, "#.## B", "en", "0 B")]
        [InlineData(0, "B", "en", "0 B")]
        [InlineData(2, null, "en", "2 B")]
        [InlineData(2, null, "fr", "2 o")]
        [InlineData(2000, "KB", "en", "2 KB")]
        [InlineData(2123, "#.##", "en", "2.12 KB")]
        [InlineData(10000000, "KB", "en", "10000 KB")]
        [InlineData(10000000, "#,##0 KB", "en", "10,000 KB")]
        [InlineData(10000000, "#,##0.# KB", "en", "10,000 KB")]
        public void HumanizesBytes(double input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Bytes().Humanize(format, cultureInfo));
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
        [InlineData(0, null, "en", "0 b")]
        [InlineData(0, "b", "en", "0 b")]
        [InlineData(2, null, "en", "2 b")]
        [InlineData(2, null, "fr", "2 b")]
        [InlineData(12, "B", "en", "1.5 B")]
        [InlineData(10000, "#.# KB", "en", "1.3 KB")]
        [InlineData(30000, "#.# KB", "en", "3.8 KB")]
        [InlineData(30000, "#.# KiB", "en", "3.7 KiB")]
        public void HumanizesBits(long input, string format, string cultureName, string expectedValue)
        {
            var cultureInfo = new CultureInfo(cultureName);

            Assert.Equal(expectedValue, input.Bits().Humanize(format, cultureInfo));
        }
    }
}
