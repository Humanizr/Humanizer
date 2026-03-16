namespace nl;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("nl-NL"));
        Assert.Equal("nooit", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("nl-NL"));
        Assert.Equal("eergisteren", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("nl-NL"));
        Assert.Equal("overmorgen", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("nl-NL"));
        Assert.Equal("{0} oud", ageFormat);
    }
}
