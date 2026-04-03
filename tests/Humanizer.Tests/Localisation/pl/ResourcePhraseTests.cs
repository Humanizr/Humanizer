namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pl
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "przed sekundą" },
        { 2, TimeUnit.Second, Tense.Future, "za 2 sekundy" },
        { 1, TimeUnit.Minute, Tense.Past, "przed minutą" },
        { 2, TimeUnit.Minute, Tense.Future, "za 2 minuty" },
        { 1, TimeUnit.Hour, Tense.Past, "przed godziną" },
        { 2, TimeUnit.Hour, Tense.Future, "za 2 godziny" },
        { 1, TimeUnit.Day, Tense.Past, "wczoraj" },
        { 1, TimeUnit.Day, Tense.Future, "jutro" },
        { 2, TimeUnit.Day, Tense.Past, "przed 2 dniami" },
        { 2, TimeUnit.Day, Tense.Future, "za 2 dni" },
        { 1, TimeUnit.Month, Tense.Past, "przed miesiącem" },
        { 2, TimeUnit.Month, Tense.Future, "za 2 miesiące" },
        { 1, TimeUnit.Year, Tense.Past, "przed rokiem" },
        { 2, TimeUnit.Year, Tense.Future, "za 2 lata" },
        { 0, TimeUnit.Second, Tense.Future, "teraz" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunda" },
        { 2, TimeUnit.Second, false, "2 sekundy" },
        { 1, TimeUnit.Minute, false, "1 minuta" },
        { 2, TimeUnit.Minute, false, "2 minuty" },
        { 1, TimeUnit.Hour, false, "1 godzina" },
        { 2, TimeUnit.Hour, false, "2 godziny" },
        { 1, TimeUnit.Day, false, "1 dzień" },
        { 2, TimeUnit.Day, false, "2 dni" },
        { 0, TimeUnit.Millisecond, false, "0 milisekund" },
        { 0, TimeUnit.Millisecond, true, "brak czasu" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("pl", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("pl", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("pl", unit, timeUnit, toWords, expected);
}
