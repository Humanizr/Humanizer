namespace sl;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.Equal("nikoli", Resources.GetResource("DateHumanize_Never", new("sl-SI")));
        Assert.Equal("pred 2 dnevoma", Resources.GetResource("DateHumanize_TwoDaysAgo", new("sl-SI")));
        Assert.Equal("čez 2 dni", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("sl-SI")));
        Assert.Equal("{0}", Resources.GetResource("TimeSpanHumanize_Age", new("sl-SI")));
    }
}
