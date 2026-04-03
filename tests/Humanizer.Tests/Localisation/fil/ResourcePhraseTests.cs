namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fil
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "isang segundo ang nakalipas" },
        { "DateFutureSecond2", "2 segundo mula ngayon" },
        { "DatePastMinute1", "isang minutong nakalipas" },
        { "DateFutureMinute2", "2 minuto mula ngayon" },
        { "DatePastHour1", "isang oras ang nakalipas" },
        { "DateFutureHour2", "2 oras mula ngayon" },
        { "DatePastDay1", "kahapon" },
        { "DateFutureDay1", "bukas" },
        { "DatePastDay2", "2 isang araw ang nakalipas" },
        { "DateFutureDay2", "2 araw mula ngayon" },
        { "DatePastMonth1", "isang buwan ang nakalipas" },
        { "DateFutureMonth2", "2 buwan mula ngayon" },
        { "DatePastYear1", "isang taong nakalipas" },
        { "DateFutureYear2", "2 taon mula ngayon" },
        { "DateNow", "ngayon" },
        { "DateNever", "hindi kailanman" },
        { "SpanSecond1", "1 segundo" },
        { "SpanSecond2", "2 segundo" },
        { "SpanMinute1", "1 minuto" },
        { "SpanMinute2", "2 minuto" },
        { "SpanHour1", "1 oras" },
        { "SpanHour2", "2 oras" },
        { "SpanDay1", "1 araw" },
        { "SpanDay2", "2 araw" },
        { "SpanZero", "0 milliseconds" },
        { "SpanZeroWords", "walang oras" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("fil", caseName, expected);
}
