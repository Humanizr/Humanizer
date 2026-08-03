namespace Humanizer.Tests.Localisation.hr;

#if NET6_0_OR_GREATER
[UseCulture("hr")]
public class CroatianClockNotationTests
{
    static readonly CultureInfo HrHr = new("hr-HR");

    [Theory]
    [InlineData(5, 1, "pet sati i jedna minuta")]
    [InlineData(2, 2, "dva sata i dvije minute")]
    [InlineData(3, 3, "tri sata i tri minute")]
    [InlineData(4, 4, "četiri sata i četiri minute")]
    [InlineData(5, 21, "pet sati i dvadeset jedna minuta")]
    [InlineData(22, 22, "dvadeset dva sata i dvadeset dvije minute")]
    public void ToClockNotation_AgreesMinuteNumberAndSuffix(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ToClockNotation_RegionalCultureUsesCroatianProfile()
    {
        Assert.Equal(
            "pet sati i dvadeset dvije minute",
            new TimeOnly(5, 22).ToClockNotation(ClockNotationRounding.None, HrHr));
    }
}
#endif