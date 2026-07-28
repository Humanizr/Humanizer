namespace Humanizer.Tests.Localisation.cs;

#if NET6_0_OR_GREATER
[UseCulture("cs")]
public class CzechClockNotationTests
{
    [Theory]
    [InlineData(2, 2, "dvě hodiny dva minuty")]
    [InlineData(3, 3, "tři hodiny tři minuty")]
    [InlineData(4, 4, "čtyři hodiny čtyři minuty")]
    [InlineData(22, 22, "dvacet dvě hodin dvacet dva minut")]
    public void ToClockNotation_UsesLowOnlyPaucalMinuteSuffix(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }
}
#endif