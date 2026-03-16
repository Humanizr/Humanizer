namespace da;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysAgo", new("da"), out var twoDaysAgo));
        Assert.Equal("forgårs", twoDaysAgo);

        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysFromNow", new("da"), out var twoDaysFromNow));
        Assert.Equal("i overmorgen", twoDaysFromNow);

        Assert.True(Resources.TryGetResource("TimeSpanHumanize_Age", new("da"), out var ageFormat));
        Assert.Equal("{0} gammel", ageFormat);
    }
}
