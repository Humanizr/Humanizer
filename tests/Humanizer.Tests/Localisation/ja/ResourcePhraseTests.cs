namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ja
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "1 秒前" },
        { "DateFutureSecond2", "2 秒後" },
        { "DatePastMinute1", "1 分前" },
        { "DateFutureMinute2", "2 分後" },
        { "DatePastHour1", "1 時間前" },
        { "DateFutureHour2", "2 時間後" },
        { "DatePastDay1", "昨日" },
        { "DateFutureDay1", "明日" },
        { "DatePastDay2", "2 日前" },
        { "DateFutureDay2", "2 日後" },
        { "DatePastMonth1", "先月" },
        { "DateFutureMonth2", "2 か月後" },
        { "DatePastYear1", "去年" },
        { "DateFutureYear2", "2 年後" },
        { "DateNow", "今" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 秒" },
        { "SpanSecond2", "2 秒" },
        { "SpanMinute1", "1 分" },
        { "SpanMinute2", "2 分" },
        { "SpanHour1", "1 時間" },
        { "SpanHour2", "2 時間" },
        { "SpanDay1", "1 日" },
        { "SpanDay2", "2 日" },
        { "SpanZero", "0 ミリ秒" },
        { "SpanZeroWords", "0 秒" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ja", caseName, expected);
}
