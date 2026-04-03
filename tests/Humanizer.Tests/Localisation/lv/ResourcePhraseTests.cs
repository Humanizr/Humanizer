namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lv
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "pirms sekundes" },
        { "DateFutureSecond2", "pēc 2 sekundēm" },
        { "DatePastMinute1", "pirms minūtes" },
        { "DateFutureMinute2", "pēc 2 minūtēm" },
        { "DatePastHour1", "pirms stundas" },
        { "DateFutureHour2", "pēc 2 stundām" },
        { "DatePastDay1", "vakardien" },
        { "DateFutureDay1", "rītdien" },
        { "DatePastDay2", "pirms 2 dienām" },
        { "DateFutureDay2", "pēc 2 dienām" },
        { "DatePastMonth1", "pirms mēneša" },
        { "DateFutureMonth2", "pēc 2 mēnešiem" },
        { "DatePastYear1", "pirms gada" },
        { "DateFutureYear2", "pēc 2 gadiem" },
        { "DateNow", "tagad" },
        { "DateNever", "nekad" },
        { "SpanSecond1", "1 sekunde" },
        { "SpanSecond2", "2 sekundes" },
        { "SpanMinute1", "1 minūte" },
        { "SpanMinute2", "2 minūtes" },
        { "SpanHour1", "1 stunda" },
        { "SpanHour2", "2 stundas" },
        { "SpanDay1", "1 diena" },
        { "SpanDay2", "2 dienas" },
        { "SpanZero", "0 milisekundes" },
        { "SpanZeroWords", "bez laika" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("lv", caseName, expected);
}
