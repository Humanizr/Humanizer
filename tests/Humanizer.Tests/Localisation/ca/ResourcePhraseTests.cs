namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ca
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "fa un segon" },
        { 2, TimeUnit.Second, Tense.Future, "d'aquí 2 segons" },
        { 1, TimeUnit.Minute, Tense.Past, "fa un minut" },
        { 2, TimeUnit.Minute, Tense.Future, "d'aquí 2 minuts" },
        { 1, TimeUnit.Hour, Tense.Past, "fa una hora" },
        { 2, TimeUnit.Hour, Tense.Future, "d'aquí 2 hores" },
        { 1, TimeUnit.Day, Tense.Past, "ahir" },
        { 1, TimeUnit.Day, Tense.Future, "demà" },
        { 2, TimeUnit.Day, Tense.Past, "fa 2 dies" },
        { 2, TimeUnit.Day, Tense.Future, "d'aquí 2 dies" },
        { 1, TimeUnit.Month, Tense.Past, "fa un mes" },
        { 2, TimeUnit.Month, Tense.Future, "d'aquí 2 mesos" },
        { 1, TimeUnit.Year, Tense.Past, "fa un any" },
        { 2, TimeUnit.Year, Tense.Future, "d'aquí 2 anys" },
        { 0, TimeUnit.Second, Tense.Future, "ara" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 segon" },
        { 2, TimeUnit.Second, false, "2 segons" },
        { 1, TimeUnit.Minute, false, "1 minut" },
        { 2, TimeUnit.Minute, false, "2 minuts" },
        { 1, TimeUnit.Hour, false, "1 hora" },
        { 2, TimeUnit.Hour, false, "2 hores" },
        { 1, TimeUnit.Day, false, "1 dia" },
        { 2, TimeUnit.Day, false, "2 dies" },
        { 0, TimeUnit.Millisecond, false, "0 mil·lisegons" },
        { 0, TimeUnit.Millisecond, true, "res" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ca", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ca", "mai");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ca", unit, timeUnit, toWords, expected);
}
