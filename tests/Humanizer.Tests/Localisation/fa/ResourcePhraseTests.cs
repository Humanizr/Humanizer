namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fa
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "یک ثانیه پیش" },
        { 2, TimeUnit.Second, Tense.Future, "2 ثانیه بعد" },
        { 1, TimeUnit.Minute, Tense.Past, "یک دقیقه پیش" },
        { 2, TimeUnit.Minute, Tense.Future, "2 دقیقه بعد" },
        { 1, TimeUnit.Hour, Tense.Past, "یک ساعت پیش" },
        { 2, TimeUnit.Hour, Tense.Future, "2 ساعت بعد" },
        { 1, TimeUnit.Day, Tense.Past, "دیروز" },
        { 1, TimeUnit.Day, Tense.Future, "فردا" },
        { 2, TimeUnit.Day, Tense.Past, "2 روز پیش" },
        { 2, TimeUnit.Day, Tense.Future, "2 روز بعد" },
        { 1, TimeUnit.Month, Tense.Past, "یک ماه پیش" },
        { 2, TimeUnit.Month, Tense.Future, "2 ماه بعد" },
        { 1, TimeUnit.Year, Tense.Past, "یک سال پیش" },
        { 2, TimeUnit.Year, Tense.Future, "2 سال بعد" },
        { 0, TimeUnit.Second, Tense.Future, "الآن" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "یک ثانیه" },
        { 2, TimeUnit.Second, false, "2 ثانیه" },
        { 1, TimeUnit.Minute, false, "یک دقیقه" },
        { 2, TimeUnit.Minute, false, "2 دقیقه" },
        { 1, TimeUnit.Hour, false, "یک ساعت" },
        { 2, TimeUnit.Hour, false, "2 ساعت" },
        { 1, TimeUnit.Day, false, "یک روز" },
        { 2, TimeUnit.Day, false, "2 روز" },
        { 0, TimeUnit.Millisecond, false, "0 میلی ثانیه" },
        { 0, TimeUnit.Millisecond, true, "الآن" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("fa", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("fa", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("fa", unit, timeUnit, toWords, expected);
}
