namespace hu;

[UseCulture("hu-HU")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "1 év")]
    [InlineData(731, "2 év")]
    [InlineData(1096, "3 év")]
    [InlineData(4018, "11 év")]
    public void Years(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "1 hónap")]
    [InlineData(61, "2 hónap")]
    [InlineData(92, "3 hónap")]
    [InlineData(335, "11 hónap")]
    public void Months(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [InlineData(14, "2 hét")]
    [InlineData(7, "1 hét")]
    public void Weeks(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(2, "2 nap")]
    [InlineData(1, "1 nap")]
    public void Days(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(2, "2 óra")]
    [InlineData(1, "1 óra")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());

    [Theory]
    [InlineData(2, "2 perc")]
    [InlineData(1, "1 perc")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());

    [Theory]
    [InlineData(2, "2 másodperc")]
    [InlineData(1, "1 másodperc")]
    public void Seconds(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "egy év")]
    [InlineData(731, "kettő év")]
    [InlineData(1096, "három év")]
    [InlineData(4018, "tizenegy év")]
    public void YearsWithWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: true));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "egy hónap")]
    [InlineData(61, "kettő hónap")]
    [InlineData(92, "három hónap")]
    [InlineData(335, "tizenegy hónap")]
    public void MonthsWithWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: true));

    [Theory]
    [InlineData(14, "kettő hét")]
    [InlineData(7, "egy hét")]
    public void WeeksWithWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));

    [Theory]
    [InlineData(2, "kettő nap")]
    [InlineData(1, "egy nap")]
    public void DaysWithWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));

    [Theory]
    [InlineData(2, "kettő óra")]
    [InlineData(1, "egy óra")]
    public void HoursWithWords(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: true));

    [Theory]
    [InlineData(2, "kettő perc")]
    [InlineData(1, "egy perc")]
    public void MinutesWithWords(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: true));

    [Theory]
    [InlineData(2, "kettő másodperc")]
    [InlineData(1, "egy másodperc")]
    public void SecondsWithWords(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: true));

    [Fact]
    public void NoTime()
    {
        var noTime = TimeSpan.Zero;
        var actual = noTime.Humanize();
        Assert.Equal("0 ezredmásodperc", actual);
    }

    [Fact]
    public void NoTimeToWords()
    {
        var noTime = TimeSpan.Zero;
        var actual = noTime.Humanize(toWords: true);
        Assert.Equal("nincs idő", actual);
    }
}