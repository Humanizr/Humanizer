namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fi
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "sekuntti sitten" },
        { 2, TimeUnit.Second, Tense.Future, "2 seconds from now" },
        { 1, TimeUnit.Minute, Tense.Past, "minuutti sitten" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minuutin päästä" },
        { 1, TimeUnit.Hour, Tense.Past, "tunti sitten" },
        { 2, TimeUnit.Hour, Tense.Future, "2 tunnin päästä" },
        { 1, TimeUnit.Day, Tense.Past, "eilen" },
        { 1, TimeUnit.Day, Tense.Future, "huomenna" },
        { 2, TimeUnit.Day, Tense.Past, "2 päivää sitten" },
        { 2, TimeUnit.Day, Tense.Future, "2 päivän päästä" },
        { 1, TimeUnit.Month, Tense.Past, "kuukausi sitten" },
        { 2, TimeUnit.Month, Tense.Future, "2 months from now" },
        { 1, TimeUnit.Year, Tense.Past, "vuosi sitten" },
        { 2, TimeUnit.Year, Tense.Future, "2 years from now" },
        { 0, TimeUnit.Second, Tense.Future, "now" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 second" },
        { 2, TimeUnit.Second, false, "2 seconds" },
        { 1, TimeUnit.Minute, false, "1 minute" },
        { 2, TimeUnit.Minute, false, "2 minutes" },
        { 1, TimeUnit.Hour, false, "tunti" },
        { 2, TimeUnit.Hour, false, "2 hours" },
        { 1, TimeUnit.Day, false, "päivä" },
        { 2, TimeUnit.Day, false, "2 days" },
        { 0, TimeUnit.Millisecond, false, "0 milliseconds" },
        { 0, TimeUnit.Millisecond, true, "nyt" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("fi", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("fi", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("fi", unit, timeUnit, toWords, expected);
}
