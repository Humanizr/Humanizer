namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hr
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "prije jedne sekunde" },
        { 2, TimeUnit.Second, Tense.Future, "za 2 sekunde" },
        { 1, TimeUnit.Minute, Tense.Past, "prije jedne minute" },
        { 2, TimeUnit.Minute, Tense.Future, "za 2 minute" },
        { 1, TimeUnit.Hour, Tense.Past, "prije sat vremena" },
        { 2, TimeUnit.Hour, Tense.Future, "za 2 sata" },
        { 1, TimeUnit.Day, Tense.Past, "jučer" },
        { 1, TimeUnit.Day, Tense.Future, "sutra" },
        { 2, TimeUnit.Day, Tense.Past, "prije 2 dana" },
        { 2, TimeUnit.Day, Tense.Future, "2 days from now" },
        { 1, TimeUnit.Month, Tense.Past, "prije mjesec dana" },
        { 2, TimeUnit.Month, Tense.Future, "za 2 mjeseca" },
        { 1, TimeUnit.Year, Tense.Past, "prije godinu dana" },
        { 2, TimeUnit.Year, Tense.Future, "za 2 godine" },
        { 0, TimeUnit.Second, Tense.Future, "upravo sada" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunda" },
        { 2, TimeUnit.Second, false, "2 sekunde" },
        { 1, TimeUnit.Minute, false, "1 minuta" },
        { 2, TimeUnit.Minute, false, "2 minute" },
        { 1, TimeUnit.Hour, false, "1 sat" },
        { 2, TimeUnit.Hour, false, "2 sata" },
        { 1, TimeUnit.Day, false, "1 dan" },
        { 2, TimeUnit.Day, false, "2 dana" },
        { 0, TimeUnit.Millisecond, false, "0 milisekundi" },
        { 0, TimeUnit.Millisecond, true, "bez podatka o vremenu" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("hr", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("hr", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("hr", unit, timeUnit, toWords, expected);
}
