[UseCulture("en-US")]
public class FractionalizeTests
{
    [Theory]
    [InlineData("1", "1")]
    [InlineData("1.25", "1 1/4")]
    [InlineData("2.5", "2 1/2")]
    [InlineData("4.33", "4 1/3")]
    [InlineData("1.8", "1 4/5")]
    [InlineData("0.75", "3/4")]
    public void FractionalizesIssueExamples(string input, string expected) =>
        Assert.Equal(expected, decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(5, 0.01m));

    [Theory]
    [InlineData("0.34", "0.1", 5, "1/3")]
    [InlineData("0.34", "0.0", 5, "0.34")]
    [InlineData("0.34", "0.0", 55, "17/50")]
    [InlineData("0.3", "0.4", 3, "1/3")]
    [InlineData("0.199", "0.1", 5, "1/5")]
    [InlineData("0.199", "0.1", 3, "0.199")]
    [InlineData("0.1", "0.0", 10, "1/10")]
    [InlineData("0.11", "0.0", 10, "0.11")]
    public void HonorsIssueConfigurationTable(
        string input,
        string tolerance,
        int maxDenominator,
        string expected) =>
        Assert.Equal(
            expected,
            decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(
                maxDenominator,
                decimal.Parse(tolerance, CultureInfo.InvariantCulture)));

    public static TheoryData<decimal, int, string> LinkedGistExamples =>
        new()
        {
            { 0.5m, 2, "1/2" },
            { 1m / 3m, 3, "1/3" },
            { 2m / 55m, 55, "2/55" },
            { 7m / 555m, 555, "7/555" },
            { 7m / 5555m, 5555, "7/5555" },
            { 7m / 55555m, 55555, "7/55555" },
            { 17m / 555555m, 555555, "17/555555" },
            { 17m / 5555555m, 5555555, "17/5555555" },
            { 17m / 55555555m, 55555555, "17/55555555" },
            { 1m, 1, "1" },
            { 0.67m, 3, "2/3" },
        };

    [Theory]
    [MemberData(nameof(LinkedGistExamples))]
    public void CoversLinkedGistExamples(decimal input, int maxDenominator, string expected) =>
        Assert.Equal(expected, input.Fractionalize(maxDenominator, 0.01m));

    [Theory]
    [InlineData("0.5", "½")]
    [InlineData("0.3333333333333333333333333333", "⅓")]
    [InlineData("0.6666666666666666666666666667", "⅔")]
    [InlineData("0.25", "¼")]
    [InlineData("0.75", "¾")]
    [InlineData("0.2", "⅕")]
    [InlineData("0.4", "⅖")]
    [InlineData("0.6", "⅗")]
    [InlineData("0.8", "⅘")]
    [InlineData("0.1666666666666666666666666667", "⅙")]
    [InlineData("0.8333333333333333333333333333", "⅚")]
    [InlineData("0.1428571428571428571428571429", "⅐")]
    [InlineData("0.125", "⅛")]
    [InlineData("0.375", "⅜")]
    [InlineData("0.625", "⅝")]
    [InlineData("0.875", "⅞")]
    [InlineData("0.1111111111111111111111111111", "⅑")]
    [InlineData("0.1", "⅒")]
    public void UsesUnicodeVulgarFractionWhenAvailable(string input, string expected) =>
        Assert.Equal(
            expected,
            decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(10, 0.0000000000000000000000000001m, true));

    [Fact]
    public void UsesSlashNotationWhenUnicodeVulgarFractionIsUnavailable() =>
        Assert.Equal("2/7", (2m / 7m).Fractionalize(7, 0.0000000000000000000000000001m, true));

    [Theory]
    [InlineData("1.25", "1¼")]
    [InlineData("-1.25", "-1¼")]
    [InlineData("-0.75", "-¾")]
    [InlineData("4.333", "4⅓")]
    [InlineData("1.8", "1⅘")]
    public void FormatsSignedUnicodeMixedFractions(string input, string expected) =>
        Assert.Equal(
            expected,
            decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(5, 0.01m, true));

    [Theory]
    [InlineData("-1.25", "-1 1/4")]
    [InlineData("-0.75", "-3/4")]
    [InlineData("0.50", "1/2")]
    [InlineData("0.5", "1")]
    public void HandlesSignsReductionAndTies(string input, string expected)
    {
        var maxDenominator = input == "0.5" ? 1 : 4;
        var tolerance = input == "0.5" ? 0.5m : 0m;

        Assert.Equal(
            expected,
            decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(maxDenominator, tolerance));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1.00")]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    public void FormatsWholeValuesWithoutAFraction(string input) =>
        Assert.Equal(
            decimal.Truncate(decimal.Parse(input, CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture),
            decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(10, 0m));

    [Theory]
    [InlineData("7922816251426433759354395033.5", "7922816251426433759354395033 1/2")]
    [InlineData("-7922816251426433759354395033.5", "-7922816251426433759354395033 1/2")]
    public void HandlesDecimalExtremes(string input, string expected) =>
        Assert.Equal(
            expected,
            decimal.Parse(input, CultureInfo.InvariantCulture).Fractionalize(2, 0m));

    [Fact]
    [UseCulture("fr-FR")]
    public void UsesCurrentCultureForDecimalFallback() =>
        Assert.Equal("0,11", 0.11m.Fractionalize(10, 0m));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectsInvalidMaximumDenominator(int maxDenominator) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => 0.5m.Fractionalize(maxDenominator, 0m));

    [Fact]
    public void RejectsNegativeTolerance() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => 0.5m.Fractionalize(2, -0.01m));
}