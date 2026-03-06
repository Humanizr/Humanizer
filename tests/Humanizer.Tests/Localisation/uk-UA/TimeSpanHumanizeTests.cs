namespace ukUA;

[UseCulture("uk-UA")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "один рік", true)]
    [InlineData(366, "1 рік")]
    [InlineData(731, "2 роки")]
    [InlineData(1096, "3 роки")]
    [InlineData(4018, "11 років")]
    public void Years(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: toWords));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "один місяць", true)]
    [InlineData(31, "1 місяць")]
    [InlineData(61, "2 місяці")]
    [InlineData(92, "3 місяці")]
    [InlineData(335, "11 місяців")]
    public void Months(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year, toWords: toWords));

    [Theory]
    [InlineData(7, "один тиждень", true)]
    [InlineData(7, "1 тиждень")]
    [InlineData(14, "2 тижні")]
    [InlineData(21, "3 тижні")]
    [InlineData(28, "4 тижні")]
    [InlineData(35, "5 тижнів")]
    [InlineData(77, "11 тижнів")]
    [InlineData(147, "двадцять один тиждень", true)]
    public void Weeks(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "один день", true)]
    [InlineData(1, "1 день")]
    [InlineData(2, "2 дні")]
    [InlineData(3, "3 дні")]
    [InlineData(4, "4 дні")]
    [InlineData(5, "5 днів")]
    [InlineData(6, "6 днів")]
    [InlineData(21, "двадцять один день", true)]
    public void Days(int days, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(toWords: toWords, maxUnit: TimeUnit.Day));

    [Theory]
    [InlineData(1, "одна година", true)]
    [InlineData(1, "1 година")]
    [InlineData(2, "2 години")]
    [InlineData(3, "3 години")]
    [InlineData(4, "4 години")]
    [InlineData(5, "5 годин")]
    [InlineData(6, "6 годин")]
    [InlineData(10, "10 годин")]
    [InlineData(11, "11 годин")]
    [InlineData(19, "19 годин")]
    [InlineData(20, "20 годин")]
    [InlineData(21, "21 година")]
    [InlineData(21, "двадцять одна година", true)]
    [InlineData(22, "22 години")]
    [InlineData(23, "23 години")]
    public void Hours(int hours, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "одна хвилина", true)]
    [InlineData(1, "1 хвилина")]
    [InlineData(2, "2 хвилини")]
    [InlineData(3, "3 хвилини")]
    [InlineData(4, "4 хвилини")]
    [InlineData(5, "5 хвилин")]
    [InlineData(6, "6 хвилин")]
    [InlineData(10, "10 хвилин")]
    [InlineData(11, "11 хвилин")]
    [InlineData(19, "19 хвилин")]
    [InlineData(20, "20 хвилин")]
    [InlineData(21, "21 хвилина")]
    [InlineData(21, "двадцять одна хвилина", true)]
    [InlineData(22, "22 хвилини")]
    [InlineData(23, "23 хвилини")]
    [InlineData(24, "24 хвилини")]
    [InlineData(25, "25 хвилин")]
    [InlineData(40, "40 хвилин")]
    public void Minutes(int minutes, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "одна секунда", true)]
    [InlineData(1, "1 секунда")]
    [InlineData(2, "2 секунди")]
    [InlineData(3, "3 секунди")]
    [InlineData(4, "4 секунди")]
    [InlineData(5, "5 секунд")]
    [InlineData(6, "6 секунд")]
    [InlineData(10, "10 секунд")]
    [InlineData(11, "11 секунд")]
    [InlineData(19, "19 секунд")]
    [InlineData(20, "20 секунд")]
    [InlineData(21, "21 секунда")]
    [InlineData(21, "двадцять одна секунда", true)]
    [InlineData(22, "22 секунди")]
    [InlineData(23, "23 секунди")]
    [InlineData(24, "24 секунди")]
    [InlineData(25, "25 секунд")]
    [InlineData(40, "40 секунд")]
    public void Seconds(int seconds, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(toWords: toWords));

    [Theory]
    [InlineData(1, "одна мілісекунда", true)]
    [InlineData(1, "1 мілісекунда")]
    [InlineData(2, "2 мілісекунди")]
    [InlineData(3, "3 мілісекунди")]
    [InlineData(4, "4 мілісекунди")]
    [InlineData(5, "5 мілісекунд")]
    [InlineData(6, "6 мілісекунд")]
    [InlineData(10, "10 мілісекунд")]
    [InlineData(11, "11 мілісекунд")]
    [InlineData(19, "19 мілісекунд")]
    [InlineData(20, "20 мілісекунд")]
    [InlineData(21, "21 мілісекунда")]
    [InlineData(21, "двадцять одна мілісекунда", true)]
    [InlineData(22, "22 мілісекунди")]
    [InlineData(23, "23 мілісекунди")]
    [InlineData(24, "24 мілісекунди")]
    [InlineData(25, "25 мілісекунд")]
    [InlineData(40, "40 мілісекунд")]
    public void Milliseconds(int milliseconds, string expected, bool toWords = false) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize(toWords: toWords));

    [Fact]
    public void NoTime() =>
        Assert.Equal("0 мілісекунд", TimeSpan.Zero.Humanize());

    [Fact]
    public void NoTimeToWords() =>
        Assert.Equal("без часу", TimeSpan.Zero.Humanize(toWords: true));
}