namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uz_Cyrl_UZ
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "бир сония аввал" },
        { "DateFutureSecond2", "2 секунддан сўнг" },
        { "DatePastMinute1", "бир дақиқа аввал" },
        { "DateFutureMinute2", "2 минутдан сўнг" },
        { "DatePastHour1", "бир соат аввал" },
        { "DateFutureHour2", "2 соатдан сўнг" },
        { "DatePastDay1", "кеча" },
        { "DateFutureDay1", "эртага" },
        { "DatePastDay2", "2 кун аввал" },
        { "DateFutureDay2", "2 кундан сўнг" },
        { "DatePastMonth1", "бир ой аввал" },
        { "DateFutureMonth2", "2 ойдан сўнг" },
        { "DatePastYear1", "бир йил аввал" },
        { "DateFutureYear2", "2 йилдан сўнг" },
        { "DateNow", "ҳозир" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 секунд" },
        { "SpanSecond2", "2 секунд" },
        { "SpanMinute1", "1 минут" },
        { "SpanMinute2", "2 минут" },
        { "SpanHour1", "1 соат" },
        { "SpanHour2", "2 соат" },
        { "SpanDay1", "1 кун" },
        { "SpanDay2", "2 кун" },
        { "SpanZero", "0 миллисекунд" },
        { "SpanZeroWords", "вақт йўқ" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("uz-Cyrl-UZ", caseName, expected);
}
