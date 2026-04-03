namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_bg
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "преди секунда" },
        { 2, TimeUnit.Second, Tense.Future, "след 2 секунди" },
        { 1, TimeUnit.Minute, Tense.Past, "преди минута" },
        { 2, TimeUnit.Minute, Tense.Future, "след 2 минути" },
        { 1, TimeUnit.Hour, Tense.Past, "преди час" },
        { 2, TimeUnit.Hour, Tense.Future, "след 2 часа" },
        { 1, TimeUnit.Day, Tense.Past, "вчера" },
        { 1, TimeUnit.Day, Tense.Future, "утре" },
        { 2, TimeUnit.Day, Tense.Past, "преди 2 дена" },
        { 2, TimeUnit.Day, Tense.Future, "след 2 дена" },
        { 1, TimeUnit.Month, Tense.Past, "преди месец" },
        { 2, TimeUnit.Month, Tense.Future, "след 2 месеца" },
        { 1, TimeUnit.Year, Tense.Past, "преди година" },
        { 2, TimeUnit.Year, Tense.Future, "след 2 години" },
        { 0, TimeUnit.Second, Tense.Future, "сега" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 секунда" },
        { 2, TimeUnit.Second, false, "2 секунди" },
        { 1, TimeUnit.Minute, false, "1 минута" },
        { 2, TimeUnit.Minute, false, "2 минути" },
        { 1, TimeUnit.Hour, false, "1 час" },
        { 2, TimeUnit.Hour, false, "2 часа" },
        { 1, TimeUnit.Day, false, "1 ден" },
        { 2, TimeUnit.Day, false, "2 дена" },
        { 0, TimeUnit.Millisecond, false, "0 милисекунди" },
        { 0, TimeUnit.Millisecond, true, "няма време" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("bg", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("bg", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("bg", unit, timeUnit, toWords, expected);
}
