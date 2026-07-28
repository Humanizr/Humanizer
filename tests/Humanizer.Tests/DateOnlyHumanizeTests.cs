#if NET6_0_OR_GREATER

[UseCulture("en-US")]
public class DateOnlyHumanizeTests
{
    [Fact]
    public void DefaultStrategy_SameDate()
    {
        Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

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
        Configurator.DateOnlyHumanizeStrategy = usePrecisionStrategy
            ? new PrecisionDateOnlyHumanizeStrategy()
            : new DefaultDateOnlyHumanizeStrategy();

        var date = DateOnly.MinValue;

        Assert.Equal("today", date.Humanize(date, new("en-GB")));
        Assert.Equal("maintenant", date.Humanize(date, new("fr-FR")));
    }

    [Fact]
    public void TodayWording_PreservesCustomNowOverride()
    {
        var formatter = new CustomNowFormatter();

        Assert.Equal("custom now", formatter.DateHumanize_Today());
    }

    [Fact]
    public void DefaultStrategy_MonthApart()
    {
        Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

        var inputTime = new DateOnly(2015, 08, 05);
        var baseTime = new DateOnly(2015, 07, 05);

        const string expectedResult = "one month from now";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_DaysAgo()
    {
        Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

        var inputTime = new DateOnly(2015, 07, 02);
        var baseTime = new DateOnly(2015, 07, 05);

        const string expectedResult = "3 days ago";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_YearsAgo()
    {
        Configurator.DateOnlyHumanizeStrategy = new DefaultDateOnlyHumanizeStrategy();

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
        Configurator.DateOnlyHumanizeStrategy = new PrecisionDateOnlyHumanizeStrategy(0.75);

        var inputTime = new DateOnly(2015, 07, 05);
        var baseTime = new DateOnly(2015, 07, 04);

        const string expectedResult = "tomorrow";
        var actualResult = inputTime.Humanize(baseTime);

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
        Configurator.DateOnlyHumanizeStrategy = new PrecisionDateOnlyHumanizeStrategy(0.75);

        var baseTime = new DateOnly(baseYear, baseMonth, baseDay);
        var inputTime = new DateOnly(inputYear, inputMonth, inputDay);

        Assert.Equal(expectedResult, inputTime.Humanize(baseTime));
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

    sealed class CustomNowFormatter()
        : DefaultFormatter("en")
    {
        public override string DateHumanize_Now() =>
            "custom now";
    }
}

#endif