namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_af
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "1 sekonde terug" },
        { "DateFutureSecond2", "oor 2 sekondes" },
        { "DatePastMinute1", "1 minuut terug" },
        { "DateFutureMinute2", "oor 2 minute" },
        { "DatePastHour1", "1 uur terug" },
        { "DateFutureHour2", "oor 2 ure" },
        { "DatePastDay1", "gister" },
        { "DateFutureDay1", "môre" },
        { "DatePastDay2", "2 dae gelede" },
        { "DateFutureDay2", "oor 2 dae" },
        { "DatePastMonth1", "1 maand gelede" },
        { "DateFutureMonth2", "oor 2 maande" },
        { "DatePastYear1", "1 jaar gelede" },
        { "DateFutureYear2", "oor 2 jaar" },
        { "DateNow", "nou" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekond" },
        { "SpanSecond2", "2 sekondes" },
        { "SpanMinute1", "1 minuut" },
        { "SpanMinute2", "2 minute" },
        { "SpanHour1", "1 uur" },
        { "SpanHour2", "2 ure" },
        { "SpanDay1", "1 dag" },
        { "SpanDay2", "2 dae" },
        { "SpanZero", "0 millisekondes" },
        { "SpanZeroWords", "geen tyd" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("af", caseName, expected);
}
