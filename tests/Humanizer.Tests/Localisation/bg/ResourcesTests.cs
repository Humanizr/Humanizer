namespace bg;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_Never", new("bg"), out var never));
        Assert.Equal("никога", never);

        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysAgo", new("bg"), out var twoDaysAgo));
        Assert.Equal("преди 2 дена", twoDaysAgo);

        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysFromNow", new("bg"), out var twoDaysFromNow));
        Assert.Equal("след 2 дена", twoDaysFromNow);

        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysAgo_Paucal", new("bg"), out var daysAgoPaucal));
        Assert.Equal("преди {0} дена", daysAgoPaucal);

        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("bg"), out var daysFromNowPaucal));
        Assert.Equal("след {0} дена", daysFromNowPaucal);

        Assert.True(Resources.TryGetResource("TimeSpanHumanize_Age", new("bg"), out var ageFormat));
        Assert.Equal("{0}", ageFormat);
    }
}
