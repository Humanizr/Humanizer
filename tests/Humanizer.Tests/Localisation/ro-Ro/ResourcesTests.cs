namespace roRO;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("ro-RO"));
        Assert.Equal("niciodată", never);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("ro-RO"));
        Assert.Equal("{0}", ageFormat);
    }
}
