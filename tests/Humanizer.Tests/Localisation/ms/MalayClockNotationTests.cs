namespace Humanizer.Tests.Localisation.ms;

#if NET6_0_OR_GREATER
[UseCulture("ms")]
public class MalayClockNotationTests
{
    [Theory]
    [InlineData(0, "pukul dua belas malam")]
    [InlineData(1, "pukul satu pagi")]
    [InlineData(5, "pukul lima pagi")]
    [InlineData(6, "pukul enam pagi")]
    [InlineData(11, "pukul sebelas pagi")]
    [InlineData(12, "pukul dua belas petang")]
    [InlineData(20, "pukul lapan petang")]
    [InlineData(21, "pukul sembilan malam")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif