namespace it;

[UseCulture("it")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "1 anno")]
    [InlineData(366, "un anno", true)]
    [InlineData(731, "2 anni")]
    [InlineData(1096, "3 anni")]
    [InlineData(4018, "11 anni")]
    public void Years(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: toWords));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "1 mese")]
    [InlineData(31, "un mese", true)]
    [InlineData(61, "2 mesi")]
    [InlineData(92, "3 mesi")]
    [InlineData(335, "11 mesi")]
    public void Months(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: toWords));

    [Theory]
    [InlineData(7, "1 settimana")]
    [InlineData(7, "una settimana", true)]
    [InlineData(14, "2 settimane")]
    public void Weeks(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "1 giorno")]
    [InlineData(1, "un giorno", true)]
    [InlineData(2, "2 giorni")]
    public void Days(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "1 ora")]
    [InlineData(1, "una ora", true)]
    [InlineData(2, "2 ore")]
    public void Hours(int hours, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "1 minuto")]
    [InlineData(1, "un minuto", true)]
    [InlineData(2, "2 minuti")]
    public void Minutes(int minutes, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "1 secondo")]
    [InlineData(1, "un secondo", true)]
    [InlineData(2, "2 secondi")]
    public void Seconds(int seconds, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "1 millisecondo")]
    [InlineData(1, "un millisecondo", true)]
    [InlineData(2, "2 millisecondi")]
    public void Milliseconds(int milliseconds, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(toWords: toWords));

    [Fact]
    public void NoTime() =>
        Assert.Equal("0 millisecondi", TimeSpan.Zero.Humanize());

    [Fact]
    public void NoTimeToWords() =>
        // This does not make much sense in italian, anyway
        Assert.Equal("0 secondi", TimeSpan.Zero.Humanize(toWords: true));
}