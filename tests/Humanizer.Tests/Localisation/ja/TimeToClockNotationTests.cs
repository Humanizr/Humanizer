#if NET6_0_OR_GREATER

namespace ja;

[UseCulture("ja-JP")]
public class TimeToClockNotationTests
{
    [Theory]
    [InlineData(0, 0, "0時0分")]
    [InlineData(13, 23, "13時23分")]
    [InlineData(15, 45, "15時45分")]
    public void ToClockNotation_UsesJapaneseNumericClockNotation(int hours, int minutes, string expected) =>
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());

    [Theory]
    [InlineData(13, 23, "13時25分")]
    [InlineData(23, 58, "0時0分")]
    public void ToClockNotation_RoundedUsesJapaneseNumericClockNotation(int hours, int minutes, string expected) =>
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
}

#endif
