namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_da
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "et sekund siden" },
        { "DateFutureSecond2", "2 sekunder fra nu" },
        { "DatePastMinute1", "et minut siden" },
        { "DateFutureMinute2", "2 minutter fra nu" },
        { "DatePastHour1", "en time siden" },
        { "DateFutureHour2", "2 timer fra nu" },
        { "DatePastDay1", "i går" },
        { "DateFutureDay1", "i morgen" },
        { "DatePastDay2", "2 dage siden" },
        { "DateFutureDay2", "2 dage fra nu" },
        { "DatePastMonth1", "en måned siden" },
        { "DateFutureMonth2", "2 måneder fra nu" },
        { "DatePastYear1", "et år siden" },
        { "DateFutureYear2", "2 år fra nu" },
        { "DateNow", "nu" },
        { "DateNever", "never" },
        { "SpanSecond1", "et sekund" },
        { "SpanSecond2", "2 sekunder" },
        { "SpanMinute1", "et minut" },
        { "SpanMinute2", "2 minutter" },
        { "SpanHour1", "en time" },
        { "SpanHour2", "2 timer" },
        { "SpanDay1", "en dag" },
        { "SpanDay2", "2 dage" },
        { "SpanZero", "0 millisekunder" },
        { "SpanZeroWords", "ingen tid" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("da", caseName, expected);
}
