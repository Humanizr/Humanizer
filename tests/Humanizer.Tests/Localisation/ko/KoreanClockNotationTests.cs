namespace Humanizer.Tests.Localisation.ko;

#if NET6_0_OR_GREATER
[UseCulture("ko")]
public class KoreanClockNotationTests
{
    [Theory]
    [InlineData(0, "밤 열두 시")]
    [InlineData(20, "오후 여덟 시")]
    [InlineData(21, "밤 아홉 시")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());

    [Theory]
    [InlineData(1, "오전 한 시")]
    [InlineData(2, "오전 두 시")]
    [InlineData(3, "오전 세 시")]
    [InlineData(4, "오전 네 시")]
    [InlineData(5, "오전 다섯 시")]
    [InlineData(6, "오전 여섯 시")]
    [InlineData(7, "오전 일곱 시")]
    [InlineData(8, "오전 여덟 시")]
    [InlineData(9, "오전 아홉 시")]
    [InlineData(10, "오전 열 시")]
    [InlineData(11, "오전 열한 시")]
    [InlineData(12, "오후 열두 시")]
    public void ToClockNotation_UsesEveryMappedHour(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif