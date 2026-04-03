namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ro
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "acum o secundă" },
        { "DateFutureSecond2", "peste 2 secunde" },
        { "DatePastMinute1", "acum un minut" },
        { "DateFutureMinute2", "peste 2 minute" },
        { "DatePastHour1", "acum o oră" },
        { "DateFutureHour2", "peste 2 ore" },
        { "DatePastDay1", "ieri" },
        { "DateFutureDay1", "mâine" },
        { "DatePastDay2", "acum 2 zile" },
        { "DateFutureDay2", "peste 2 zile" },
        { "DatePastMonth1", "acum o lună" },
        { "DateFutureMonth2", "peste 2 luni" },
        { "DatePastYear1", "acum un an" },
        { "DateFutureYear2", "peste 2 ani" },
        { "DateNow", "acum" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 secundă" },
        { "SpanSecond2", "2 secunde" },
        { "SpanMinute1", "1 minut" },
        { "SpanMinute2", "2 minute" },
        { "SpanHour1", "1 oră" },
        { "SpanHour2", "2 ore" },
        { "SpanDay1", "1 zi" },
        { "SpanDay2", "2 zile" },
        { "SpanZero", "0 de milisecunde" },
        { "SpanZeroWords", "0 secunde" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ro", caseName, expected);
}
