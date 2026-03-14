namespace bg;

public class ResourcesTests
{
    [Fact]
    public void HasExplicitResidualResources()
    {
        var never = Resources.GetResource("DateHumanize_Never", new("bg-BG"));
        Assert.Equal("никога", never);

        var twoDaysAgo = Resources.GetResource("DateHumanize_TwoDaysAgo", new("bg-BG"));
        Assert.Equal("преди 2 дни", twoDaysAgo);

        var twoDaysFromNow = Resources.GetResource("DateHumanize_TwoDaysFromNow", new("bg-BG"));
        Assert.Equal("след 2 дни", twoDaysFromNow);

        var daysAgoPaucal = Resources.GetResource("DateHumanize_MultipleDaysAgo_Paucal", new("bg-BG"));
        Assert.Equal("преди {0} дни", daysAgoPaucal);

        var daysFromNowPaucal = Resources.GetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("bg-BG"));
        Assert.Equal("след {0} дни", daysFromNowPaucal);

        var ageFormat = Resources.GetResource("TimeSpanHumanize_Age", new("bg-BG"));
        Assert.Equal("{0}", ageFormat);
    }
}
