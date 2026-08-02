namespace Humanizer.Tests.Localisation.cs;

#if NET6_0_OR_GREATER
[UseCulture("cs")]
public class CzechClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "dvě hodiny dvě minuty")]
    [InlineData(3, 3, "tři hodiny tři minuty")]
    [InlineData(4, 4, "čtyři hodiny čtyři minuty")]
    [InlineData(22, 22, "dvacet dvě hodin dvacet dva minut")]
    public void ToClockNotation_UsesLowOnlyPaucalMinuteSuffix(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Theory]
    [InlineData(1, "pět hodin jedna minuta")]
    [InlineData(2, "pět hodin dvě minuty")]
    [InlineData(21, "pět hodin dvacet jedna minut")]
    [InlineData(22, "pět hodin dvacet dva minut")]
    [InlineData(31, "pět hodin třicet jedna minut")]
    [InlineData(41, "pět hodin čtyřicet jedna minut")]
    [InlineData(51, "pět hodin padesát jedna minut")]
    public void ToClockNotation_UsesMinuteFormsThatAgreeWithTheMinuteNoun(int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(5, minutes).ToClockNotation());
    }
}
#endif