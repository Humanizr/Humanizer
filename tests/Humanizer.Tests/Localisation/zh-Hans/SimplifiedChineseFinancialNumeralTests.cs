namespace Humanizer.Tests.Localisation.zh_Hans;

[UseCulture("zh-Hans")]
public class SimplifiedChineseFinancialNumeralTests
{
    static readonly CultureInfo ZhHans = CultureInfo.GetCultureInfo("zh-Hans");

    [Theory]
    [InlineData(0, "零")]
    [InlineData(-5, "负伍")]
    [InlineData(10, "壹拾")]
    [InlineData(1001000001, "壹拾亿零壹佰万零壹")]
    public void ConvertsFinancialCharacters(long number, string expected) =>
        Assert.Equal(expected, number.ToChineseFinancialCharacters(ZhHans));

    [Fact]
    public void ConvertsFullLongRange() =>
        Assert.Equal(
            "负玖佰贰拾贰京叁仟叁佰柒拾贰兆零叁佰陆拾捌亿伍仟肆佰柒拾柒万伍仟捌佰零捌",
            long.MinValue.ToChineseFinancialCharacters(ZhHans));

    [Theory]
    [InlineData("zh-CN")]
    [InlineData("zh-SG")]
    public void ResolvesSimplifiedCultureFallback(string cultureName) =>
        Assert.Equal("贰", 2.ToChineseFinancialCharacters(CultureInfo.GetCultureInfo(cultureName)));

    [Theory]
    [InlineData(-23, "负 第 二十三")]
    [InlineData(int.MinValue, "负 第 二十一亿四千七百四十八万三千六百四十八")]
    public void ConvertsNegativeStandardOrdinals(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords(ZhHans));

    [Fact]
    public void DoesNotReplaceStandardChineseWords() =>
        Assert.Equal("十", 10.ToWords(ZhHans));
}