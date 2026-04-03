namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sv
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "en sekund sedan" },
        { "DateFutureSecond2", "om 2 sekunder" },
        { "DatePastMinute1", "en minut sedan" },
        { "DateFutureMinute2", "om 2 minuter" },
        { "DatePastHour1", "en timme sedan" },
        { "DateFutureHour2", "om 2 timmar" },
        { "DatePastDay1", "igår" },
        { "DateFutureDay1", "i morgon" },
        { "DatePastDay2", "för 2 dagar sedan" },
        { "DateFutureDay2", "om 2 dagar" },
        { "DatePastMonth1", "en månad sedan" },
        { "DateFutureMonth2", "om 2 månader" },
        { "DatePastYear1", "ett år sedan" },
        { "DateFutureYear2", "om 2 år" },
        { "DateNow", "nu" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekund" },
        { "SpanSecond2", "2 sekunder" },
        { "SpanMinute1", "1 minut" },
        { "SpanMinute2", "2 minuter" },
        { "SpanHour1", "1 timma" },
        { "SpanHour2", "2 timmar" },
        { "SpanDay1", "1 dag" },
        { "SpanDay2", "2 dagar" },
        { "SpanZero", "0 millisekunder" },
        { "SpanZeroWords", "ingen tid" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("sv", caseName, expected);
}
