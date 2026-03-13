namespace el;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("el"));
        Assert.Equal("ποτέ", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("el"));
        Assert.Equal("προχθές", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("el"));
        Assert.Equal("μεθαύριο", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("el"));
        Assert.Equal("{0}", ageFormat);
    }
}
