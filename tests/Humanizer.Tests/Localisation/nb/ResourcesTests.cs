namespace nb;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("nb"));
        Assert.Equal("aldri", never);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("nb"));
        Assert.Equal("{0} gammel", ageFormat);
    }
}
