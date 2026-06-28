namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliDurationTests
{
    [Theory]
    [InlineData(1, "1 मिलिसेकेन्ड")]
    [InlineData(2, "2 मिलिसेकेन्ड")]
    public void Milliseconds(int ms, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromMilliseconds(ms).Humanize(culture: new CultureInfo("ne")));
    }

    [Theory]
    [InlineData(1, "1 सेकेन्ड")]
    [InlineData(2, "2 सेकेन्ड")]
    public void Seconds(int s, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromSeconds(s).Humanize(culture: new CultureInfo("ne")));
    }

    [Theory]
    [InlineData(1, "1 मिनेट")]
    [InlineData(2, "2 मिनेट")]
    public void Minutes(int m, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromMinutes(m).Humanize(culture: new CultureInfo("ne")));
    }

    [Theory]
    [InlineData(1, "1 घण्टा")]
    [InlineData(2, "2 घण्टा")]
    public void Hours(int h, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromHours(h).Humanize(culture: new CultureInfo("ne")));
    }

    [Theory]
    [InlineData(1, "1 दिन")]
    [InlineData(2, "2 दिन")]
    public void Days(int d, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(d).Humanize(culture: new CultureInfo("ne")));
    }

    [Theory]
    [InlineData(7, "1 हप्ता")]
    [InlineData(14, "2 हप्ता")]
    public void Weeks(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: new CultureInfo("ne")));
    }

    [Fact]
    public void MultiPart_ThreeUnits()
    {
        var result = TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: new CultureInfo("ne"), collectionSeparator: null);
        Assert.Equal("2 हप्ता, 1 दिन र 1 घण्टा", result);
    }

    [Fact]
    public void Zero_ReturnsZeroMilliseconds()
    {
        Assert.Equal("0 मिलिसेकेन्ड", TimeSpan.Zero.Humanize(culture: new CultureInfo("ne")));
    }

    [Fact]
    public void ToWords_UsesNepaliWords()
    {
        Assert.Equal("एक घण्टा", TimeSpan.FromHours(1).Humanize(toWords: true, culture: new CultureInfo("ne")));
    }
}