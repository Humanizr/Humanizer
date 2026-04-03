namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pt
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "há um segundo" },
        { 2, TimeUnit.Second, Tense.Future, "daqui a 2 segundos" },
        { 1, TimeUnit.Minute, Tense.Past, "há um minuto" },
        { 2, TimeUnit.Minute, Tense.Future, "daqui a 2 minutos" },
        { 1, TimeUnit.Hour, Tense.Past, "há uma hora" },
        { 2, TimeUnit.Hour, Tense.Future, "daqui a 2 horas" },
        { 1, TimeUnit.Day, Tense.Past, "ontem" },
        { 1, TimeUnit.Day, Tense.Future, "amanhã" },
        { 2, TimeUnit.Day, Tense.Past, "há 2 dias" },
        { 2, TimeUnit.Day, Tense.Future, "daqui a 2 dias" },
        { 1, TimeUnit.Month, Tense.Past, "há um mês" },
        { 2, TimeUnit.Month, Tense.Future, "daqui a 2 meses" },
        { 1, TimeUnit.Year, Tense.Past, "há um ano" },
        { 2, TimeUnit.Year, Tense.Future, "daqui a 2 anos" },
        { 0, TimeUnit.Second, Tense.Future, "agora" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 segundo" },
        { 2, TimeUnit.Second, false, "2 segundos" },
        { 1, TimeUnit.Minute, false, "1 minuto" },
        { 2, TimeUnit.Minute, false, "2 minutos" },
        { 1, TimeUnit.Hour, false, "1 hora" },
        { 2, TimeUnit.Hour, false, "2 horas" },
        { 1, TimeUnit.Day, false, "1 dia" },
        { 2, TimeUnit.Day, false, "2 dias" },
        { 0, TimeUnit.Millisecond, false, "0 milisegundos" },
        { 0, TimeUnit.Millisecond, true, "sem horário" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("pt", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("pt", "nunca");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("pt", unit, timeUnit, toWords, expected);
}
