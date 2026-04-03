namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_it
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "un secondo fa" },
        { "DateFutureSecond2", "tra 2 secondi" },
        { "DatePastMinute1", "un minuto fa" },
        { "DateFutureMinute2", "tra 2 minuti" },
        { "DatePastHour1", "un'ora fa" },
        { "DateFutureHour2", "tra 2 ore" },
        { "DatePastDay1", "ieri" },
        { "DateFutureDay1", "domani" },
        { "DatePastDay2", "2 giorni fa" },
        { "DateFutureDay2", "tra 2 giorni" },
        { "DatePastMonth1", "un mese fa" },
        { "DateFutureMonth2", "tra 2 mesi" },
        { "DatePastYear1", "un anno fa" },
        { "DateFutureYear2", "tra 2 anni" },
        { "DateNow", "adesso" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 secondo" },
        { "SpanSecond2", "2 secondi" },
        { "SpanMinute1", "1 minuto" },
        { "SpanMinute2", "2 minuti" },
        { "SpanHour1", "1 ora" },
        { "SpanHour2", "2 ore" },
        { "SpanDay1", "1 giorno" },
        { "SpanDay2", "2 giorni" },
        { "SpanZero", "0 millisecondi" },
        { "SpanZeroWords", "0 secondi" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("it", caseName, expected);
}
