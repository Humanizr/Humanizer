namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliListHumanizeTests
{
    [Fact]
    public void TwoElements_UsesRa()
    {
        int[] pair = [1, 2];

        Assert.Equal("1 र 2", pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndRa()
    {
        int[] triple = [1, 2, 3];

        Assert.Equal("1, 2 र 3", triple.Humanize());
    }
}