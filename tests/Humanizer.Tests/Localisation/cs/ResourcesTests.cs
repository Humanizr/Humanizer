namespace cs;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_Never", new("cs"), out var never));
        Assert.Equal("nikdy", never);

        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysAgo", new("cs"), out var twoDaysAgo));
        Assert.Equal("před 2 dny", twoDaysAgo);

        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysFromNow", new("cs"), out var twoDaysFromNow));
        Assert.Equal("za 2 dny", twoDaysFromNow);

        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysAgo_Paucal", new("cs"), out var daysAgoPaucal));
        Assert.Equal("před {0} dny", daysAgoPaucal);

        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("cs"), out var daysFromNowPaucal));
        Assert.Equal("za {0} dny", daysFromNowPaucal);

        Assert.True(Resources.TryGetResource("TimeSpanHumanize_Age", new("cs"), out var ageFormat));
        Assert.Equal("{0}", ageFormat);
    }
}
