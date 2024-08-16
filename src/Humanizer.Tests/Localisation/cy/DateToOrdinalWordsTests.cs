namespace cy;

[UseCulture("cy")]
public class DateToOrdinalWordsTests
{
    [Fact]
    public void OrdinalizeString() =>
        Assert.Equal("1st January 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

#if NET6_0_OR_GREATER

    [Fact]
    public void OrdinalizeDateOnlyString() =>
        Assert.Equal("1st January 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

#endif


    [Fact]
    public void DateTime1SecondAgo() =>
        Assert.Equal("one second ago", DateTime.UtcNow.AddSeconds(-1).Humanize());

    [Fact]
    public void DateTimeMultipleSecondsAgo() =>
        Assert.Equal("3 seconds ago", DateTime.UtcNow.AddSeconds(-3).Humanize());

    [Fact]
    public void DateTime1MinuteAgo() =>
        Assert.Equal("a minute ago", DateTime.UtcNow.AddMinutes(-1).Humanize());

    [Fact]
    public void DateTimeMultipleMinutesAgo() =>
        Assert.Equal("3 minute ago", DateTime.UtcNow.AddMinutes(-3).Humanize());

    [Fact]
    public void DateTime1HourAgo() =>
        Assert.Equal("an hour ago", DateTime.UtcNow.AddMinutes(-1).Humanize());

    [Fact]
    public void DateTimeMultipleHourAgo() =>
        Assert.Equal("3 hours ago", DateTime.UtcNow.AddMinutes(-3).Humanize());

    [Fact]
    public void DateTime1DayAgo() =>
        Assert.Equal("yesterday", DateTime.UtcNow.AddDays(-1).Humanize());

    [Fact]
    public void DateTimeMultipleDaysAgo() =>
        Assert.Equal("3 days ago", DateTime.UtcNow.AddDays(-3).Humanize());

    [Fact]
    public void DateTime1MonthAgo() =>
        Assert.Equal("one month ago", DateTime.UtcNow.AddMonths(-1).Humanize());

    [Fact]
    public void DateTimeMultipleMonthsAgo() =>
        Assert.Equal("3 months ago", DateTime.UtcNow.AddMonths(-3).Humanize());


    [Fact]
    public void DateTime1YearAgo() =>
        Assert.Equal("one year ago", DateTime.UtcNow.AddYears(-1).Humanize());

    [Fact]
    public void TimeSpanDays() =>
        Assert.Equal("2 days", new TimeSpan(48,0,0).Humanize());

    [Fact]
    public void TimeSpanHours() =>
        Assert.Equal("2 hours", new TimeSpan(2, 0, 0).Humanize());

    [Fact]
    public void TimeSpanMilliseconds() =>
        Assert.Equal("100 milliseconds", new TimeSpan(0, 0, 0, 0, 100).Humanize());

    [Fact]
    public void TimeSpanMinutes() =>
        Assert.Equal("5 minutes", new TimeSpan(0, 5, 0).Humanize());

    [Fact]
    public void TimeSpanSeconds() =>
        Assert.Equal("5 seconds", new TimeSpan(0, 0, 5).Humanize());

    [Fact]
    public void TimeSpan1Day() =>
        Assert.Equal("1 day", new TimeSpan(24, 0, 0).Humanize());

    [Fact]
    public void TimeSpan1Hour() =>
        Assert.Equal("1 hour", new TimeSpan(1, 0, 0).Humanize());

    [Fact]
    public void TimeSpan1Millisecond() =>
        Assert.Equal("1 millisecond", new TimeSpan(0, 0, 0,  0, 1).Humanize());

    [Fact]
    public void TimeSpan1Minute() =>
        Assert.Equal("1 minute", new TimeSpan(0, 1, 0).Humanize());

    [Fact]
    public void TimeSpan1Second() =>
        Assert.Equal("1 second", new TimeSpan(0, 1, 0).Humanize());

    [Fact]
    public void TimeSpanZero() =>
        Assert.Equal("no time", new TimeSpan(0).Humanize());

    [Fact]
    public void TimeSpanMultipleWeeks() =>
        Assert.Equal("2 weeks", new TimeSpan(14,0,0,0).Humanize());

    [Fact]
    public void TimeSpan1Weeks() =>
        Assert.Equal("1 week", new TimeSpan(7, 0, 0, 0).Humanize());

    [Fact]
    public void DateTimeMultipleDaysFromNow() =>
        Assert.Equal("2 days from now", DateTime.UtcNow.AddDays(2).Humanize());

    [Fact]
    public void DateTimeMultipleHoursFromNow() =>
        Assert.Equal("2 hours from now", DateTime.UtcNow.AddHours(2).Humanize());

    [Fact]
    public void DateTimeMultipleMinutesFromNow() =>
        Assert.Equal("2 minutes from now", DateTime.UtcNow.AddMinutes(2).Humanize());

    [Fact]
    public void DateTimeMultipleMonthsFromNow() =>
        Assert.Equal("2 months from now", DateTime.UtcNow.AddMonths(2).Humanize());

    [Fact]
    public void DateTimeMultipleSecondsFromNow() =>
        Assert.Equal("2 seconds from now", DateTime.UtcNow.AddSeconds(2).Humanize());

    [Fact]
    public void DateTimeMultipleYearsFromNow() =>
        Assert.Equal("2 years from now", DateTime.UtcNow.AddYears(2).Humanize());

    [Fact]
    public void DateTimeNow() =>
        Assert.Equal("now", DateTime.UtcNow.Humanize());

    [Fact]
    public void DateTimeTomorrow() =>
        Assert.Equal("tomorrow", DateTime.UtcNow.AddDays(1).Humanize());

    [Fact]
    public void DateTime1HourFromNow() =>
        Assert.Equal("an hour from now", DateTime.UtcNow.AddHours(1).Humanize());

    [Fact]
    public void DateTime1MinuteFromNow() =>
        Assert.Equal("a minute from now", DateTime.UtcNow.AddMinutes(1).Humanize());

    [Fact]
    public void DateTime1MonthFromNow() =>
        Assert.Equal("one month from now", DateTime.UtcNow.AddMonths(1).Humanize());


    [Fact]
    public void DateTime1SecondFromNow() =>
        Assert.Equal("one second from now", DateTime.UtcNow.AddSeconds(1).Humanize());

    [Fact]
    public void DateTime1YearFromNow() =>
        Assert.Equal("one year from now", DateTime.UtcNow.AddYears(1).Humanize());
}