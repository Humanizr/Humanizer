namespace Humanizer.Tests.Localisation;

static class LocaleResourcePhraseAssertions
{
    public static void VerifyDateHumanize(string localeName, int unit, TimeUnit timeUnit, Tense tense, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        DateHumanize.Verify(expected, unit, timeUnit, tense, culture: culture);
    }

    public static void VerifyNullDateHumanize(string localeName, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        Assert.Equal(expected, ((DateTime?)null).Humanize(culture: culture));
    }

    public static void VerifyTimeSpanHumanize(string localeName, int unit, TimeUnit timeUnit, bool toWords, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);
        var timeSpan = timeUnit switch
        {
            TimeUnit.Millisecond => TimeSpan.FromMilliseconds(unit),
            TimeUnit.Second => TimeSpan.FromSeconds(unit),
            TimeUnit.Minute => TimeSpan.FromMinutes(unit),
            TimeUnit.Hour => TimeSpan.FromHours(unit),
            TimeUnit.Day => TimeSpan.FromDays(unit),
            _ => throw new Xunit.Sdk.XunitException($"Unsupported timespan unit '{timeUnit}'.")
        };

        Assert.Equal(expected, timeSpan.Humanize(culture: culture, toWords: toWords));
    }
}
