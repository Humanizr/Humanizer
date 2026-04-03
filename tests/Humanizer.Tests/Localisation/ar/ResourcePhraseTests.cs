namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ar
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "منذ ثانية واحدة" },
        { 2, TimeUnit.Second, Tense.Future, "في غضون ثانيتين من الآن" },
        { 1, TimeUnit.Minute, Tense.Past, "منذ دقيقة واحدة" },
        { 2, TimeUnit.Minute, Tense.Future, "في غضون دقيقتين من الآن" },
        { 1, TimeUnit.Hour, Tense.Past, "منذ ساعة واحدة" },
        { 2, TimeUnit.Hour, Tense.Future, "في غضون ساعتين من الآن" },
        { 1, TimeUnit.Day, Tense.Past, "أمس" },
        { 1, TimeUnit.Day, Tense.Future, "في غضون يوم واحد من الآن" },
        { 2, TimeUnit.Day, Tense.Past, "منذ يومين" },
        { 2, TimeUnit.Day, Tense.Future, "في غضون يومين من الآن" },
        { 1, TimeUnit.Month, Tense.Past, "منذ شهر واحد" },
        { 2, TimeUnit.Month, Tense.Future, "في غضون شهرين من الآن" },
        { 1, TimeUnit.Year, Tense.Past, "العام السابق" },
        { 2, TimeUnit.Year, Tense.Future, "في غضون سنتين من الآن" },
        { 0, TimeUnit.Second, Tense.Future, "الآن" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "ثانية واحدة" },
        { 2, TimeUnit.Second, false, "ثانيتين" },
        { 1, TimeUnit.Minute, false, "دقيقة واحدة" },
        { 2, TimeUnit.Minute, false, "دقيقتين" },
        { 1, TimeUnit.Hour, false, "ساعة واحدة" },
        { 2, TimeUnit.Hour, false, "ساعتين" },
        { 1, TimeUnit.Day, false, "يوم واحد" },
        { 2, TimeUnit.Day, false, "يومين" },
        { 0, TimeUnit.Millisecond, false, "0 جزء من الثانية" },
        { 0, TimeUnit.Millisecond, true, "حالاً" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ar", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ar", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ar", unit, timeUnit, toWords, expected);
}
