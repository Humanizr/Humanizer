namespace Humanizer.Tests.Localisation.ur;

[UseCulture("ur")]
public class UrduDurationTests
{
    [Theory]
    [InlineData(1, "1 ملی سیکنڈ")]
    [InlineData(2, "2 ملی سیکنڈ")]
    public void Milliseconds(int ms, string expected)
    {
        var result = TimeSpan.FromMilliseconds(ms).Humanize(culture: new CultureInfo("ur"));
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, "1 سیکنڈ")]
    [InlineData(2, "2 سیکنڈ")]
    public void Seconds(int s, string expected)
    {
        var result = TimeSpan.FromSeconds(s).Humanize(culture: new CultureInfo("ur"));
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, "1 منٹ")]
    [InlineData(2, "2 منٹ")]
    public void Minutes(int m, string expected)
    {
        var result = TimeSpan.FromMinutes(m).Humanize(culture: new CultureInfo("ur"));
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, "1 گھنٹہ")]
    [InlineData(2, "2 گھنٹے")]
    public void Hours(int h, string expected)
    {
        var result = TimeSpan.FromHours(h).Humanize(culture: new CultureInfo("ur"));
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(1, "1 دن")]
    [InlineData(2, "2 دن")]
    public void Days(int d, string expected)
    {
        var result = TimeSpan.FromDays(d).Humanize(culture: new CultureInfo("ur"));
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Theory]
    [InlineData(7, "1 ہفتہ")]
    [InlineData(14, "2 ہفتے")]
    public void Weeks(int days, string expected)
    {
        var result = TimeSpan.FromDays(days).Humanize(culture: new CultureInfo("ur"));
        Assert.Equal(expected, result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void MultiPart_ThreeUnits()
    {
        var result = TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: new CultureInfo("ur"), collectionSeparator: null);
        Assert.Equal("2 ہفتے, 1 دن اور 1 گھنٹہ", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }

    [Fact]
    public void Zero_ReturnsZeroMilliseconds()
    {
        var result = TimeSpan.Zero.Humanize(culture: new CultureInfo("ur"));
        Assert.Equal("0 ملی سیکنڈ", result);
        UrduBidiControlSweep.AssertNoBidiControls(result);
    }
}