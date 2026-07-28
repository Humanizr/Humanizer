#if NET6_0_OR_GREATER

[UseCulture("en-US")]
public class DateOnlyHumanizeTests
{
    [Fact]
    public void DefaultStrategy_SameDate()
    {
        var inputTime = new DateOnly(2015, 07, 05);
        var baseTime = new DateOnly(2015, 07, 05);

        const string expectedResult = "today";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SameDate_UsesDateOnlyWordingWithoutChangingCultureFallback(bool usePrecisionStrategy)
    {
        IDateOnlyHumanizeStrategy strategy = usePrecisionStrategy
            ? new PrecisionDateOnlyHumanizeStrategy()
            : new DefaultDateOnlyHumanizeStrategy();

        var date = DateOnly.MinValue;

        Assert.Equal("today", strategy.Humanize(date, date, new("en-GB")));
        Assert.Equal("maintenant", strategy.Humanize(date, date, new("fr-FR")));
    }

    [Fact]
    public void TodayWording_PreservesCustomDateHumanizeOverride()
    {
        var formatter = new CustomDateHumanizeFormatter();
        var now = formatter.DateHumanize(TimeUnit.Millisecond, Tense.Past, 0);

        Assert.Equal("custom now", formatter.DateHumanize_Today(now));
    }

    [Fact]
    public void DefaultStrategy_MonthApart()
    {
        var inputTime = new DateOnly(2015, 08, 05);
        var baseTime = new DateOnly(2015, 07, 05);

        const string expectedResult = "one month from now";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_DaysAgo()
    {
        var inputTime = new DateOnly(2015, 07, 02);
        var baseTime = new DateOnly(2015, 07, 05);

        const string expectedResult = "3 days ago";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_YearsAgo()
    {
        var baseDate = DateTime.Now;
        var inputTime = DateOnly.FromDateTime(baseDate.AddMonths(-24));
        var baseTime = DateOnly.FromDateTime(baseDate);

        const string expectedResult = "2 years ago";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void PrecisionStrategy_NextDay()
    {
        var inputTime = new DateOnly(2015, 07, 05);
        var baseTime = new DateOnly(2015, 07, 04);

        const string expectedResult = "tomorrow";
        var actualResult = new PrecisionDateOnlyHumanizeStrategy(0.75)
            .Humanize(inputTime, baseTime, CultureInfo.CurrentUICulture);

        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(2015, 12, 31, 2016, 1, 1, "tomorrow")]
    [InlineData(2015, 1, 1, 2016, 1, 1, "one year from now")]
    public void PrecisionStrategy_UsesAbsoluteDayDistance(
        int baseYear,
        int baseMonth,
        int baseDay,
        int inputYear,
        int inputMonth,
        int inputDay,
        string expectedResult)
    {
        var baseTime = new DateOnly(baseYear, baseMonth, baseDay);
        var inputTime = new DateOnly(inputYear, inputMonth, inputDay);

        Assert.Equal(
            expectedResult,
            new PrecisionDateOnlyHumanizeStrategy(0.75).Humanize(inputTime, baseTime, CultureInfo.CurrentUICulture));
    }

    [Fact]
    public void Never()
    {
        DateOnly? never = null;
        Assert.Equal("never", never.Humanize());
    }

    [Fact]
    public void Nullable_ExpectSame()
    {
        DateOnly? never = new DateOnly(2015, 12, 7);

        Assert.Equal(never.Value.Humanize(), never.Humanize());
    }

    sealed class CustomDateHumanizeFormatter()
        : DefaultFormatter("en")
    {
        public override string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit) =>
            $"custom {base.DateHumanize(timeUnit, timeUnitTense, unit)}";
    }
}

#endif