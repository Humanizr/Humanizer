namespace Humanizer.Tests.Localisation.ha;

[UseCulture("ha")]
public class HausaListHumanizeTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesDa()
    {
        Assert.Equal("1 da 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndDa()
    {
        Assert.Equal("1, 2 da 3", Triple.Humanize());
    }
}