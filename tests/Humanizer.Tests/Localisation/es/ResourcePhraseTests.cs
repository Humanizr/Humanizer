namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_es
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "hace un segundo" },
        { "DateFutureSecond2", "en 2 segundos" },
        { "DatePastMinute1", "hace un minuto" },
        { "DateFutureMinute2", "en 2 minutos" },
        { "DatePastHour1", "hace una hora" },
        { "DateFutureHour2", "en 2 horas" },
        { "DatePastDay1", "ayer" },
        { "DateFutureDay1", "mañana" },
        { "DatePastDay2", "hace 2 días" },
        { "DateFutureDay2", "en 2 días" },
        { "DatePastMonth1", "hace un mes" },
        { "DateFutureMonth2", "en 2 meses" },
        { "DatePastYear1", "hace un año" },
        { "DateFutureYear2", "en 2 años" },
        { "DateNow", "ahora" },
        { "DateNever", "nunca" },
        { "SpanSecond1", "1 segundo" },
        { "SpanSecond2", "2 segundos" },
        { "SpanMinute1", "1 minuto" },
        { "SpanMinute2", "2 minutos" },
        { "SpanHour1", "1 hora" },
        { "SpanHour2", "2 horas" },
        { "SpanDay1", "1 día" },
        { "SpanDay2", "2 días" },
        { "SpanZero", "0 milisegundos" },
        { "SpanZeroWords", "nada" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("es", caseName, expected);
}
