namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ko
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "1초 전" },
        { "DateFutureSecond2", "2초 후" },
        { "DatePastMinute1", "1분 전" },
        { "DateFutureMinute2", "2분 후" },
        { "DatePastHour1", "1시간 전" },
        { "DateFutureHour2", "2시간 후" },
        { "DatePastDay1", "어제" },
        { "DateFutureDay1", "내일" },
        { "DatePastDay2", "2일 전" },
        { "DateFutureDay2", "2일 후" },
        { "DatePastMonth1", "1개월 전" },
        { "DateFutureMonth2", "2개월 후" },
        { "DatePastYear1", "1년 전" },
        { "DateFutureYear2", "2년 후" },
        { "DateNow", "지금" },
        { "DateNever", "사용 안 함" },
        { "SpanSecond1", "1초" },
        { "SpanSecond2", "2초" },
        { "SpanMinute1", "1분" },
        { "SpanMinute2", "2분" },
        { "SpanHour1", "1시간" },
        { "SpanHour2", "2시간" },
        { "SpanDay1", "1일" },
        { "SpanDay2", "2일" },
        { "SpanZero", "0밀리초" },
        { "SpanZeroWords", "방금" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ko", caseName, expected);
}
