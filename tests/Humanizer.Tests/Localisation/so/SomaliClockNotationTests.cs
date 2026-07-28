namespace Humanizer.Tests.Localisation.so;

#if NET6_0_OR_GREATER
[UseCulture("so")]
public class SomaliClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "kow saac subaxnimo")]
    [InlineData(1, 5, "kow iyo shan daqiiqo subaxnimo")]
    [InlineData(13, 23, "kow iyo saddex iyo labaatan daqiiqo galabnimo")]
    [InlineData(20, 0, "siddeed saac fiidnimo")]
    [InlineData(23, 40, "kow iyo toban iyo afartan daqiiqo habeennimo")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("kow iyo shan iyo labaatan daqiiqo galabnimo", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif