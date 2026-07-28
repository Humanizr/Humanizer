namespace Humanizer.Tests.Localisation.zh_Hant;

[UseCulture("zh-Hant")]
public class TraditionalChineseFinancialNumeralTests
{
    static readonly CultureInfo ZhHant = CultureInfo.GetCultureInfo("zh-Hant");

    [Theory]
    [InlineData(0, "零")]
    [InlineData(-5, "負伍")]
    [InlineData(10, "壹拾")]
    [InlineData(1001000001, "壹拾億零壹佰萬零壹")]
    public void ConvertsFinancialCharacters(long number, string expected) =>
        Assert.Equal(expected, number.ToChineseFinancialCharacters(ZhHant));

    [Fact]
    public void ConvertsFullLongRange() =>
        Assert.Equal(
            "負玖佰貳拾貳京參仟參佰柒拾貳兆零參佰陸拾捌億伍仟肆佰柒拾柒萬伍仟捌佰零捌",
            long.MinValue.ToChineseFinancialCharacters(ZhHant));

    [Theory]
    [InlineData("zh-TW")]
    [InlineData("zh-HK")]
    public void ResolvesTraditionalCultureFallback(string cultureName) =>
        Assert.Equal("貳", 2.ToChineseFinancialCharacters(CultureInfo.GetCultureInfo(cultureName)));

    [Theory]
    [InlineData("zh")]
    [InlineData("en-US")]
    public void RejectsUnsupportedCultures(string cultureName) =>
        Assert.Throws<NotSupportedException>(
            () => 1.ToChineseFinancialCharacters(CultureInfo.GetCultureInfo(cultureName)));

    [Fact]
    public void RejectsNullCulture() =>
        Assert.Throws<ArgumentNullException>(() => 1.ToChineseFinancialCharacters(null!));

    [Fact]
    public void DoesNotReplaceStandardChineseWords() =>
        Assert.Equal("十", 10.ToWords(ZhHant));
}