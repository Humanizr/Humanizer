namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ja
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "1 秒前" },
        { 2, TimeUnit.Second, Tense.Future, "2 秒後" },
        { 1, TimeUnit.Minute, Tense.Past, "1 分前" },
        { 2, TimeUnit.Minute, Tense.Future, "2 分後" },
        { 1, TimeUnit.Hour, Tense.Past, "1 時間前" },
        { 2, TimeUnit.Hour, Tense.Future, "2 時間後" },
        { 1, TimeUnit.Day, Tense.Past, "昨日" },
        { 1, TimeUnit.Day, Tense.Future, "明日" },
        { 2, TimeUnit.Day, Tense.Past, "2 日前" },
        { 2, TimeUnit.Day, Tense.Future, "2 日後" },
        { 1, TimeUnit.Month, Tense.Past, "先月" },
        { 2, TimeUnit.Month, Tense.Future, "2 か月後" },
        { 1, TimeUnit.Year, Tense.Past, "去年" },
        { 2, TimeUnit.Year, Tense.Future, "2 年後" },
        { 0, TimeUnit.Second, Tense.Future, "今" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 秒" },
        { 2, TimeUnit.Second, false, "2 秒" },
        { 1, TimeUnit.Minute, false, "1 分" },
        { 2, TimeUnit.Minute, false, "2 分" },
        { 1, TimeUnit.Hour, false, "1 時間" },
        { 2, TimeUnit.Hour, false, "2 時間" },
        { 1, TimeUnit.Day, false, "1 日" },
        { 2, TimeUnit.Day, false, "2 日" },
        { 0, TimeUnit.Millisecond, false, "0 ミリ秒" },
        { 0, TimeUnit.Millisecond, true, "0 秒" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ja", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ja", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ja", unit, timeUnit, toWords, expected);
}
