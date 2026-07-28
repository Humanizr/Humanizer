namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboListHumanizeTests
{
    [Fact]
    public void TwoElements_UsesNa()
    {
        var pair = new[] { 1, 2 };
        Assert.Equal("1 na 2", pair.Humanize());
    }

    [Fact]
    public void ThreeElements_UsesCommaAndNa()
    {
        var triple = new[] { 1, 2, 3 };
        Assert.Equal("1, 2 na 3", triple.Humanize());
    }
}