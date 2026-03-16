namespace pl;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("pl-PL"));
        Assert.Equal("nigdy", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("pl-PL"));
        Assert.Equal("przed 2 dniami", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("pl-PL"));
        Assert.Equal("za 2 dni", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("pl-PL"));
        Assert.Equal("{0}", ageFormat);
    }
}
