namespace Humanizer.Tests.Localisation.uk;

#if NET6_0_OR_GREATER
[UseCulture("uk")]
public class UkrainianClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "друга дві")]
    [InlineData(22, 22, "десята двадцять дві")]
    public void ToClockNotation_UsesFeminineMinuteNumbers(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif