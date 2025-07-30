namespace bg;

[UseCulture("bg-BG")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "1 година")]
    [InlineData(731, "2 години")]
    [InlineData(1096, "3 години")]
    [InlineData(4018, "11 години")]
    public void Years(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "една година")]
    [InlineData(731, "две години")]
    [InlineData(1096, "три години")]
    [InlineData(4018, "единадесет години")]
    // [InlineData(7671, "двадесет и една година")]
    public void YearsToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: true));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "1 месец")]
    [InlineData(61, "2 месеца")]
    [InlineData(92, "3 месеца")]
    [InlineData(335, "11 месеца")]
    public void Months(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "един месец")]
    [InlineData(61, "два месеца")]
    [InlineData(92, "три месеца")]
    [InlineData(335, "единадесет месеца")]
    public void MonthsToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Month, toWords: true));

    [Theory]
    [InlineData(7, "1 седмица")]
    [InlineData(14, "2 седмици")]
    public void Weeks(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(7, "една седмица")]
    [InlineData(14, "две седмици")]
    public void WeeksToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 ден")]
    [InlineData(2, "2 дена")]
    public void Days(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(1, "един ден")]
    [InlineData(2, "два дена")]
    public void DaysToWords(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 час")]
    [InlineData(2, "2 часа")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());

    [Theory]
    [InlineData(1, "един час")]
    [InlineData(2, "два часа")]
    public void HoursToWords(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 минута")]
    [InlineData(2, "2 минути")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());

    [Theory]
    [InlineData(1, "една минута")]
    [InlineData(2, "две минути")]
    public void MinutesToWords(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 секунда")]
    [InlineData(2, "2 секунди")]
    public void Seconds(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());

    [Theory]
    [InlineData(1, "една секунда")]
    [InlineData(2, "две секунди")]
    public void SecondsToWords(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: true));

    [Theory]
    [InlineData(1, "1 милисекунда")]
    [InlineData(2, "2 милисекунди")]
    public void Milliseconds(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());

    [Theory]
    [InlineData(1, "една милисекунда")]
    [InlineData(2, "две милисекунди")]
    public void MillisecondsToWords(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(toWords: true));

    [Fact]
    public void NoTime() =>
        // This one doesn't make a lot of sense but ... w/e
        Assert.Equal("0 милисекунди", TimeSpan.Zero.Humanize());

    [Fact]
    public void NoTimeToWords() =>
        Assert.Equal("няма време", TimeSpan.Zero.Humanize(toWords: true));
}