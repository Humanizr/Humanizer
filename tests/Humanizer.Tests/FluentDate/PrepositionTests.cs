public class PrepositionTests
{
    [Fact]
    public void AtMidnight()
    {
        var now = DateTime.Now;
        var midnight = now.AtMidnight();
        Assert.Equal(new(now.Year, now.Month, now.Day), midnight);
    }

    [Fact]
    public void AtNoon()
    {
        var now = DateTime.Now;
        var noon = now.AtNoon();
        Assert.Equal(new(now.Year, now.Month, now.Day, 12, 0, 0), noon);
    }

    [Fact]
    public void InYear()
    {
        var now = DateTime.Now;
        var in2012 = now.In(2012);
        Assert.Equal(new(2012, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond), in2012);
    }
}