namespace koKR;

[UseCulture("ko-KR")]
public class DateHumanizeTests
{
    [Fact]
    public void TwoDaysAgoHasExplicitKoreanResource() =>
        Assert.Equal("2일 전", Resources.GetResource("DateHumanize_TwoDaysAgo", new("ko-KR")));

    [Fact]
    public void TwoDaysFromNowHasExplicitKoreanResource() =>
        Assert.Equal("2일 후", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("ko-KR")));

    [Fact]
    public void DayPaucalResourcesExist()
    {
        Assert.Equal("{0}일 전", Resources.GetResource("DateHumanize_MultipleDaysAgo_Paucal", new("ko-KR")));
        Assert.Equal("{0}일 후", Resources.GetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("ko-KR")));
    }
}
