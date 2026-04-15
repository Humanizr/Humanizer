namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduListHumanizeTests
{
    [Fact]
    public void TwoElements_UsesAur()
    {
        var result = new[] { 1, 2 }.Humanize();
        Assert.Equal("1 اور 2", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void ThreeElements_UsesCommaAndAur()
    {
        var result = new[] { 1, 2, 3 }.Humanize();
        Assert.Equal("1, 2 اور 3", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void UrPk_InheritsListFormat()
    {
        using var _ = new CultureSwap(new CultureInfo("ur-PK"));
        var result = new[] { 1, 2 }.Humanize();
        Assert.Equal("1 اور 2", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void UrIn_InheritsListFormat()
    {
        using var _ = new CultureSwap(new CultureInfo("ur-IN"));
        var result = new[] { 1, 2 }.Humanize();
        Assert.Equal("1 اور 2", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}