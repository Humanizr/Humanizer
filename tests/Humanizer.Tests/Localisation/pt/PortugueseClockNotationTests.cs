namespace Humanizer.Tests.Localisation.pt;

#if NET6_0_OR_GREATER
[UseCulture("pt-PT")]
public class PortugueseClockNotationTests
{
    [Theory]
    [InlineData(5, 1, "cinco e um")]
    [InlineData(1, 2, "uma e dois")]
    [InlineData(10, 21, "dez e vinte e um")]
    [InlineData(11, 22, "onze e vinte e dois")]
    public void ToClockNotation_UsesMasculineMinuteValues(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif