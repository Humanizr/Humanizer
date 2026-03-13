namespace hy;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("hy"));
        Assert.Equal("երբեք", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("hy"));
        Assert.Equal("2 օր առաջ", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("hy"));
        Assert.Equal("2 օրից", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("hy"));
        Assert.Equal("{0}", ageFormat);
    }
}
