namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiDurationTests
{
    [Theory]
    [InlineData(1, "1 मिलीसेकंड")]
    [InlineData(2, "2 मिलीसेकंड")]
    public void Milliseconds(int ms, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromMilliseconds(ms).Humanize(culture: new CultureInfo("hi")));
    }

    [Theory]
    [InlineData(1, "1 सेकंड")]
    [InlineData(2, "2 सेकंड")]
    public void Seconds(int s, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromSeconds(s).Humanize(culture: new CultureInfo("hi")));
    }

    [Theory]
    [InlineData(1, "1 मिनट")]
    [InlineData(2, "2 मिनट")]
    public void Minutes(int m, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromMinutes(m).Humanize(culture: new CultureInfo("hi")));
    }

    [Theory]
    [InlineData(1, "1 घंटा")]
    [InlineData(2, "2 घंटे")]
    public void Hours(int h, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromHours(h).Humanize(culture: new CultureInfo("hi")));
    }

    [Theory]
    [InlineData(1, "1 दिन")]
    [InlineData(2, "2 दिन")]
    public void Days(int d, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(d).Humanize(culture: new CultureInfo("hi")));
    }

    [Theory]
    [InlineData(7, "1 सप्ताह")]
    [InlineData(14, "2 सप्ताह")]
    public void Weeks(int days, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(culture: new CultureInfo("hi")));
    }

    [Fact]
    public void MultiPart_ThreeUnits()
    {
        var result = TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: new CultureInfo("hi"), collectionSeparator: null);
        Assert.Equal("2 सप्ताह, 1 दिन और 1 घंटा", result);
    }

    [Fact]
    public void Zero_ReturnsZeroMilliseconds()
    {
        Assert.Equal("0 मिलीसेकंड", TimeSpan.Zero.Humanize(culture: new CultureInfo("hi")));
    }

    [Fact]
    public void ToWords_UsesHindiWords()
    {
        Assert.Equal("एक घंटा", TimeSpan.FromHours(1).Humanize(toWords: true, culture: new CultureInfo("hi")));
    }
}