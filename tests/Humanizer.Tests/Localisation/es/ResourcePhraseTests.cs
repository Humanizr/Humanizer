namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_es
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "hace un segundo" },
        { 2, TimeUnit.Second, Tense.Future, "en 2 segundos" },
        { 1, TimeUnit.Minute, Tense.Past, "hace un minuto" },
        { 2, TimeUnit.Minute, Tense.Future, "en 2 minutos" },
        { 1, TimeUnit.Hour, Tense.Past, "hace una hora" },
        { 2, TimeUnit.Hour, Tense.Future, "en 2 horas" },
        { 1, TimeUnit.Day, Tense.Past, "ayer" },
        { 1, TimeUnit.Day, Tense.Future, "mañana" },
        { 2, TimeUnit.Day, Tense.Past, "hace 2 días" },
        { 2, TimeUnit.Day, Tense.Future, "en 2 días" },
        { 1, TimeUnit.Month, Tense.Past, "hace un mes" },
        { 2, TimeUnit.Month, Tense.Future, "en 2 meses" },
        { 1, TimeUnit.Year, Tense.Past, "hace un año" },
        { 2, TimeUnit.Year, Tense.Future, "en 2 años" },
        { 0, TimeUnit.Second, Tense.Future, "ahora" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 segundo" },
        { 2, TimeUnit.Second, false, "2 segundos" },
        { 1, TimeUnit.Minute, false, "1 minuto" },
        { 2, TimeUnit.Minute, false, "2 minutos" },
        { 1, TimeUnit.Hour, false, "1 hora" },
        { 2, TimeUnit.Hour, false, "2 horas" },
        { 1, TimeUnit.Day, false, "1 día" },
        { 2, TimeUnit.Day, false, "2 días" },
        { 0, TimeUnit.Millisecond, false, "0 milisegundos" },
        { 0, TimeUnit.Millisecond, true, "nada" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("es", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("es", "nunca");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("es", unit, timeUnit, toWords, expected);
}
