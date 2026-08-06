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
    [InlineData(2, "GB", "en", "2048 GB")]
    [InlineData(2.1, null, "en", "2.1 TB")]
    [InlineData(2.123, "#.#", "en", "2.1 TB")]
    public void HumanizesTerabytes(double input, string? format, string cultureName, string expectedValue)
    {
        var culture = new CultureInfo(cultureName);

        Assert.Equal(expectedValue, input
            .Terabytes()
            .Humanize(format, culture));
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
    [InlineData(2, "MB", "en", "2048 MB")]
    [InlineData(2.123, "#.##", "en", "2.12 GB")]
    public void HumanizesGigabytes(double input, string? format, string cultureName, string expectedValue)
    {
        var cultureInfo = new CultureInfo(cultureName);

        Assert.Equal(expectedValue, input
            .Gigabytes()
            .Humanize(format, cultureInfo));
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
    [InlineData(2, "KB", "en", "2048 KB")]
    [InlineData(2.123, "#", "en", "2 MB")]
    public void HumanizesMegabytes(double input, string? format, string cultureName, string expectedValue)
    {
        var cultureInfo = new CultureInfo(cultureName);

        Assert.Equal(expectedValue, input
            .Megabytes()
            .Humanize(format, cultureInfo));
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
    [InlineData(2, "B", "en", "2048 B")]
    [InlineData(2.123, "#.####", "en", "2.123 KB")]
    public void HumanizesKilobytes(double input, string? format, string cultureName, string expectedValue)
    {
        var cultureInfo = new CultureInfo(cultureName);

        Assert.Equal(expectedValue, input
            .Kilobytes()
            .Humanize(format, cultureInfo));
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
    [InlineData(2000, "KB", "en", "1.95 KB")]
    [InlineData(2123, "#.##", "en", "2.07 KB")]
    [InlineData(10000000, "KB", "en", "9765.63 KB")]
    [InlineData(10000000, "#,##0 KB", "en", "9,766 KB")]
    [InlineData(10000000, "#,##0.# KB", "en", "9,765.6 KB")]
    public void HumanizesBytes(double input, string? format, string cultureName, string expectedValue)
    {
        var cultureInfo = new CultureInfo(cultureName);

        Assert.Equal(expectedValue, input
            .Bytes()
            .Humanize(format, cultureInfo));
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

    [Fact]
    public void DoubleBits()
    {
        const double size = 2.5;
        Assert.Equal(0.3125, size.Bits().Bytes);
    }

    [Theory]
    [InlineData(0, null, "en", "0 b")]
    [InlineData(0, "b", "en", "0 b")]
    [InlineData(2, null, "en", "2 b")]
    [InlineData(2, null, "fr", "2 b")]
    [InlineData(12, "B", "en", "1.5 B")]
    [InlineData(10000, "#.# KB", "en", "1.2 KB")]
    public void HumanizesBits(long input, string? format, string cultureName, string expectedValue)
    {
        var cultureInfo = new CultureInfo(cultureName);

        Assert.Equal(expectedValue, input
            .Bits()
            .Humanize(format, cultureInfo));
    }

    [Fact]
    public void HumanizesCompositeBytesWithoutChangingDefaultHumanize()
    {
        var input = 10242.Bytes();

        Assert.Equal("10 KB", input.Humanize());
        Assert.Equal("10 KB 2 B", input.HumanizeComposite());
        Assert.Equal("10 kilobytes, 2 bytes", input.HumanizeComposite(separator: ", ", toWords: true));
    }

    [Theory]
    [InlineData(1, "1 MB")]
    [InlineData(2, "1 MB 1 KB")]
    [InlineData(3, "1 MB 1 KB 1 B")]
    public void LimitsCompositeOutputToNonZeroParts(int precision, string expected)
    {
        var input = ByteSize.FromBytes(ByteSize.BytesInMegabyte + ByteSize.BytesInKilobyte + 1);

        Assert.Equal(expected, input.HumanizeComposite(precision));
    }

    [Theory]
    [InlineData(1023, "1023 B")]
    [InlineData(1024, "1 KB")]
    [InlineData(0, "0 b")]
    public void HumanizesCompositeBoundaries(long bytes, string expected) =>
        Assert.Equal(expected, bytes.Bytes().HumanizeComposite());

    [Fact]
    public void HumanizesCompositeBitResidualsExactly() =>
        Assert.Equal("10 KB 2 B 1 b", ByteSize.FromBits(81937).HumanizeComposite(3));

    [Fact]
    public void UsesExistingLargeUnitFactorsInDescendingOrder()
    {
        var input = ByteSize.FromBytes(ByteSize.BytesInPetabyte + ByteSize.BytesInTerabyte + ByteSize.BytesInGigabyte);

        Assert.Equal("1 PB 1 TB 1 GB", input.HumanizeComposite(3));
        Assert.Equal("1 EB", ByteSize.FromExabytes(1).HumanizeComposite());
    }

    [Fact]
    public void HumanizesNegativeCompositeValuesWithOneLeadingSign()
    {
        var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        numberFormat.NegativeSign = "~";

        Assert.Equal("-10 KB 2 B", (-10242).Bytes().HumanizeComposite());
        Assert.Equal("~10 KB 2 B", (-10242).Bytes().HumanizeComposite(formatProvider: numberFormat));
        Assert.Equal("-1 EB", ByteSize.FromBits(long.MinValue).HumanizeComposite(1));
    }

    [Fact]
    public void HumanizesCompositeWithSeparatorAndCulture()
    {
        var input = ByteSize.FromBytes(ByteSize.BytesInMegabyte + ByteSize.BytesInKilobyte + 1);

        Assert.Equal("1 MB, 1 KB, 1 B", input.HumanizeComposite(3, separator: ", "));
        Assert.Equal("1 Mo 1 Ko 1 o", input.HumanizeComposite(3, new CultureInfo("fr")));
        Assert.Equal("1 megabyte, 1 kilobyte, 1 byte", input.HumanizeComposite(3, separator: ", ", toWords: true));
    }

    [Fact]
    public void PreservesLegacyLabelsForLowerIecCompositeValues()
    {
        var input = ByteSize.FromBytes(ByteSize.BytesInMebibyte + ByteSize.BytesInKibibyte);

        Assert.Equal("1 MB 1 KB", input.HumanizeComposite());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectsInvalidCompositePrecision(int precision)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 1.Bytes().HumanizeComposite(precision));

        Assert.Equal(nameof(precision), exception.ParamName);
    }

    [Fact]
    public void RejectsNullCompositeSeparator() =>
        Assert.Throws<ArgumentNullException>(() => 1.Bytes().HumanizeComposite(separator: null!));
}