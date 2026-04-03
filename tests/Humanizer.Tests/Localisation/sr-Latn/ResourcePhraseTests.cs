namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sr_Latn
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "pre sekund" },
        { 2, TimeUnit.Second, Tense.Future, "za 2 sekunde" },
        { 1, TimeUnit.Minute, Tense.Past, "pre minut" },
        { 2, TimeUnit.Minute, Tense.Future, "za 2 minuta" },
        { 1, TimeUnit.Hour, Tense.Past, "pre sat vremena" },
        { 2, TimeUnit.Hour, Tense.Future, "za 2 sata" },
        { 1, TimeUnit.Day, Tense.Past, "juče" },
        { 1, TimeUnit.Day, Tense.Future, "sutra" },
        { 2, TimeUnit.Day, Tense.Past, "pre 2 dana" },
        { 2, TimeUnit.Day, Tense.Future, "za 2 dana" },
        { 1, TimeUnit.Month, Tense.Past, "pre mesec dana" },
        { 2, TimeUnit.Month, Tense.Future, "za 2 meseca" },
        { 1, TimeUnit.Year, Tense.Past, "pre godinu dana" },
        { 2, TimeUnit.Year, Tense.Future, "za 2 godine" },
        { 0, TimeUnit.Second, Tense.Future, "sada" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunda" },
        { 2, TimeUnit.Second, false, "2 sekunde" },
        { 1, TimeUnit.Minute, false, "1 minut" },
        { 2, TimeUnit.Minute, false, "2 minuta" },
        { 1, TimeUnit.Hour, false, "1 sat" },
        { 2, TimeUnit.Hour, false, "2 sata" },
        { 1, TimeUnit.Day, false, "1 dan" },
        { 2, TimeUnit.Day, false, "2 dana" },
        { 0, TimeUnit.Millisecond, false, "0 milisekundi" },
        { 0, TimeUnit.Millisecond, true, "bez proteklog vremena" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("sr-Latn", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("sr-Latn", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("sr-Latn", unit, timeUnit, toWords, expected);
}
