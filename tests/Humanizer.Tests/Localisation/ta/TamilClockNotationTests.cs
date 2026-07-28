namespace Humanizer.Tests.Localisation.ta;

#if NET6_0_OR_GREATER
[UseCulture("ta")]
public class TamilClockNotationTests
{
    [Theory]
    [InlineData(0, 59, "இரவு பனிரெண்டு மணி ஐம்பத்து ஒன்பது நிமிடம்")]
    [InlineData(1, 0, "அதிகாலை ஒரு மணி")]
    [InlineData(5, 59, "அதிகாலை ஐந்து மணி ஐம்பத்து ஒன்பது நிமிடம்")]
    [InlineData(6, 0, "காலை ஆறு மணி")]
    [InlineData(11, 59, "காலை பதினொன்று மணி ஐம்பத்து ஒன்பது நிமிடம்")]
    [InlineData(12, 0, "பிற்பகல் பனிரெண்டு மணி")]
    [InlineData(20, 59, "பிற்பகல் எட்டு மணி ஐம்பத்து ஒன்பது நிமிடம்")]
    [InlineData(21, 0, "இரவு ஒன்பது மணி")]
    public void ToClockNotation_UsesAuthoredDayPeriodsAtBoundaries(int hour, int minute, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hour, minute).ToClockNotation());
    }
}
#endif