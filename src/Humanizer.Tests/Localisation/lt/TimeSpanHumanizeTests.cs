namespace lt;

[UseCulture("lt")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(366, "1 metai")]
    [InlineData(731, "2 metai")]
    [InlineData(1096, "3 metai")]
    [InlineData(4018, "11 metų")]
    public void Years(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Native speaker")]
    [InlineData(31, "1 mėnuo")]
    [InlineData(61, "2 mėnesiai")]
    [InlineData(280, "9 mėnesiai")]
    [InlineData(330, "10 mėnesių")]
    public void Months(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [InlineData(7, "1 savaitė")]
    [InlineData(14, "2 savaitės")]
    [InlineData(21, "3 savaitės")]
    [InlineData(77, "11 savaičių")]
    [InlineData(147, "21 savaitė")]
    public void Weeks(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(1, "1 diena")]
    [InlineData(2, "2 dienos")]
    [InlineData(9, "9 dienos")]
    [InlineData(10, "10 dienų")]
    [InlineData(17, "17 dienų")]
    [InlineData(21, "21 diena")]
    public void Days(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Day));

    [Theory]
    [InlineData(1, "1 valanda")]
    [InlineData(2, "2 valandos")]
    [InlineData(3, "3 valandos")]
    [InlineData(9, "9 valandos")]
    [InlineData(10, "10 valandų")]
    [InlineData(19, "19 valandų")]
    [InlineData(21, "21 valanda")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());

    [Theory]
    [InlineData(1, "1 minutė")]
    [InlineData(2, "2 minutės")]
    [InlineData(3, "3 minutės")]
    [InlineData(9, "9 minutės")]
    [InlineData(10, "10 minučių")]
    [InlineData(19, "19 minučių")]
    [InlineData(21, "21 minutė")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());

    [Theory]
    [InlineData(1, "1 sekundė")]
    [InlineData(2, "2 sekundės")]
    [InlineData(3, "3 sekundės")]
    [InlineData(9, "9 sekundės")]
    [InlineData(10, "10 sekundžių")]
    [InlineData(19, "19 sekundžių")]
    [InlineData(21, "21 sekundė")]
    public void Seconds(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());

    [Theory]
    [InlineData(1, "1 milisekundė")]
    [InlineData(2, "2 milisekundės")]
    [InlineData(3, "3 milisekundės")]
    [InlineData(9, "9 milisekundės")]
    [InlineData(10, "10 milisekundžių")]
    [InlineData(19, "19 milisekundžių")]
    [InlineData(21, "21 milisekundė")]
    public void Milliseconds(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());

    [Fact]
    public void NoTime() =>
        Assert.Equal("0 milisekundžių", TimeSpan.Zero.Humanize());

    [Fact]
    public void NoTimeToWords() =>
        Assert.Equal("nėra laiko", TimeSpan.Zero.Humanize(toWords: true));
}