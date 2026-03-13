namespace af;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("af"));
        Assert.Equal("nooit", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("af"));
        Assert.Equal("2 dae gelede", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("af"));
        Assert.Equal("oor 2 dae", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("af"));
        Assert.Equal("{0} oud", ageFormat);
    }
}
