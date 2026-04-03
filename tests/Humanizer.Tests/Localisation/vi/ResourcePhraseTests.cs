namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_vi
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "cách đây một giây" },
        { 2, TimeUnit.Second, Tense.Future, "2 giây nữa" },
        { 1, TimeUnit.Minute, Tense.Past, "cách đây một phút" },
        { 2, TimeUnit.Minute, Tense.Future, "2 phút nữa" },
        { 1, TimeUnit.Hour, Tense.Past, "cách đây một tiếng" },
        { 2, TimeUnit.Hour, Tense.Future, "2 tiếng nữa" },
        { 1, TimeUnit.Day, Tense.Past, "hôm qua" },
        { 1, TimeUnit.Day, Tense.Future, "ngày mai" },
        { 2, TimeUnit.Day, Tense.Past, "cách đây 2 ngày" },
        { 2, TimeUnit.Day, Tense.Future, "2 ngày nữa" },
        { 1, TimeUnit.Month, Tense.Past, "cách đây một tháng" },
        { 2, TimeUnit.Month, Tense.Future, "2 tháng nữa" },
        { 1, TimeUnit.Year, Tense.Past, "cách đây một năm" },
        { 2, TimeUnit.Year, Tense.Future, "2 năm nữa" },
        { 0, TimeUnit.Second, Tense.Future, "bây giờ" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 giây" },
        { 2, TimeUnit.Second, false, "2 giây" },
        { 1, TimeUnit.Minute, false, "1 phút" },
        { 2, TimeUnit.Minute, false, "2 phút" },
        { 1, TimeUnit.Hour, false, "1 giờ" },
        { 2, TimeUnit.Hour, false, "2 giờ" },
        { 1, TimeUnit.Day, false, "1 ngày" },
        { 2, TimeUnit.Day, false, "2 ngày" },
        { 0, TimeUnit.Millisecond, false, "0 phần ngàn giây" },
        { 0, TimeUnit.Millisecond, true, "không giờ" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("vi", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("vi", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("vi", unit, timeUnit, toWords, expected);
}
