namespace Humanizer.Tests.Localisation.ru;

#if NET6_0_OR_GREATER
[UseCulture("ru")]
public class RussianClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "два две")]
    [InlineData(22, 22, "десять двадцать две")]
    public void ToClockNotation_UsesFeminineMinuteNumbers(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif