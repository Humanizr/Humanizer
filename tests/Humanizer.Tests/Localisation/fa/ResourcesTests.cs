namespace fa;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("fa"));
        Assert.Equal("هرگز", never);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("fa"));
        Assert.Equal("{0}", ageFormat);
    }
}
