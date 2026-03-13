namespace hr;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("hr-HR"));
        Assert.Equal("nikada", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("hr-HR"));
        Assert.Equal("prije 2 dana", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("hr-HR"));
        Assert.Equal("za 2 dana", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("hr-HR"));
        Assert.Equal("{0}", ageFormat);
    }
}
