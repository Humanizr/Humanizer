namespace Humanizer.Tests.Localisation.sw;

#if NET6_0_OR_GREATER
[UseCulture("sw")]
public class SwahiliClockNotationTests
{
    [Theory]
    [InlineData(1, 0, "saa saba asubuhi")]
    [InlineData(1, 5, "saa saba na dakika tano asubuhi")]
    [InlineData(13, 23, "saa saba na dakika ishirini na tatu mchana")]
    [InlineData(20, 0, "saa mbili jioni")]
    [InlineData(23, 40, "saa tano na dakika arobaini usiku")]
    public void ToClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("saa saba na dakika ishirini na tano mchana", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}
#endif