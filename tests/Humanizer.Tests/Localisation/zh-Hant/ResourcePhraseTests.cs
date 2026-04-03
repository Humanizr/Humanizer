namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_zh_Hant
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "1 秒鐘前" },
        { "DateFutureSecond2", "2 秒鐘後" },
        { "DatePastMinute1", "1 分鐘前" },
        { "DateFutureMinute2", "2 分鐘後" },
        { "DatePastHour1", "1 小時前" },
        { "DateFutureHour2", "2 小時後" },
        { "DatePastDay1", "昨天" },
        { "DateFutureDay1", "明天" },
        { "DatePastDay2", "2 天前" },
        { "DateFutureDay2", "2 天後" },
        { "DatePastMonth1", "1 個月前" },
        { "DateFutureMonth2", "2 個月後" },
        { "DatePastYear1", "去年" },
        { "DateFutureYear2", "2 年後" },
        { "DateNow", "現在" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 秒" },
        { "SpanSecond2", "2 秒" },
        { "SpanMinute1", "1 分" },
        { "SpanMinute2", "2 分" },
        { "SpanHour1", "1 小時" },
        { "SpanHour2", "2 小時" },
        { "SpanDay1", "1 天" },
        { "SpanDay2", "2 天" },
        { "SpanZero", "0 毫秒" },
        { "SpanZeroWords", "沒有時間" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("zh-Hant", caseName, expected);
}
