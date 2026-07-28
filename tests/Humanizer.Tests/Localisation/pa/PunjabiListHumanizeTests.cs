namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiListHumanizeTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesAte()
    {
        Assert.Equal("1 ਅਤੇ 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndAte()
    {
        Assert.Equal("1, 2 ਅਤੇ 3", Triple.Humanize());
    }
}