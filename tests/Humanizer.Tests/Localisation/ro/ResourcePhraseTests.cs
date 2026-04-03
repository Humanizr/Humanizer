namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ro
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "acum o secundă" },
        { 2, TimeUnit.Second, Tense.Future, "peste 2 secunde" },
        { 1, TimeUnit.Minute, Tense.Past, "acum un minut" },
        { 2, TimeUnit.Minute, Tense.Future, "peste 2 minute" },
        { 1, TimeUnit.Hour, Tense.Past, "acum o oră" },
        { 2, TimeUnit.Hour, Tense.Future, "peste 2 ore" },
        { 1, TimeUnit.Day, Tense.Past, "ieri" },
        { 1, TimeUnit.Day, Tense.Future, "mâine" },
        { 2, TimeUnit.Day, Tense.Past, "acum 2 zile" },
        { 2, TimeUnit.Day, Tense.Future, "peste 2 zile" },
        { 1, TimeUnit.Month, Tense.Past, "acum o lună" },
        { 2, TimeUnit.Month, Tense.Future, "peste 2 luni" },
        { 1, TimeUnit.Year, Tense.Past, "acum un an" },
        { 2, TimeUnit.Year, Tense.Future, "peste 2 ani" },
        { 0, TimeUnit.Second, Tense.Future, "acum" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 secundă" },
        { 2, TimeUnit.Second, false, "2 secunde" },
        { 1, TimeUnit.Minute, false, "1 minut" },
        { 2, TimeUnit.Minute, false, "2 minute" },
        { 1, TimeUnit.Hour, false, "1 oră" },
        { 2, TimeUnit.Hour, false, "2 ore" },
        { 1, TimeUnit.Day, false, "1 zi" },
        { 2, TimeUnit.Day, false, "2 zile" },
        { 0, TimeUnit.Millisecond, false, "0 de milisecunde" },
        { 0, TimeUnit.Millisecond, true, "0 secunde" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ro", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ro", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ro", unit, timeUnit, toWords, expected);
}
