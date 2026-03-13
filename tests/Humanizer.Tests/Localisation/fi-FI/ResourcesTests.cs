namespace fiFI;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("fi-FI"));
        Assert.Equal("ei koskaan", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("fi-FI"));
        Assert.Equal("toissapäivänä", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("fi-FI"));
        Assert.Equal("ylihuomenna", twoDaysFromNow);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("fi-FI"));
        Assert.Equal("{0} vanha", ageFormat);
    }
}
