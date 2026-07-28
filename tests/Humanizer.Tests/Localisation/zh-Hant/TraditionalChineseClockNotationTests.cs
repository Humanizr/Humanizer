namespace Humanizer.Tests.Localisation.zh_Hant;

#if NET6_0_OR_GREATER
[UseCulture("zh-Hant")]
public class TraditionalChineseClockNotationTests
{
    [Theory]
    [InlineData(0, "晚上十二")]
    [InlineData(1, "凌晨一")]
    [InlineData(5, "凌晨五")]
    [InlineData(6, "上午六")]
    [InlineData(11, "上午十一")]
    [InlineData(12, "下午十二")]
    [InlineData(20, "下午八")]
    [InlineData(21, "晚上九")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif