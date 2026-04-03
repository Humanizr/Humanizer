namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_zh_Hans
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "1 秒钟前" },
        { "DateFutureSecond2", "2 秒钟后" },
        { "DatePastMinute1", "1 分钟前" },
        { "DateFutureMinute2", "2 分钟后" },
        { "DatePastHour1", "1 小时前" },
        { "DateFutureHour2", "2 小时后" },
        { "DatePastDay1", "昨天" },
        { "DateFutureDay1", "明天" },
        { "DatePastDay2", "2 天前" },
        { "DateFutureDay2", "2 天后" },
        { "DatePastMonth1", "1 个月前" },
        { "DateFutureMonth2", "2 个月后" },
        { "DatePastYear1", "去年" },
        { "DateFutureYear2", "2 年后" },
        { "DateNow", "现在" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 秒" },
        { "SpanSecond2", "2 秒" },
        { "SpanMinute1", "1 分" },
        { "SpanMinute2", "2 分" },
        { "SpanHour1", "1 小时" },
        { "SpanHour2", "2 小时" },
        { "SpanDay1", "1 天" },
        { "SpanDay2", "2 天" },
        { "SpanZero", "0 毫秒" },
        { "SpanZeroWords", "没有时间" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("zh-Hans", caseName, expected);
}
