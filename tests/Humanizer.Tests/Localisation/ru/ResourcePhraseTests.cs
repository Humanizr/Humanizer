namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ru
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "секунду назад" },
        { 2, TimeUnit.Second, Tense.Future, "через 2 секунды" },
        { 1, TimeUnit.Minute, Tense.Past, "минуту назад" },
        { 2, TimeUnit.Minute, Tense.Future, "через 2 минуты" },
        { 1, TimeUnit.Hour, Tense.Past, "час назад" },
        { 2, TimeUnit.Hour, Tense.Future, "через 2 часа" },
        { 1, TimeUnit.Day, Tense.Past, "вчера" },
        { 1, TimeUnit.Day, Tense.Future, "завтра" },
        { 2, TimeUnit.Day, Tense.Past, "2 дня назад" },
        { 2, TimeUnit.Day, Tense.Future, "через 2 дня" },
        { 1, TimeUnit.Month, Tense.Past, "месяц назад" },
        { 2, TimeUnit.Month, Tense.Future, "через 2 месяца" },
        { 1, TimeUnit.Year, Tense.Past, "год назад" },
        { 2, TimeUnit.Year, Tense.Future, "через 2 года" },
        { 0, TimeUnit.Second, Tense.Future, "сейчас" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 секунда" },
        { 2, TimeUnit.Second, false, "2 секунды" },
        { 1, TimeUnit.Minute, false, "1 минута" },
        { 2, TimeUnit.Minute, false, "2 минуты" },
        { 1, TimeUnit.Hour, false, "1 час" },
        { 2, TimeUnit.Hour, false, "2 часа" },
        { 1, TimeUnit.Day, false, "1 день" },
        { 2, TimeUnit.Day, false, "2 дня" },
        { 0, TimeUnit.Millisecond, false, "0 миллисекунд" },
        { 0, TimeUnit.Millisecond, true, "нет времени" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ru", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ru", "никогда");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ru", unit, timeUnit, toWords, expected);
}
