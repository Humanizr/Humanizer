namespace Humanizer.Tests.Localisation.bg;

#if NET6_0_OR_GREATER
[UseCulture("bg")]
public class BulgarianClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "два часа и две минути")]
    [InlineData(22, 22, "двадесет и два часа и двадесет и две минути")]
    public void ToClockNotation_UsesFeminineMinuteNumbers(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif