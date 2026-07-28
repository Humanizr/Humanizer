namespace Humanizer.Tests.Localisation.lt;

#if NET6_0_OR_GREATER
[UseCulture("lt")]
public class LithuanianClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "dvi valandų dvi minutės")]
    [InlineData(22, 22, "dvidešimt dvi valandų dvidešimt dvi minutės")]
    public void ToClockNotation_UsesFeminineMinuteNumbers(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif