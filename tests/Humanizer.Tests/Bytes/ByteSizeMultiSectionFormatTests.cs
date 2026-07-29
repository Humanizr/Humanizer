[UseCulture("en-US")]
public class ByteSizeMultiSectionFormatTests
{
    [Theory]
    [InlineData(1_000_000, "1 megabyte")]
    [InlineData(-2_000_000, "-2 megabytes")]
    [InlineData(0, "0 megabytes")]
    public void ReplacesExplicitUnitTokensInEveryCustomFormatSection(double bytes, string expected)
    {
        var result = ByteSize.FromBytes(bytes).FormatFullWords(
            ByteSizeUnitSystem.DecimalSi,
            "0 MB;-0 MB;0 MB",
            CultureInfo.GetCultureInfo("en-US"));

        Assert.Equal(expected, result);
    }

    [Fact]
    public void LegacyFullWordsPreservesNegativeImplicitSuffixBehavior() =>
        Assert.Equal(
            "-2 megabyte",
            ByteSize.FromMegabytes(-2).ToFullWords("0", CultureInfo.GetCultureInfo("en-US")));

    [Fact]
    public void DecoratedNegativeExplicitFormatUsesMagnitudeForGrammar() =>
        Assert.Equal(
            "negative 2 megabytes",
            ByteSize.FromDecimalMegabytes(-2).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0 MB;'negative '0 MB;0 MB",
                CultureInfo.GetCultureInfo("en-US")));

    [Fact]
    public void ReplacesEveryExplicitUnitTokenWithLocalizedWords()
    {
        var result = ByteSize.FromDecimalMegabytes(1).FormatFullWords(
            ByteSizeUnitSystem.DecimalSi,
            "0 MB;-0 MB;0 MB",
            CultureInfo.GetCultureInfo("fr"));

        Assert.Equal("1 mégaoctet", result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void LegacyHumanizePreservesEmptyAndWhitespaceFormatBehavior(string format)
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        var size = ByteSize.FromKilobytes(1);

        Assert.Equal(
            size.Humanize(format, culture),
            size.HumanizeWithUnitSystem(ByteSizeUnitSystem.Legacy, format, culture));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void LegacyRateHumanizePreservesEmptyAndWhitespaceFormatBehavior(string format)
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        var rate = ByteSize.FromKilobytes(1).Per(TimeSpan.FromSeconds(1));

        Assert.Equal(
            rate.Humanize(format, TimeUnit.Second, culture),
            rate.HumanizeWithUnitSystem(ByteSizeUnitSystem.Legacy, format, TimeUnit.Second, culture));
    }
}