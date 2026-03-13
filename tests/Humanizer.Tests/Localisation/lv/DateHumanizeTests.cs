namespace lv;

[UseCulture("lv")]
public class DateHumanizeTests
{
    [Fact]
    public void TwoDaysAgoHasExplicitLatvianResource() =>
        Assert.Equal("pirms 2 dienām", Resources.GetResource("DateHumanize_TwoDaysAgo", new("lv")));

    [Fact]
    public void TwoDaysFromNowHasExplicitLatvianResource() =>
        Assert.Equal("pēc 2 dienām", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("lv")));

    [Fact]
    public void DayPaucalResourcesExist()
    {
        Assert.Equal("pirms {0} dienām", Resources.GetResource("DateHumanize_MultipleDaysAgo_Paucal", new("lv")));
        Assert.Equal("pēc {0} dienām", Resources.GetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("lv")));
    }
}
