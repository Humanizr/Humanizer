namespace Humanizer.Tests.Localisation.lv;

#if NET6_0_OR_GREATER
[UseCulture("lv")]
public class LatvianClockNotationTests
{
    static readonly CultureInfo LvLv = new("lv-LV");

    [Theory]
    [InlineData(1, "pieci un viena")]
    [InlineData(2, "pieci un divas")]
    [InlineData(21, "pieci un divdesmit viena")]
    [InlineData(22, "pieci un divdesmit divas")]
    public void ToClockNotation_UsesMinuteValuesThatAgreeWithTheMinuteNoun(int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(5, minutes).ToClockNotation(ClockNotationRounding.None));
    }

    [Fact]
    public void ToClockNotation_RegionalCultureUsesLatvianProfile()
    {
        Assert.Equal(
            "pieci un divdesmit divas",
            new TimeOnly(5, 22).ToClockNotation(ClockNotationRounding.None, LvLv));
    }
}
#endif