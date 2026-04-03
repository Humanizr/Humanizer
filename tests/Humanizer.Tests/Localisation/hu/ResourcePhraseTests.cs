namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hu
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "egy másodperce" },
        { 2, TimeUnit.Second, Tense.Future, "2 másodperc múlva" },
        { 1, TimeUnit.Minute, Tense.Past, "egy perce" },
        { 2, TimeUnit.Minute, Tense.Future, "2 perc múlva" },
        { 1, TimeUnit.Hour, Tense.Past, "egy órája" },
        { 2, TimeUnit.Hour, Tense.Future, "2 óra múlva" },
        { 1, TimeUnit.Day, Tense.Past, "tegnap" },
        { 1, TimeUnit.Day, Tense.Future, "holnap" },
        { 2, TimeUnit.Day, Tense.Past, "2 napja" },
        { 2, TimeUnit.Day, Tense.Future, "2 nap múlva" },
        { 1, TimeUnit.Month, Tense.Past, "egy hónapja" },
        { 2, TimeUnit.Month, Tense.Future, "2 hónap múlva" },
        { 1, TimeUnit.Year, Tense.Past, "egy éve" },
        { 2, TimeUnit.Year, Tense.Future, "2 év múlva" },
        { 0, TimeUnit.Second, Tense.Future, "most" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 másodperc" },
        { 2, TimeUnit.Second, false, "2 másodperc" },
        { 1, TimeUnit.Minute, false, "1 perc" },
        { 2, TimeUnit.Minute, false, "2 perc" },
        { 1, TimeUnit.Hour, false, "1 óra" },
        { 2, TimeUnit.Hour, false, "2 óra" },
        { 1, TimeUnit.Day, false, "1 nap" },
        { 2, TimeUnit.Day, false, "2 nap" },
        { 0, TimeUnit.Millisecond, false, "0 ezredmásodperc" },
        { 0, TimeUnit.Millisecond, true, "nincs idő" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("hu", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("hu", "soha");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("hu", unit, timeUnit, toWords, expected);
}
