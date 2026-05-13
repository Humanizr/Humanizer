namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiDurationTests
{
    [Theory]
    [InlineData(1, "1 ਮਿਲੀਸਕਿੰਟ")]
    [InlineData(2, "2 ਮਿਲੀਸਕਿੰਟ")]
    public void Milliseconds(int ms, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromMilliseconds(ms).Humanize(culture: new CultureInfo("pa")));
    }

    [Theory]
    [InlineData(1, "1 ਸਕਿੰਟ")]
    [InlineData(2, "2 ਸਕਿੰਟ")]
    public void Seconds(int s, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromSeconds(s).Humanize(culture: new CultureInfo("pa")));
    }

    [Theory]
    [InlineData(1, "1 ਮਿੰਟ")]
    [InlineData(2, "2 ਮਿੰਟ")]
    public void Minutes(int m, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromMinutes(m).Humanize(culture: new CultureInfo("pa")));
    }

    [Theory]
    [InlineData(1, "1 ਘੰਟਾ")]
    [InlineData(2, "2 ਘੰਟੇ")]
    public void Hours(int h, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromHours(h).Humanize(culture: new CultureInfo("pa")));
    }

    [Theory]
    [InlineData(1, "1 ਦਿਨ")]
    [InlineData(2, "2 ਦਿਨ")]
    public void Days(int d, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromDays(d).Humanize(culture: new CultureInfo("pa")));
    }

    [Fact]
    public void MultiPart_ThreeUnits()
    {
        var result = TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: new CultureInfo("pa"), collectionSeparator: null);
        Assert.Equal("2 ਹਫ਼ਤੇ, 1 ਦਿਨ ਅਤੇ 1 ਘੰਟਾ", result);
    }

    [Fact]
    public void Zero_ReturnsZeroMilliseconds()
    {
        Assert.Equal("0 ਮਿਲੀਸਕਿੰਟ", TimeSpan.Zero.Humanize(culture: new CultureInfo("pa")));
    }

    [Fact]
    public void ToWords_UsesPunjabiWords()
    {
        Assert.Equal("ਇੱਕ ਘੰਟਾ", TimeSpan.FromHours(1).Humanize(toWords: true, culture: new CultureInfo("pa")));
    }
}