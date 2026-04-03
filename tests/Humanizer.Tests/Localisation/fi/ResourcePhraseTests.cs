namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fi
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "sekuntti sitten" },
        { "DateFutureSecond2", "2 seconds from now" },
        { "DatePastMinute1", "minuutti sitten" },
        { "DateFutureMinute2", "2 minuutin päästä" },
        { "DatePastHour1", "tunti sitten" },
        { "DateFutureHour2", "2 tunnin päästä" },
        { "DatePastDay1", "eilen" },
        { "DateFutureDay1", "huomenna" },
        { "DatePastDay2", "2 päivää sitten" },
        { "DateFutureDay2", "2 päivän päästä" },
        { "DatePastMonth1", "kuukausi sitten" },
        { "DateFutureMonth2", "2 months from now" },
        { "DatePastYear1", "vuosi sitten" },
        { "DateFutureYear2", "2 years from now" },
        { "DateNow", "now" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 second" },
        { "SpanSecond2", "2 seconds" },
        { "SpanMinute1", "1 minute" },
        { "SpanMinute2", "2 minutes" },
        { "SpanHour1", "tunti" },
        { "SpanHour2", "2 hours" },
        { "SpanDay1", "päivä" },
        { "SpanDay2", "2 days" },
        { "SpanZero", "0 milliseconds" },
        { "SpanZeroWords", "nyt" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("fi", caseName, expected);
}
