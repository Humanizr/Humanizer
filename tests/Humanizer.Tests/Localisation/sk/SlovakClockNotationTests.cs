namespace Humanizer.Tests.Localisation.sk;

#if NET6_0_OR_GREATER
[UseCulture("sk")]
public class SlovakClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "dve hodiny dva minúty")]
    [InlineData(3, 3, "tri hodiny tri minúty")]
    [InlineData(4, 4, "štyri hodiny štyri minúty")]
    [InlineData(22, 22, "dvadsať dve hodín dvadsaťdva minút")]
    public void ToClockNotation_UsesLowOnlyPaucalMinuteSuffix(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif