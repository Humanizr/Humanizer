namespace Humanizer.Tests.Localisation.vi;

[UseCulture("vi")]
public class VietnameseLocaleParityTests
{
    static readonly CultureInfo Vietnamese = new("vi");

    [Theory]
    [InlineData(TimeUnit.Millisecond, "một mili giây")]
    [InlineData(TimeUnit.Second, "một giây")]
    [InlineData(TimeUnit.Minute, "một phút")]
    [InlineData(TimeUnit.Hour, "một giờ")]
    [InlineData(TimeUnit.Day, "một ngày")]
    [InlineData(TimeUnit.Week, "một tuần")]
    [InlineData(TimeUnit.Month, "một tháng")]
    [InlineData(TimeUnit.Year, "một năm")]
    public void TimeSpanHumanize_UsesVietnameseSingularWords(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Vietnamese);

        Assert.Equal(expected, formatter.TimeSpanHumanize(unit, 1, toWords: true));
    }

    [Fact]
    public void WordsToNumber_ParsesVietnameseOrdinal() =>
        Assert.Equal(2, "thứ nhì".ToNumber(Vietnamese));
}

#if NET6_0_OR_GREATER
[UseCulture("vi")]
public class VietnameseClockNotationTests
{
    [Theory]
    [InlineData(0, "mười hai giờ tối")]
    [InlineData(1, "một giờ sáng")]
    [InlineData(5, "năm giờ sáng")]
    [InlineData(6, "sáu giờ sáng")]
    [InlineData(11, "mười một giờ sáng")]
    [InlineData(12, "mười hai giờ chiều")]
    [InlineData(20, "tám giờ chiều")]
    [InlineData(21, "chín giờ tối")]
    public void ToClockNotation_UsesDayPeriodBoundaries(int hour, string expected) =>
        Assert.Equal(expected, new TimeOnly(hour, 0).ToClockNotation());
}
#endif