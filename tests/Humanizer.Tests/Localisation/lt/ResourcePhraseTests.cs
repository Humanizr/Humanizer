namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lt
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "prieš vieną sekundę" },
        { 2, TimeUnit.Second, Tense.Future, "po 2 sekundžių" },
        { 1, TimeUnit.Minute, Tense.Past, "prieš minutę" },
        { 2, TimeUnit.Minute, Tense.Future, "po 2 minučių" },
        { 1, TimeUnit.Hour, Tense.Past, "prieš valandą" },
        { 2, TimeUnit.Hour, Tense.Future, "po 2 valandų" },
        { 1, TimeUnit.Day, Tense.Past, "vakar" },
        { 1, TimeUnit.Day, Tense.Future, "rytoj" },
        { 2, TimeUnit.Day, Tense.Past, "prieš 2 dienas" },
        { 2, TimeUnit.Day, Tense.Future, "po 2 dienų" },
        { 1, TimeUnit.Month, Tense.Past, "prieš vieną mėnesį" },
        { 2, TimeUnit.Month, Tense.Future, "po 2 mėnesių" },
        { 1, TimeUnit.Year, Tense.Past, "prieš vienerius metus" },
        { 2, TimeUnit.Year, Tense.Future, "po 2 metų" },
        { 0, TimeUnit.Second, Tense.Future, "dabar" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekundė" },
        { 2, TimeUnit.Second, false, "2 sekundės" },
        { 1, TimeUnit.Minute, false, "1 minutė" },
        { 2, TimeUnit.Minute, false, "2 minutės" },
        { 1, TimeUnit.Hour, false, "1 valanda" },
        { 2, TimeUnit.Hour, false, "2 valandos" },
        { 1, TimeUnit.Day, false, "1 diena" },
        { 2, TimeUnit.Day, false, "2 dienos" },
        { 0, TimeUnit.Millisecond, false, "0 milisekundžių" },
        { 0, TimeUnit.Millisecond, true, "nėra laiko" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("lt", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("lt", "niekada");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("lt", unit, timeUnit, toWords, expected);
}
