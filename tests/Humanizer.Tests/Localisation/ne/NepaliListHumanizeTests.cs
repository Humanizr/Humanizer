namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliListHumanizeTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesRa()
    {
        Assert.Equal("1 र 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndRa()
    {
        Assert.Equal("1, 2 र 3", Triple.Humanize());
    }
}