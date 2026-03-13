namespace sr;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        Assert.Equal("никада", Resources.GetResource("DateHumanize_Never", new("sr")));
        Assert.Equal("пре 2 дана", Resources.GetResource("DateHumanize_TwoDaysAgo", new("sr")));
        Assert.Equal("за 2 дана", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("sr")));
        Assert.Equal("{0}", Resources.GetResource("TimeSpanHumanize_Age", new("sr")));
    }
}
