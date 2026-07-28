namespace Humanizer.Tests.Localisation.hr;

#if NET6_0_OR_GREATER
[UseCulture("hr")]
public class CroatianClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "dva sata i dva minute")]
    [InlineData(3, 3, "tri sata i tri minute")]
    [InlineData(4, 4, "četiri sata i četiri minute")]
    [InlineData(22, 22, "dvadeset dva sata i dvadeset dva minute")]
    public void ToClockNotation_UsesPaucalMinuteSuffix(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif