// Coverage tests for TimeSpanHumanizeExtensions tail branches identified in
// artifacts/fn-9-local-coverage/uncovered.json.
//
// Targets:
// - FormatAge with generic format string (line 74)
// - GetNormalCaseTimeAsInteger isTimeUnitToGetTheMaximumTimeUnit branch (lines 175-179)
// - TimeSpan.Zero with minUnit variations
// - Negative TimeSpan paths
// - countEmptyUnits + precision combinations
// - collectionSeparator = null path (line 221)
// - Year/Month maxUnit with large TimeSpan values

namespace Humanizer.Tests.Localisation;

[UseCulture("en-US")]
public class TimeSpanHumanizeTailCoverageTests
{
    // ---------------------------------------------------------------------------
    //  FormatAge: generic format string path (line 74)
    //  When ageFormat is not "{0} old" and not "{value} old" and doesn't contain
    //  "{value}", it falls through to string.Format.
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData("Age: {0}", "2 days", "Age: 2 days")]
    [InlineData("{0}!", "3 weeks", "3 weeks!")]
    public void FormatAge_FallsBackToStringFormat(string ageFormat, string value, string expected)
    {
        var actual = TimeSpanHumanizeExtensions.FormatAge(value, ageFormat);
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------------
    //  GetNormalCaseTimeAsInteger: isTimeUnitToGetTheMaximumTimeUnit branch (lines 175-179)
    //  When maxUnit matches the current unit being computed, the method returns
    //  (int)totalTimeNumberOfUnits instead of timeNumberOfUnits.
    // ---------------------------------------------------------------------------

    [Fact]
    public void MaxUnitMillisecond_UsesTotalMilliseconds()
    {
        var actual = TimeSpan.FromMilliseconds(int.MaxValue).Humanize(precision: 1, maxUnit: TimeUnit.Millisecond);
        Assert.Equal("2147483647 milliseconds", actual);
    }

    [Fact]
    public void MaxUnitSecond_UsesTotalSeconds()
    {
        var actual = TimeSpan.FromSeconds(int.MaxValue).Humanize(precision: 1, maxUnit: TimeUnit.Second);
        Assert.Equal("2147483647 seconds", actual);
    }

    // ---------------------------------------------------------------------------
    //  TimeSpan.Zero with various minUnit/toWords combinations
    //  Exercises IsContainingOnlyNullValue and CreateTimePartsWithNoTimeValue paths
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(TimeUnit.Second, false, "0 seconds")]
    [InlineData(TimeUnit.Minute, false, "0 minutes")]
    [InlineData(TimeUnit.Hour, false, "0 hours")]
    [InlineData(TimeUnit.Day, false, "0 days")]
    [InlineData(TimeUnit.Week, false, "0 weeks")]
    [InlineData(TimeUnit.Second, true, "no time")]
    [InlineData(TimeUnit.Minute, true, "no time")]
    public void Zero_WithVariousMinUnits(TimeUnit minUnit, bool toWords, string expected)
    {
        var actual = TimeSpan.Zero.Humanize(minUnit: minUnit, toWords: toWords);
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------------
    //  Negative TimeSpan: BuildFormatTimePart uses Math.Abs
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(-7, "1 week")]
    [InlineData(-14, "2 weeks")]
    [InlineData(-1, "1 day")]
    public void Negative_TimeSpan_ReturnsPositiveText(int days, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize();
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------------
    //  Year/Month maxUnit with precision
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(400, 2, "1 year, 1 month")]
    [InlineData(800, 3, "2 years, 2 months, 8 days")]
    public void YearMonthMaxUnit_WithPrecision(int days, int precision, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize(precision: precision, maxUnit: TimeUnit.Year);
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------------
    //  countEmptyUnits = true: middle gaps count toward precision
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(86400000 + 1000, 3, true, "1 day")]
    [InlineData(86400000 + 1000, 4, true, "1 day, 1 second")]
    public void CountEmptyUnits_ReducesPrecision(long ms, int precision, bool countEmpty, string expected)
    {
        var actual = TimeSpan.FromMilliseconds(ms).Humanize(precision: precision, countEmptyUnits: countEmpty);
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------------
    //  collectionSeparator = null: uses CollectionFormatter
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(62000, 2, "1 minute and 2 seconds")]
    [InlineData(62020, 3, "1 minute, 2 seconds, and 20 milliseconds")]
    public void NullCollectionSeparator_UsesCollectionFormatter(int ms, int precision, string expected)
    {
        var actual = TimeSpan.FromMilliseconds(ms).Humanize(precision, collectionSeparator: null);
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------------
    //  ToAge: exercises the ToAge extension method
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(30, false, "4 weeks old")]
    [InlineData(400, false, "1 year old")]
    [InlineData(400, true, "one year old")]
    public void ToAge_WithVariousInputs(int days, bool toWords, string expected)
    {
        var actual = TimeSpan.FromDays(days).ToAge(toWords: toWords);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToAge_WithMaxUnitMonth()
    {
        var actual = TimeSpan.FromDays(400).ToAge(maxUnit: TimeUnit.Month);
        Assert.Equal("13 months old", actual);
    }

    // ---------------------------------------------------------------------------
    //  Very large TimeSpan with year maxUnit
    // ---------------------------------------------------------------------------

    [Fact]
    public void VeryLargeTimeSpan_WithYearMaxUnit()
    {
        var ts = TimeSpan.FromDays(3650); // ~10 years
        var actual = ts.Humanize(precision: 1, maxUnit: TimeUnit.Year);
        Assert.Equal("9 years", actual);
    }

    // ---------------------------------------------------------------------------
    //  GetSpecialCaseWeeksAsInteger: timespan.Days < DaysInAMonth path
    // ---------------------------------------------------------------------------

    [Theory]
    [InlineData(20, 2, "2 weeks, 6 days")]
    [InlineData(28, 2, "4 weeks")]
    public void WeeksWithDaysUnderMonth_Precision(int days, int precision, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize(precision: precision, maxUnit: TimeUnit.Month);
        Assert.Equal(expected, actual);
    }

    // GetSpecialCaseDaysAsInteger: maximumTimeUnit == Week path
    [Theory]
    [InlineData(50, 2, "7 weeks, 1 day")]
    public void DaysWithWeekMaxUnit(int days, int precision, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize(precision: precision, maxUnit: TimeUnit.Week);
        Assert.Equal(expected, actual);
    }

    // GetSpecialCaseDaysAsInteger: maximumTimeUnit > Week and days >= DaysInAMonth
    [Theory]
    [InlineData(45, 3, "1 month, 14 days")]
    public void DaysWithMonthMaxUnit_Over30Days(int days, int precision, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize(precision: precision, maxUnit: TimeUnit.Year);
        Assert.Equal(expected, actual);
    }

    // Precision with toWords=true
    [Theory]
    [InlineData(0, true, "no time")]
    [InlineData(86400000, true, "one day")]
    public void ToWords_WithZeroAndOneDay(long ms, bool toWords, string expected)
    {
        var actual = TimeSpan.FromMilliseconds(ms).Humanize(precision: 1, toWords: toWords);
        Assert.Equal(expected, actual);
    }
}