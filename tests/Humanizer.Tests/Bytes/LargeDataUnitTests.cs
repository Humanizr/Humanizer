[UseCulture("en")]
public class LargeDataUnitTests
{
    [Fact]
    public void CreatesTheLargestSupportedSiAndIecUnits()
    {
        var petabyte = ByteSize.FromPetabytes(1);
        var exabyte = ByteSize.FromExabytes(1);
        var pebibyte = ByteSize.FromPebibytes(1);

        Assert.Equal(1000000000000000, petabyte.Bytes);
        Assert.Equal(8000000000000000, petabyte.Bits);
        Assert.Equal(1, petabyte.Petabytes);

        Assert.Equal(1000000000000000000, exabyte.Bytes);
        Assert.Equal(8000000000000000000, exabyte.Bits);
        Assert.Equal(1, exabyte.Exabytes);

        Assert.Equal(1125899906842624, pebibyte.Bytes);
        Assert.Equal(9007199254740992, pebibyte.Bits);
        Assert.Equal(1, pebibyte.Pebibytes);
    }

    [Fact]
    public void NumericExtensionsCreateLargeUnits()
    {
        Assert.Equal(ByteSize.FromPetabytes(1), 1.Petabytes());
        Assert.Equal(ByteSize.FromExabytes(1), 1.Exabytes());
        Assert.Equal(ByteSize.FromPebibytes(1), 1.Pebibytes());
    }

    [Theory]
    [InlineData(999999999999999, "909.49 TB")]
    [InlineData(1000000000000000, "1 PB")]
    [InlineData(-1000000000000000, "-1 PB")]
    [InlineData(1000000000000000000, "1 EB")]
    public void SelectsSiUnitsAtTheirDecimalBoundaries(double bytes, string expected) =>
        Assert.Equal(expected, ByteSize.FromBytes(bytes).ToString());

    [Theory]
    [InlineData(1, "1 petabyte")]
    [InlineData(2, "2 petabytes")]
    public void FormatsPetabytesAsFullWords(double petabytes, string expected) =>
        Assert.Equal(expected, ByteSize.FromPetabytes(petabytes).ToFullWords());

    [Fact]
    public void FormatsExplicitSiAndIecUnits()
    {
        Assert.Equal("1.5 PB", ByteSize.FromPetabytes(1.5).ToString("0.0 PB"));
        Assert.Equal("1 EB", ByteSize.FromExabytes(1).ToString("0 EB"));
        Assert.Equal("1 PiB", ByteSize.FromPebibytes(1).ToString("0 PiB"));
        Assert.Equal("1 Pio", ByteSize.FromPebibytes(1).ToString("0 pib", new CultureInfo("fr")));
        Assert.Equal("1 pebibyte", ByteSize.FromPebibytes(1).ToFullWords("0 PiB"));
    }

    [Theory]
    [InlineData("1PB", 1000000000000000)]
    [InlineData("1.1 eb", 1100000000000000000)]
    [InlineData("1PiB", 1125899906842624)]
    public void ParsesLargeUnitsFromStringsAndSpans(string input, double expectedBytes)
    {
        Assert.True(ByteSize.TryParse(input, out var parsed));
        Assert.Equal(expectedBytes, parsed.Bytes, 256d);

        Assert.True(ByteSize.TryParse(input.AsSpan(), out parsed));
        Assert.Equal(expectedBytes, parsed.Bytes, 256d);
    }

    [Fact]
    public void AddsAndComparesLargeUnits()
    {
        var one = ByteSize.FromPetabytes(1);
        var two = one.AddPetabytes(1);

        Assert.Equal(ByteSize.FromPetabytes(2), two);
        Assert.True(two > one);
    }

    [Fact]
    public void HumanizesExabyteRates() =>
        Assert.Equal(
            "1 EB/s",
            ByteSize.FromExabytes(1)
                .Per(TimeSpan.FromSeconds(1))
                .Humanize());

    [Fact]
    public void FallsBackToEnglishWhenTheLocaleDoesNotYetDefineLargeUnits()
    {
        var spanish = new CultureInfo("es");
        var size = ByteSize.FromExabytes(1);

        Assert.Equal("1 EB", size.Humanize(spanish));
        Assert.Equal("1 exabyte", size.ToFullWords(provider: spanish));
    }

    [Fact]
    public void UsesLocalizedLargeUnitPhrasesWhenAvailable()
    {
        var french = new CultureInfo("fr");

        Assert.Equal("2 Po", ByteSize.FromPetabytes(2).Humanize(french));
        Assert.Equal("1 exaoctet", ByteSize.FromExabytes(1).ToFullWords(provider: french));
        Assert.Equal("1 Pio", ByteSize.FromPebibytes(1).Humanize("PiB", french));
    }

    [Theory]
    [InlineData("2EB")]
    [InlineData("1024PiB")]
    [InlineData("InfinityPB")]
    [InlineData("NaNPB")]
    public void RejectsValuesOutsideTheLongBitRange(string input) =>
        Assert.False(ByteSize.TryParse(input, CultureInfo.InvariantCulture, out _));

    [Fact]
    public void FactoriesRejectValuesOutsideTheLongBitRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromExabytes(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromPebibytes(1024));
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromPetabytes(double.NaN));
    }

    [Fact]
    public void AcceptsTheMinimumIecValue() =>
        Assert.Equal(long.MinValue, ByteSize.FromPebibytes(-1024).Bits);

    [Fact]
    public void RejectsArithmeticOutsideTheLongBitRange()
    {
        var positive = ByteSize.FromPebibytes(800);

        Assert.Throws<OverflowException>(() => positive.AddPebibytes(800));
        Assert.Throws<OverflowException>(() => ByteSize.FromExabytes(1).AddExabytes(1));
    }

    [Fact]
    public void DoesNotClaimUnsupportedExbibytes() =>
        Assert.False(ByteSize.TryParse("1EiB", out _));
}