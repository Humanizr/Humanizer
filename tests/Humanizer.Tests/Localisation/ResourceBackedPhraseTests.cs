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
    [MemberData(nameof(LocalePhraseTheoryData.NullDateHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedNullDateHumanizePhrase(string localeName, string expected)
    {
        var culture = GetCulture(localeName);
        Assert.Equal(expected, ((DateTime?)null).Humanize(culture: culture));
    }

    [Theory]
    [MemberData(nameof(LocalePhraseTheoryData.TimeSpanHumanizeCases), MemberType = typeof(LocalePhraseTheoryData))]
    public void UsesExpectedTimeSpanHumanizePhrases(string localeName, int unit, TimeUnit timeUnit, bool toWords, string expected)
    {
        var culture = GetCulture(localeName);
        var timeSpan = CreateTimeSpan(unit, timeUnit);

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, toWords: toWords));
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);

    static TimeSpan CreateTimeSpan(int unit, TimeUnit timeUnit) => timeUnit switch
    {
        TimeUnit.Millisecond => TimeSpan.FromMilliseconds(unit),
        TimeUnit.Second => TimeSpan.FromSeconds(unit),
        TimeUnit.Minute => TimeSpan.FromMinutes(unit),
        TimeUnit.Hour => TimeSpan.FromHours(unit),
        TimeUnit.Day => TimeSpan.FromDays(unit),
        _ => throw new Xunit.Sdk.XunitException($"Unsupported timespan unit '{timeUnit}'.")
    };
}
