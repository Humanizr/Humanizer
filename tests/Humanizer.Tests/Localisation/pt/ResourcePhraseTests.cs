namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pt
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "há um segundo" },
        { "DateFutureSecond2", "daqui a 2 segundos" },
        { "DatePastMinute1", "há um minuto" },
        { "DateFutureMinute2", "daqui a 2 minutos" },
        { "DatePastHour1", "há uma hora" },
        { "DateFutureHour2", "daqui a 2 horas" },
        { "DatePastDay1", "ontem" },
        { "DateFutureDay1", "amanhã" },
        { "DatePastDay2", "há 2 dias" },
        { "DateFutureDay2", "daqui a 2 dias" },
        { "DatePastMonth1", "há um mês" },
        { "DateFutureMonth2", "daqui a 2 meses" },
        { "DatePastYear1", "há um ano" },
        { "DateFutureYear2", "daqui a 2 anos" },
        { "DateNow", "agora" },
        { "DateNever", "nunca" },
        { "SpanSecond1", "1 segundo" },
        { "SpanSecond2", "2 segundos" },
        { "SpanMinute1", "1 minuto" },
        { "SpanMinute2", "2 minutos" },
        { "SpanHour1", "1 hora" },
        { "SpanHour2", "2 horas" },
        { "SpanDay1", "1 dia" },
        { "SpanDay2", "2 dias" },
        { "SpanZero", "0 milisegundos" },
        { "SpanZeroWords", "sem horário" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("pt", caseName, expected);
}
