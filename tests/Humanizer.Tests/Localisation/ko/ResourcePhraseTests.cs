namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ko
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "1초 전" },
        { 2, TimeUnit.Second, Tense.Future, "2초 후" },
        { 1, TimeUnit.Minute, Tense.Past, "1분 전" },
        { 2, TimeUnit.Minute, Tense.Future, "2분 후" },
        { 1, TimeUnit.Hour, Tense.Past, "1시간 전" },
        { 2, TimeUnit.Hour, Tense.Future, "2시간 후" },
        { 1, TimeUnit.Day, Tense.Past, "어제" },
        { 1, TimeUnit.Day, Tense.Future, "내일" },
        { 2, TimeUnit.Day, Tense.Past, "2일 전" },
        { 2, TimeUnit.Day, Tense.Future, "2일 후" },
        { 1, TimeUnit.Month, Tense.Past, "1개월 전" },
        { 2, TimeUnit.Month, Tense.Future, "2개월 후" },
        { 1, TimeUnit.Year, Tense.Past, "1년 전" },
        { 2, TimeUnit.Year, Tense.Future, "2년 후" },
        { 0, TimeUnit.Second, Tense.Future, "지금" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1초" },
        { 2, TimeUnit.Second, false, "2초" },
        { 1, TimeUnit.Minute, false, "1분" },
        { 2, TimeUnit.Minute, false, "2분" },
        { 1, TimeUnit.Hour, false, "1시간" },
        { 2, TimeUnit.Hour, false, "2시간" },
        { 1, TimeUnit.Day, false, "1일" },
        { 2, TimeUnit.Day, false, "2일" },
        { 0, TimeUnit.Millisecond, false, "0밀리초" },
        { 0, TimeUnit.Millisecond, true, "방금" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ko", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ko", "사용 안 함");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ko", unit, timeUnit, toWords, expected);
}
