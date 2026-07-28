namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduListHumanizeTests
{
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void TwoElements_UsesAur()
    {
        var result = Pair.Humanize();
        Assert.Equal("1 اور 2", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void ThreeElements_UsesCommaAndAur()
    {
        var result = Triple.Humanize();
        Assert.Equal("1, 2 اور 3", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void UrPk_InheritsListFormat()
    {
        using var _ = new CultureSwap(new CultureInfo("ur-PK"));
        var result = Pair.Humanize();
        Assert.Equal("1 اور 2", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void UrIn_InheritsListFormat()
    {
        using var _ = new CultureSwap(new CultureInfo("ur-IN"));
        var result = Pair.Humanize();
        Assert.Equal("1 اور 2", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}