namespace lb;

[UseCulture("lb-LU")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(366, "1 Joer")]
    [InlineData(731, "2 Joer")]
    [InlineData(1096, "3 Joer")]
    [InlineData(4018, "11 Joer")]
    public void Years(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(366, "ee Joer")]
    [InlineData(731, "zwee Joer")]
    [InlineData(1096, "dräi Joer")]
    [InlineData(4018, "eelef Joer")]
    public void YearsToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: true));

    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(31, "1 Mount")]
    [InlineData(61, "2 Méint")]
    [InlineData(92, "3 Méint")]
    [InlineData(335, "11 Méint")]
    public void Months(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(31, "ee Mount")]
    [InlineData(61, "zwee Méint")]
    [InlineData(92, "dräi Méint")]
    [InlineData(335, "eelef Méint")]
    public void MonthsToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: true));

    [Theory]
    [InlineData(7, "1 Woch")]
    [InlineData(14, "2 Wochen")]
    [InlineData(21, "3 Wochen")]
    [InlineData(77, "11 Wochen")]
    public void Weeks(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(7, "eng Woch")]
    [InlineData(14, "zwou Wochen")]
    [InlineData(21, "dräi Wochen")]
    [InlineData(77, "eelef Wochen")]
    public void WeeksToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 Dag")]
    [InlineData(2, "2 Deeg")]
    public void Days(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(1, "een Dag")]
    [InlineData(2, "zwee Deeg")]
    public void DaysToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 Stonn")]
    [InlineData(2, "2 Stonnen")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());

    [Theory]
    [InlineData(1, "eng Stonn")]
    [InlineData(2, "zwou Stonnen")]
    public void HoursToWords(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 Minutt")]
    [InlineData(2, "2 Minutten")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());

    [Theory]
    [InlineData(1, "eng Minutt")]
    [InlineData(2, "zwou Minutten")]
    public void MinutesToWords(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 Sekonn")]
    [InlineData(2, "2 Sekonnen")]
    public void Seconds(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());

    [Theory]
    [InlineData(1, "eng Sekonn")]
    [InlineData(2, "zwou Sekonnen")]
    public void SecondsToWords(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 Millisekonn")]
    [InlineData(2, "2 Millisekonnen")]
    public void Milliseconds(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());

    [Theory]
    [InlineData(1, "eng Millisekonn")]
    [InlineData(2, "zwou Millisekonnen")]
    public void MillisecondsToWords(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(toWords: true));

    [Theory]
    [InlineData(TimeUnit.Year, "0 Joer")]
    [InlineData(TimeUnit.Month, "0 Méint")]
    [InlineData(TimeUnit.Week, "0 Wochen")]
    [InlineData(TimeUnit.Day, "0 Deeg")]
    [InlineData(TimeUnit.Hour, "0 Stonnen")]
    [InlineData(TimeUnit.Minute, "0 Minutten")]
    [InlineData(TimeUnit.Second, "0 Sekonnen")]
    [InlineData(TimeUnit.Millisecond, "0 Millisekonnen")]
    public void NoTime(TimeUnit minUnit, string expected)
    {
        var noTime = TimeSpan.Zero;
        var actual = noTime.Humanize(minUnit: minUnit);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NoTimeToWords() =>
        // This one doesn't make a lot of sense but ... w/e
        Assert.Equal("Keng Zäit", TimeSpan.Zero.Humanize(toWords: true));
}
