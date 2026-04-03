namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lb
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "virun enger Sekonn" },
        { "DateFutureSecond2", "an 2 Sekonnen" },
        { "DatePastMinute1", "virun enger Minutt" },
        { "DateFutureMinute2", "an 2 Minutten" },
        { "DatePastHour1", "virun enger Stonn" },
        { "DateFutureHour2", "an 2 Stonnen" },
        { "DatePastDay1", "gëschter" },
        { "DateFutureDay1", "muer" },
        { "DatePastDay2", "virgëschter" },
        { "DateFutureDay2", "iwwermuer" },
        { "DatePastMonth1", "virun engem Mount" },
        { "DateFutureMonth2", "an 2 Méint" },
        { "DatePastYear1", "virun engem Joer" },
        { "DateFutureYear2", "an 2 Joer" },
        { "DateNow", "elo" },
        { "DateNever", "ni" },
        { "SpanSecond1", "1 Sekonn" },
        { "SpanSecond2", "2 Sekonnen" },
        { "SpanMinute1", "1 Minutt" },
        { "SpanMinute2", "2 Minutten" },
        { "SpanHour1", "1 Stonn" },
        { "SpanHour2", "2 Stonnen" },
        { "SpanDay1", "1 Dag" },
        { "SpanDay2", "2 Deeg" },
        { "SpanZero", "0 Millisekonnen" },
        { "SpanZeroWords", "Keng Zäit" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("lb", caseName, expected);
}
