namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_mt
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "sekonda ilu" },
        { "DateFutureSecond2", "2 sekondi oħra" },
        { "DatePastMinute1", "minuta ilu" },
        { "DateFutureMinute2", "2 minuti oħra" },
        { "DatePastHour1", "siegħa ilu" },
        { "DateFutureHour2", "sagħtejn oħra" },
        { "DatePastDay1", "il-bieraħ" },
        { "DateFutureDay1", "għada" },
        { "DatePastDay2", "jumejn ilu" },
        { "DateFutureDay2", "pitgħada" },
        { "DatePastMonth1", "xahar ilu" },
        { "DateFutureMonth2", "xahrejn oħra" },
        { "DatePastYear1", "sena ilu" },
        { "DateFutureYear2", "sentejn oħra" },
        { "DateNow", "issa" },
        { "DateNever", "qatt" },
        { "SpanSecond1", "sekonda" },
        { "SpanSecond2", "2 sekondi" },
        { "SpanMinute1", "minuta" },
        { "SpanMinute2", "2 minuti" },
        { "SpanHour1", "siegħa" },
        { "SpanHour2", "sagħtejn" },
        { "SpanDay1", "ġurnata" },
        { "SpanDay2", "jumejn" },
        { "SpanZero", "0 millisekondi" },
        { "SpanZeroWords", "xejn" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("mt", caseName, expected);
}
