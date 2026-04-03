namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_nb
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "ett sekund siden" },
        { "DateFutureSecond2", "2 sekunder fra nå" },
        { "DatePastMinute1", "ett minutt siden" },
        { "DateFutureMinute2", "2 minutter fra nå" },
        { "DatePastHour1", "en time siden" },
        { "DateFutureHour2", "2 timer fra nå" },
        { "DatePastDay1", "i går" },
        { "DateFutureDay1", "i morgen" },
        { "DatePastDay2", "2 dager siden" },
        { "DateFutureDay2", "2 dager fra nå" },
        { "DatePastMonth1", "en måned siden" },
        { "DateFutureMonth2", "2 måneder fra nå" },
        { "DatePastYear1", "ett år siden" },
        { "DateFutureYear2", "2 år fra nå" },
        { "DateNow", "nå" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekund" },
        { "SpanSecond2", "2 sekunder" },
        { "SpanMinute1", "1 minutt" },
        { "SpanMinute2", "2 minutter" },
        { "SpanHour1", "1 time" },
        { "SpanHour2", "2 timer" },
        { "SpanDay1", "1 dag" },
        { "SpanDay2", "2 dager" },
        { "SpanZero", "0 millisekunder" },
        { "SpanZeroWords", "ingen tid" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("nb", caseName, expected);
}
