namespace Humanizer.Tests.Localisation.fa;

#if NET6_0_OR_GREATER
[UseCulture("fa")]
public class PersianClockNotationTests
{
    [Theory]
    [InlineData(0, 59, "دوازده و پنجاه و نه شب")]
    [InlineData(1, 0, "یک بامداد")]
    [InlineData(5, 59, "پنج و پنجاه و نه بامداد")]
    [InlineData(6, 0, "شش صبح")]
    [InlineData(11, 59, "یازده و پنجاه و نه صبح")]
    [InlineData(12, 0, "دوازده بعدازظهر")]
    [InlineData(20, 59, "هشت و پنجاه و نه بعدازظهر")]
    [InlineData(21, 0, "نه شب")]
    public void ToClockNotation_UsesAuthoredDayPeriodsAtBoundaries(int hour, int minute, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());
    }
}
#endif