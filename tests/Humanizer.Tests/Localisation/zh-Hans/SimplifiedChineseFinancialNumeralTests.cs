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

    [Fact]
    public void DoesNotReplaceStandardChineseWords() =>
        Assert.Equal("十", 10.ToWords(ZhHans));
}