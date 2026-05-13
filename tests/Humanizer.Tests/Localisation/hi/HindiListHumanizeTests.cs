namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiListHumanizeTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesAur()
    {
        Assert.Equal("1 और 2", Pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndAur()
    {
        Assert.Equal("1, 2 और 3", Triple.Humanize());
    }
}