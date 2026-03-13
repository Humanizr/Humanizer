namespace it;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("it"));
        Assert.Equal("mai", never);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("it"));
        Assert.Equal("{0} vecchio", ageFormat);
    }
}
