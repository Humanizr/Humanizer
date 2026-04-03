namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_it
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "un secondo fa" },
        { 2, TimeUnit.Second, Tense.Future, "tra 2 secondi" },
        { 1, TimeUnit.Minute, Tense.Past, "un minuto fa" },
        { 2, TimeUnit.Minute, Tense.Future, "tra 2 minuti" },
        { 1, TimeUnit.Hour, Tense.Past, "un'ora fa" },
        { 2, TimeUnit.Hour, Tense.Future, "tra 2 ore" },
        { 1, TimeUnit.Day, Tense.Past, "ieri" },
        { 1, TimeUnit.Day, Tense.Future, "domani" },
        { 2, TimeUnit.Day, Tense.Past, "2 giorni fa" },
        { 2, TimeUnit.Day, Tense.Future, "tra 2 giorni" },
        { 1, TimeUnit.Month, Tense.Past, "un mese fa" },
        { 2, TimeUnit.Month, Tense.Future, "tra 2 mesi" },
        { 1, TimeUnit.Year, Tense.Past, "un anno fa" },
        { 2, TimeUnit.Year, Tense.Future, "tra 2 anni" },
        { 0, TimeUnit.Second, Tense.Future, "adesso" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 secondo" },
        { 2, TimeUnit.Second, false, "2 secondi" },
        { 1, TimeUnit.Minute, false, "1 minuto" },
        { 2, TimeUnit.Minute, false, "2 minuti" },
        { 1, TimeUnit.Hour, false, "1 ora" },
        { 2, TimeUnit.Hour, false, "2 ore" },
        { 1, TimeUnit.Day, false, "1 giorno" },
        { 2, TimeUnit.Day, false, "2 giorni" },
        { 0, TimeUnit.Millisecond, false, "0 millisecondi" },
        { 0, TimeUnit.Millisecond, true, "0 secondi" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("it", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("it", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("it", unit, timeUnit, toWords, expected);
}
