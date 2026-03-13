namespace sk;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("sk-SK"));
        Assert.Equal("nikdy", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("sk-SK"));
        Assert.Equal("pred 2 dňami", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("sk-SK"));
        Assert.Equal("o 2 dni", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("sk-SK"));
        Assert.Equal("{0}", ageFormat);
    }
}
