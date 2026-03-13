namespace thTH;

[UseCulture("th-TH")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(1, "หนึ่งวินาทีที่แล้ว")]
    [InlineData(10, "10 วินาทีที่แล้ว")]
    [InlineData(59, "59 วินาทีที่แล้ว")]
    [InlineData(60, "หนึ่งนาทีที่แล้ว")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Fact]
    public void TwoDaysAgoHasExplicitThaiResource() =>
        Assert.Equal("เมื่อวานซืน", Resources.GetResource("DateHumanize_TwoDaysAgo", new("th-TH")));

    [Fact]
    public void TwoDaysFromNowHasExplicitThaiResource() =>
        Assert.Equal("มะรืนนี้", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("th-TH")));

    [Fact]
    public void DayPaucalResourcesExist()
    {
        Assert.Equal("{0} วันที่แล้ว", Resources.GetResource("DateHumanize_MultipleDaysAgo_Paucal", new("th-TH")));
        Assert.Equal("{0} วันจากนี้", Resources.GetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("th-TH")));
    }
}
