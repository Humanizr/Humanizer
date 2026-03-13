namespace msMY;

[UseCulture("ms-MY")]
public class DateHumanizeTests
{
    [Fact]
    public void TwoDaysAgoHasExplicitMalayResource()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysAgo", new("ms"), out var value));
        Assert.Equal("kelmarin", value);
    }

    [Fact]
    public void TwoDaysFromNowHasExplicitMalayResource()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysFromNow", new("ms"), out var value));
        Assert.Equal("lusa", value);
    }

    [Fact]
    public void MalayHasExplicitPaucalDayResources()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysAgo_Paucal", new("ms"), out var past));
        Assert.Equal("{0} hari yang lalu", past);

        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("ms"), out var future));
        Assert.Equal("{0} hari dari sekarang", future);
    }
}
