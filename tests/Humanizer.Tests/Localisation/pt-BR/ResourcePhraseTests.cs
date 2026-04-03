namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pt_BR
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "um segundo atrás" },
        { "DateFutureSecond2", "em 2 segundos" },
        { "DatePastMinute1", "um minuto atrás" },
        { "DateFutureMinute2", "em 2 minutos" },
        { "DatePastHour1", "uma hora atrás" },
        { "DateFutureHour2", "em 2 horas" },
        { "DatePastDay1", "ontem" },
        { "DateFutureDay1", "amanhã" },
        { "DatePastDay2", "2 dias atrás" },
        { "DateFutureDay2", "em 2 dias" },
        { "DatePastMonth1", "um mês atrás" },
        { "DateFutureMonth2", "em 2 meses" },
        { "DatePastYear1", "um ano atrás" },
        { "DateFutureYear2", "em 2 anos" },
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
        LocaleResourcePhraseAssertions.Verify("pt-BR", caseName, expected);
}
