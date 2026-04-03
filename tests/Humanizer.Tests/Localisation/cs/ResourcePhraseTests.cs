namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_cs
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "před sekundou" },
        { 2, TimeUnit.Second, Tense.Future, "za 2 sekundy" },
        { 1, TimeUnit.Minute, Tense.Past, "před minutou" },
        { 2, TimeUnit.Minute, Tense.Future, "za 2 minuty" },
        { 1, TimeUnit.Hour, Tense.Past, "před hodinou" },
        { 2, TimeUnit.Hour, Tense.Future, "za 2 hodiny" },
        { 1, TimeUnit.Day, Tense.Past, "včera" },
        { 1, TimeUnit.Day, Tense.Future, "zítra" },
        { 2, TimeUnit.Day, Tense.Past, "před 2 dny" },
        { 2, TimeUnit.Day, Tense.Future, "za 2 dny" },
        { 1, TimeUnit.Month, Tense.Past, "před měsícem" },
        { 2, TimeUnit.Month, Tense.Future, "za 2 měsíce" },
        { 1, TimeUnit.Year, Tense.Past, "před rokem" },
        { 2, TimeUnit.Year, Tense.Future, "za 2 roky" },
        { 0, TimeUnit.Second, Tense.Future, "teď" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunda" },
        { 2, TimeUnit.Second, false, "2 sekundy" },
        { 1, TimeUnit.Minute, false, "1 minuta" },
        { 2, TimeUnit.Minute, false, "2 minuty" },
        { 1, TimeUnit.Hour, false, "1 hodina" },
        { 2, TimeUnit.Hour, false, "2 hodiny" },
        { 1, TimeUnit.Day, false, "1 den" },
        { 2, TimeUnit.Day, false, "2 dny" },
        { 0, TimeUnit.Millisecond, false, "0 milisekund" },
        { 0, TimeUnit.Millisecond, true, "není čas" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("cs", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("cs", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("cs", unit, timeUnit, toWords, expected);
}
