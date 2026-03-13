namespace sv;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("sv-SE"));
        Assert.Equal("aldrig", never);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("sv-SE"));
        Assert.Equal("{0} gammal", ageFormat);
    }
}
