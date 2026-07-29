using Humanizer.Tests.Localisation;

[UseCulture("en-US")]
public class DateTimeOffsetHumanizeTests
{
    [Fact]
    public void DefaultStrategy_SameOffset()
    {
        Configurator.DateTimeOffsetHumanizeStrategy = new DefaultDateTimeOffsetHumanizeStrategy();

        var inputTime = new DateTimeOffset(2015, 07, 05, 04, 0, 0, TimeSpan.Zero);
        var baseTime = new DateTimeOffset(2015, 07, 05, 03, 0, 0, TimeSpan.Zero);

        const string expectedResult = "an hour from now";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_DifferentOffsets()
    {
        Configurator.DateTimeOffsetHumanizeStrategy = new DefaultDateTimeOffsetHumanizeStrategy();

        var inputTime = new DateTimeOffset(2015, 07, 05, 03, 0, 0, new(2, 0, 0));
        var baseTime = new DateTimeOffset(2015, 07, 05, 02, 30, 0, new(1, 0, 0));

        const string expectedResult = "30 minutes ago";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void DefaultStrategy_WeekAcrossDifferentOffsets()
    {
        Configurator.DateTimeOffsetHumanizeStrategy = new DefaultDateTimeOffsetHumanizeStrategy();

        var baseTime = new DateTimeOffset(2024, 01, 01, 10, 0, 0, TimeSpan.FromHours(2));
        var inputTime = new DateTimeOffset(2024, 01, 08, 03, 0, 0, TimeSpan.FromHours(-5));

        Assert.Equal("one week from now", inputTime.Humanize(baseTime));
    }

    [Fact]
    public void PrecisionStrategy_SameOffset()
    {
        Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(0.75);

        var inputTime = new DateTimeOffset(2015, 07, 05, 04, 0, 0, TimeSpan.Zero);
        var baseTime = new DateTimeOffset(2015, 07, 04, 05, 0, 0, TimeSpan.Zero);

        const string expectedResult = "tomorrow";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void PrecisionStrategy_DifferentOffsets()
    {
        Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(0.75);

        var inputTime = new DateTimeOffset(2015, 07, 05, 03, 45, 0, new(2, 0, 0));
        var baseTime = new DateTimeOffset(2015, 07, 05, 02, 30, 0, new(-5, 0, 0));

        const string expectedResult = "6 hours ago";
        var actualResult = inputTime.Humanize(baseTime);

        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(27)]
    [InlineData(28)]
    [InlineData(29)]
    public void PrecisionStrategy_TwoMonthsAroundSixtyDays(int day)
    {
        var strategy = new PrecisionDateTimeOffsetHumanizeStrategy(0.75);
        var inputTime = new DateTimeOffset(2019, 01, day, 0, 0, 0, TimeSpan.Zero);
        var baseTime = new DateTimeOffset(2019, 03, 29, 0, 0, 0, TimeSpan.Zero);

        Assert.Equal("2 months ago", strategy.Humanize(inputTime, baseTime, null));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.FormatterExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Humanize_UsesSpecifiedCulture(string localeName, string expectedYesterday, string _)
    {
        var baseTime = new DateTimeOffset(2024, 1, 2, 12, 0, 0, TimeSpan.Zero);
        var culture = new CultureInfo(localeName);

        Assert.Equal(
            expectedYesterday,
            baseTime.AddDays(-1).Humanize(baseTime, culture));
        Assert.Equal(
            Configurator.GetFormatter(culture).DateHumanize(TimeUnit.Day, Tense.Past, 2),
            baseTime.AddDays(-2).Humanize(baseTime, culture));
    }

    [Fact]
    public void Never()
    {
        DateTimeOffset? never = null;
        Assert.Equal("never", never.Humanize());
    }

    [Fact]
    public void Nullable_ExpectSame()
    {
        DateTimeOffset? never = new DateTimeOffset(2015, 12, 7, 9, 0, 0, TimeSpan.FromHours(1));

        Assert.Equal(never.Value.Humanize(), never.Humanize());
    }
}