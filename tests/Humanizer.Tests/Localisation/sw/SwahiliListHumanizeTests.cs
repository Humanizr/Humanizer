namespace Humanizer.Tests.Localisation.sw;

[UseCulture("sw")]
public class SwahiliListHumanizeTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesNa()
    {
        Assert.Equal("1 na 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndNa()
    {
        Assert.Equal("1, 2 na 3", Triple.Humanize());
    }
}