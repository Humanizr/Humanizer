[UseCulture("en")]
public class LowerIecDataUnitTests
{
    [Fact]
    public void CreatesLowerIecUnitsWithExistingBinaryFactors()
    {
        var kibibyte = ByteSize.FromKibibytes(1);
        var mebibyte = ByteSize.FromMebibytes(1);
        var gibibyte = ByteSize.FromGibibytes(1);
        var tebibyte = ByteSize.FromTebibytes(1);

        Assert.Equal(ByteSize.BytesInKilobyte, ByteSize.BytesInKibibyte);
        Assert.Equal(ByteSize.BytesInMegabyte, ByteSize.BytesInMebibyte);
        Assert.Equal(ByteSize.BytesInGigabyte, ByteSize.BytesInGibibyte);
        Assert.Equal(ByteSize.BytesInTerabyte, ByteSize.BytesInTebibyte);
        Assert.Equal(1, kibibyte.Kibibytes);
        Assert.Equal(1, mebibyte.Mebibytes);
        Assert.Equal(1, gibibyte.Gibibytes);
        Assert.Equal(1, tebibyte.Tebibytes);
    }

    [Theory]
    [InlineData(1024, "0 KiB", "1 KiB")]
    [InlineData(1048576, "0 MiB", "1 MiB")]
    [InlineData(1073741824, "0 GiB", "1 GiB")]
    [InlineData(1099511627776, "0 TiB", "1 TiB")]
    public void FormatsExplicitLowerIecUnits(double bytes, string format, string expected) =>
        Assert.Equal(expected, ByteSize.FromBytes(bytes).ToString(format));

    [Theory]
    [InlineData("1KiB", 1024)]
    [InlineData("1.5 mib", 1572864)]
    [InlineData("1GiB", 1073741824)]
    [InlineData("1TiB", 1099511627776)]
    public void ParsesLowerIecUnitsFromStringsAndSpans(string input, double expectedBytes)
    {
        Assert.True(ByteSize.TryParse(input, out var parsed));
        Assert.Equal(expectedBytes, parsed.Bytes);

        Assert.True(ByteSize.TryParse(input.AsSpan(), out parsed));
        Assert.Equal(expectedBytes, parsed.Bytes);
    }

    [Fact]
    public void LocalizesExplicitLowerIecUnits()
    {
        var french = new CultureInfo("fr");
        var spanish = new CultureInfo("es");

        Assert.Equal("1 Kio", ByteSize.FromKibibytes(1).Humanize("KiB", french));
        Assert.Equal("1 mébioctet", ByteSize.FromMebibytes(1).ToFullWords("MiB", french));
        Assert.Equal("1 GiB", ByteSize.FromGibibytes(1).Humanize("GiB", spanish));
        Assert.Equal("1 gibibyte", ByteSize.FromGibibytes(1).ToFullWords("GiB", spanish));
    }

    [Fact]
    public void PreservesLegacyDefaultsAndEnumValues()
    {
        Assert.Equal("1 KB", ByteSize.FromKibibytes(1).ToString());
        Assert.Equal(ByteSize.FromKilobytes(1), ByteSize.Parse("1 KB"));
        Assert.Equal("1 KB/s", ByteSize.FromKibibytes(1).Per(TimeSpan.FromSeconds(1)).Humanize());
        Assert.Equal(8, (int)DataUnit.Pebibyte);
        Assert.Equal(9, (int)DataUnit.Kibibyte);
    }
}