namespace lt;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.Equal("niekada", Resources.GetResource("DateHumanize_Never", new("lt")));
        Assert.Equal("prieš 2 dienas", Resources.GetResource("DateHumanize_TwoDaysAgo", new("lt")));
        Assert.Equal("po 2 dienų", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("lt")));
        Assert.Equal("{0}", Resources.GetResource("TimeSpanHumanize_Age", new("lt")));
    }
}
