namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_nl
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "1 seconde geleden" },
        { "DateFutureSecond2", "over 2 seconden" },
        { "DatePastMinute1", "1 minuut geleden" },
        { "DateFutureMinute2", "over 2 minuten" },
        { "DatePastHour1", "1 uur geleden" },
        { "DateFutureHour2", "over 2 uur" },
        { "DatePastDay1", "gisteren" },
        { "DateFutureDay1", "morgen" },
        { "DatePastDay2", "2 dagen geleden" },
        { "DateFutureDay2", "over 2 dagen" },
        { "DatePastMonth1", "1 maand geleden" },
        { "DateFutureMonth2", "over 2 maanden" },
        { "DatePastYear1", "1 jaar geleden" },
        { "DateFutureYear2", "over 2 jaar" },
        { "DateNow", "nu" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 seconde" },
        { "SpanSecond2", "2 seconden" },
        { "SpanMinute1", "1 minuut" },
        { "SpanMinute2", "2 minuten" },
        { "SpanHour1", "1 uur" },
        { "SpanHour2", "2 uur" },
        { "SpanDay1", "1 dag" },
        { "SpanDay2", "2 dagen" },
        { "SpanZero", "0 milliseconden" },
        { "SpanZeroWords", "geen tijd" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("nl", caseName, expected);
}
