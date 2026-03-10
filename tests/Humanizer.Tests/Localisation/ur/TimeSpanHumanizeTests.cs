namespace ur;

[UseCulture("ur")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [InlineData(366, "ایک سال")]
    [InlineData(731, "2 سال")]
    [InlineData(1096, "3 سال")]
    [InlineData(4018, "11 سال")]
    public void Years(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [InlineData(31, "ایک ماہ")]
    [InlineData(61, "2 ماہ")]
    [InlineData(92, "3 ماہ")]
    [InlineData(335, "11 ماہ")]
    public void Months(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [InlineData(7, "ایک ہفتہ")]
    [InlineData(14, "2 ہفتے")]
    [InlineData(21, "3 ہفتے")]
    [InlineData(77, "11 ہفتے")]
    public void Weeks(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(1, "ایک دن")]
    [InlineData(2, "2 دن")]
    [InlineData(3, "3 دن")]
    public void Days(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());

    [Theory]
    [InlineData(1, "ایک گھنٹہ")]
    [InlineData(2, "2 گھنٹے")]
    [InlineData(3, "3 گھنٹے")]
    [InlineData(11, "11 گھنٹے")]
    public void Hours(int hours, string expected) =>
        Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());

    [Theory]
    [InlineData(1, "ایک منٹ")]
    [InlineData(2, "2 منٹ")]
    [InlineData(3, "3 منٹ")]
    [InlineData(11, "11 منٹ")]
    public void Minutes(int minutes, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());

    [Theory]
    [InlineData(1, "ایک سیکنڈ")]
    [InlineData(2, "2 سیکنڈ")]
    [InlineData(3, "3 سیکنڈ")]
    [InlineData(11, "11 سیکنڈ")]
    public void Seconds(int seconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());

    [Theory]
    [InlineData(1, "ایک ملی سیکنڈ")]
    [InlineData(2, "2 ملی سیکنڈ")]
    [InlineData(3, "3 ملی سیکنڈ")]
    [InlineData(11, "11 ملی سیکنڈ")]
    public void Milliseconds(int milliseconds, string expected) =>
        Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());

    [Fact]
    public void NoTime() =>
        Assert.Equal("0 ملی سیکنڈ", TimeSpan.Zero.Humanize());

    [Fact]
    public void NoTimeToWords() =>
        Assert.Equal("کوئی وقت نہیں", TimeSpan.Zero.Humanize(toWords: true));
}
