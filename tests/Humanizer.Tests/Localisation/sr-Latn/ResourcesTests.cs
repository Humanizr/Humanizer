namespace srLatn;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.Equal("nikada", Resources.GetResource("DateHumanize_Never", new("sr-Latn")));
        Assert.Equal("pre 2 dana", Resources.GetResource("DateHumanize_TwoDaysAgo", new("sr-Latn")));
        Assert.Equal("za 2 dana", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("sr-Latn")));
        Assert.Equal("{0}", Resources.GetResource("TimeSpanHumanize_Age", new("sr-Latn")));
    }
}
