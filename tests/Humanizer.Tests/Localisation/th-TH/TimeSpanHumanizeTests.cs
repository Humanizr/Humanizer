namespace thTH;

[UseCulture("th-TH")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [InlineData(14, "2 สัปดาห์")]
    [InlineData(366, "1 ปี")]
    public void Age(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).ToAge());

    [Fact]
    public void AgeHasExplicitThaiResource() =>
        Assert.Equal("{0}", Resources.GetResource("TimeSpanHumanize_Age", new("th-TH")));
}
