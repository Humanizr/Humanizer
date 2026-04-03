namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_zh_CN
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "1 秒钟前" },
        { 2, TimeUnit.Second, Tense.Future, "2 秒钟后" },
        { 1, TimeUnit.Minute, Tense.Past, "1 分钟前" },
        { 2, TimeUnit.Minute, Tense.Future, "2 分钟后" },
        { 1, TimeUnit.Hour, Tense.Past, "1 小时前" },
        { 2, TimeUnit.Hour, Tense.Future, "2 小时后" },
        { 1, TimeUnit.Day, Tense.Past, "昨天" },
        { 1, TimeUnit.Day, Tense.Future, "明天" },
        { 2, TimeUnit.Day, Tense.Past, "2 天前" },
        { 2, TimeUnit.Day, Tense.Future, "2 天后" },
        { 1, TimeUnit.Month, Tense.Past, "1 个月前" },
        { 2, TimeUnit.Month, Tense.Future, "2 个月后" },
        { 1, TimeUnit.Year, Tense.Past, "去年" },
        { 2, TimeUnit.Year, Tense.Future, "2 年后" },
        { 0, TimeUnit.Second, Tense.Future, "现在" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 秒" },
        { 2, TimeUnit.Second, false, "2 秒" },
        { 1, TimeUnit.Minute, false, "1 分" },
        { 2, TimeUnit.Minute, false, "2 分" },
        { 1, TimeUnit.Hour, false, "1 小时" },
        { 2, TimeUnit.Hour, false, "2 小时" },
        { 1, TimeUnit.Day, false, "1 天" },
        { 2, TimeUnit.Day, false, "2 天" },
        { 0, TimeUnit.Millisecond, false, "0 毫秒" },
        { 0, TimeUnit.Millisecond, true, "没有时间" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("zh-CN", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("zh-CN", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("zh-CN", unit, timeUnit, toWords, expected);
}
