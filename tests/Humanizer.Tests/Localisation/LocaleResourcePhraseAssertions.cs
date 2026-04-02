namespace Humanizer.Tests.Localisation;

static class LocaleResourcePhraseAssertions
{
    public static void Verify(string localeName, string yesterday, string inTwoDays, string twoDays, string noTime)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);

        DateHumanize.Verify(yesterday, 1, TimeUnit.Day, Tense.Past, culture: culture);
        DateHumanize.Verify(inTwoDays, 2, TimeUnit.Day, Tense.Future, culture: culture);
        Assert.Equal(twoDays, TimeSpan.FromDays(2).Humanize(culture: culture));
        Assert.Equal(noTime, TimeSpan.Zero.Humanize(culture: culture, toWords: true));
    }
}
