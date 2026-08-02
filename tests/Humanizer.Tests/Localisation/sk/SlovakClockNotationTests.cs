namespace Humanizer.Tests.Localisation.sk;

#if NET6_0_OR_GREATER
[UseCulture("sk")]
public class SlovakClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "dve hodiny dve minúty")]
    [InlineData(3, 3, "tri hodiny tri minúty")]
    [InlineData(4, 4, "štyri hodiny štyri minúty")]
    [InlineData(22, 22, "dvadsať dve hodín dvadsaťdva minút")]
    public void ToClockNotation_UsesLowOnlyPaucalMinuteSuffix(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Theory]
    [InlineData(1, "päť hodín jedna minúta")]
    [InlineData(2, "päť hodín dve minúty")]
    [InlineData(21, "päť hodín dvadsaťjeden minút")]
    [InlineData(22, "päť hodín dvadsaťdva minút")]
    public void ToClockNotation_UsesMinuteFormsThatAgreeWithTheMinuteNoun(int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(5, minutes).ToClockNotation());
    }
}
#endif