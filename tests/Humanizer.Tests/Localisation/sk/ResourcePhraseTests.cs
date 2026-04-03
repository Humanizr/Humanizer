namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sk
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "pred sekundou" },
        { 2, TimeUnit.Second, Tense.Future, "o 2 sekundy" },
        { 1, TimeUnit.Minute, Tense.Past, "pred minútou" },
        { 2, TimeUnit.Minute, Tense.Future, "o 2 minúty" },
        { 1, TimeUnit.Hour, Tense.Past, "pred hodinou" },
        { 2, TimeUnit.Hour, Tense.Future, "o 2 hodiny" },
        { 1, TimeUnit.Day, Tense.Past, "včera" },
        { 1, TimeUnit.Day, Tense.Future, "zajtra" },
        { 2, TimeUnit.Day, Tense.Past, "pred 2 dňami" },
        { 2, TimeUnit.Day, Tense.Future, "o 2 dni" },
        { 1, TimeUnit.Month, Tense.Past, "pred mesiacom" },
        { 2, TimeUnit.Month, Tense.Future, "o 2 mesiace" },
        { 1, TimeUnit.Year, Tense.Past, "pred rokom" },
        { 2, TimeUnit.Year, Tense.Future, "o 2 roky" },
        { 0, TimeUnit.Second, Tense.Future, "teraz" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunda" },
        { 2, TimeUnit.Second, false, "2 sekundy" },
        { 1, TimeUnit.Minute, false, "1 minúta" },
        { 2, TimeUnit.Minute, false, "2 minúty" },
        { 1, TimeUnit.Hour, false, "1 hodina" },
        { 2, TimeUnit.Hour, false, "2 hodiny" },
        { 1, TimeUnit.Day, false, "1 deň" },
        { 2, TimeUnit.Day, false, "2 dni" },
        { 0, TimeUnit.Millisecond, false, "0 milisekúnd" },
        { 0, TimeUnit.Millisecond, true, "žiadny čas" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("sk", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("sk", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("sk", unit, timeUnit, toWords, expected);
}
