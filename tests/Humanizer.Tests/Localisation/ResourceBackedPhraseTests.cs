namespace Humanizer.Tests.Localisation;

public class ResourceBackedPhraseTests
{
    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.DateHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedDateHumanizePhrases(string localeName, int unit, TimeUnit timeUnit, Tense tense, string expected)
    {
        var culture = GetCulture(localeName);
        DateHumanize.Verify(expected, unit, timeUnit, tense, culture: culture);
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.DateHumanizeBoundaryCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedDateHumanizeBoundaryPhrases(string localeName, int unit, TimeUnit timeUnit, Tense tense, string expected)
    {
        var culture = GetCulture(localeName);
        DateHumanize.Verify(expected, unit, timeUnit, tense, culture: culture);
    }

    [Theory]
    [MemberData(nameof(LocaleDateHumanizeTheoryData.AdditionalDateHumanizeCases), MemberType = typeof(LocaleDateHumanizeTheoryData))]
    public void UsesExpectedAdditionalDateHumanizePhrases(string localeName, int unit, TimeUnit timeUnit, Tense tense, string expected)
    {
        var culture = GetCulture(localeName);
        DateHumanize.Verify(expected, unit, timeUnit, tense, culture: culture);
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.NullDateHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedNullDateHumanizePhrase(string localeName, string expected)
    {
        var culture = GetCulture(localeName);
        Assert.Equal(expected, ((DateTime?)null).Humanize(culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeOnlyDefaultHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeOnlyDefaultHumanizePhrases(string localeName, string inputTime, string comparisonBase, string expected)
    {
        var culture = GetCulture(localeName);
        Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();

        Assert.Equal(expected, TimeOnly.Parse(inputTime).Humanize(TimeOnly.Parse(comparisonBase), culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeOnlyPrecisionHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeOnlyPrecisionHumanizePhrases(string localeName, string inputTime, string comparisonBase, double precision, string expected)
    {
        var culture = GetCulture(localeName);
        Configurator.TimeOnlyHumanizeStrategy = new PrecisionTimeOnlyHumanizeStrategy(precision);

        Assert.Equal(expected, TimeOnly.Parse(inputTime).Humanize(TimeOnly.Parse(comparisonBase), culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeOnlyNeverCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeOnlyNeverPhrase(string localeName, string expected)
    {
        var culture = GetCulture(localeName);
        Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();

        Assert.Equal(expected, ((TimeOnly?)null).Humanize(culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeOnlyNullableValueCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void TimeOnlyNullableValueMatchesNonNullableHumanize(string localeName, string timeValue)
    {
        var culture = GetCulture(localeName);
        Configurator.TimeOnlyHumanizeStrategy = new DefaultTimeOnlyHumanizeStrategy();
        var nullableTime = TimeOnly.Parse(timeValue);

        Assert.Equal(nullableTime.Humanize(culture: culture), ((TimeOnly?)nullableTime).Humanize(culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanHumanizePhrases(string localeName, int unit, TimeUnit timeUnit, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = CreateTimeSpan(unit, timeUnit);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanPluralizationCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanPluralizationPhrases(string localeName, int unit, TimeUnit timeUnit, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = CreateTimeSpan(unit, timeUnit);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanAdvancedHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedAdvancedTimeSpanHumanizePhrases(string localeName, int unit, TimeUnit timeUnit, bool toWords, bool useYearMaxUnit, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = CreateTimeSpan(unit, timeUnit);
        var actual = useYearMaxUnit
            ? timeSpan.Humanize(culture: culture, toWords: toWords, maxUnit: TimeUnit.Year)
            : timeSpan.Humanize(culture: culture, toWords: toWords);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanComplexCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedComplexTimeSpanPhrases(string localeName, int days, int hours, int minutes, int seconds, int precision, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = new TimeSpan(days, hours, minutes, seconds);

        Assert.Equal(expected, timeSpan.Humanize(precision, culture: culture, toWords: true));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanUniqueSequenceLocaleCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void TimeSpanHumanizeSequenceOfDaysProducesUniqueValues(string localeName)
    {
        var culture = GetCulture(localeName);
        var query = from day in Enumerable.Range(0, 100000)
                    let timeSpan = TimeSpan.FromDays(day)
                    let text = timeSpan.Humanize(precision: 3, culture: culture, maxUnit: TimeUnit.Year)
                    select text;
        var grouping = from text in query
                       group text by text into g
                       select g.Count();

        Assert.All(grouping, count => Assert.Equal(1, count));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanMaxUnitCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanMaxUnitPhrases(string localeName, long milliseconds, TimeUnit maxUnit, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, maxUnit: maxUnit));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanMinUnitCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanMinUnitPhrases(string localeName, long milliseconds, TimeUnit minUnit, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, minUnit: minUnit, precision: 7, maxUnit: TimeUnit.Year, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanPrecisionCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanPrecisionPhrases(string localeName, long milliseconds, int precision, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(precision, culture: culture, maxUnit: TimeUnit.Year, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanDaysWithPrecisionCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanDayPrecisionPhrases(string localeName, int days, int precision, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromDays(days);

        Assert.Equal(expected, timeSpan.Humanize(precision: precision, culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanMinAndMaxUnitParityCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanMinAndMaxUnitParity(string localeName, int minutes)
    {
        var culture = GetCulture(localeName);
        var expected = TimeSpan.FromMinutes(minutes).Humanize(2, culture);
        var actual = TimeSpan.FromMinutes(minutes).Humanize(2, culture, TimeUnit.Hour, TimeUnit.Minute);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanCountingEmptyUnitCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanCountingEmptyUnitPhrases(string localeName, long milliseconds, int precision, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, precision: precision, countEmptyUnits: true, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanAlternativeCollectionFormatterCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanAlternativeCollectionFormatterPhrases(string localeName, long milliseconds, int precision, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(precision, culture: culture, collectionSeparator: null, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanNumbersConvertedToWordsCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanNumberWordPhrases(string localeName, long milliseconds, int precision, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(precision, culture: culture, toWords: true));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanDayMaxUnitCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanDayMaxUnitPhrases(string localeName, int days, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromDays(days);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, maxUnit: TimeUnit.Day, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanToWordsSequenceCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanToWordsSequencePhrases(string localeName, int milliseconds, int precision, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        Assert.Equal(expected, timeSpan.Humanize(precision, culture: culture, toWords: true));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanZeroMinUnitCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanZeroMinUnitPhrases(string localeName, TimeUnit minUnit, string expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected, TimeSpan.Zero.Humanize(culture: culture, minUnit: minUnit));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanZeroCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanZeroPhrases(string localeName, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected, TimeSpan.Zero.Humanize(culture: culture, toWords: toWords));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanAgeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanAgePhrases(string localeName, int days, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = TimeSpan.FromDays(days);

        Assert.Equal(expected, timeSpan.ToAge(culture, toWords: toWords));
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);

    static TimeSpan CreateTimeSpan(int unit, TimeUnit timeUnit) => timeUnit switch
    {
        TimeUnit.Millisecond => TimeSpan.FromMilliseconds(unit),
        TimeUnit.Second => TimeSpan.FromSeconds(unit),
        TimeUnit.Minute => TimeSpan.FromMinutes(unit),
        TimeUnit.Hour => TimeSpan.FromHours(unit),
        TimeUnit.Week => TimeSpan.FromDays(unit * 7),
        TimeUnit.Day => TimeSpan.FromDays(unit),
        _ => throw new Xunit.Sdk.XunitException($"Unsupported timespan unit '{timeUnit}'.")
    };
}
