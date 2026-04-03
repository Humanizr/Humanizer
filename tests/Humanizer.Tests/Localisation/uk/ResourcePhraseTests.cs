namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uk
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "секунду тому" },
        { 2, TimeUnit.Second, Tense.Future, "через 2 секунди" },
        { 1, TimeUnit.Minute, Tense.Past, "хвилину тому" },
        { 2, TimeUnit.Minute, Tense.Future, "через 2 хвилини" },
        { 1, TimeUnit.Hour, Tense.Past, "годину тому" },
        { 2, TimeUnit.Hour, Tense.Future, "через 2 години" },
        { 1, TimeUnit.Day, Tense.Past, "вчора" },
        { 1, TimeUnit.Day, Tense.Future, "завтра" },
        { 2, TimeUnit.Day, Tense.Past, "2 дні тому" },
        { 2, TimeUnit.Day, Tense.Future, "через 2 дні" },
        { 1, TimeUnit.Month, Tense.Past, "місяць тому" },
        { 2, TimeUnit.Month, Tense.Future, "через 2 місяці" },
        { 1, TimeUnit.Year, Tense.Past, "рік тому" },
        { 2, TimeUnit.Year, Tense.Future, "через 2 роки" },
        { 0, TimeUnit.Second, Tense.Future, "зараз" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 секунда" },
        { 2, TimeUnit.Second, false, "2 секунди" },
        { 1, TimeUnit.Minute, false, "1 хвилина" },
        { 2, TimeUnit.Minute, false, "2 хвилини" },
        { 1, TimeUnit.Hour, false, "1 година" },
        { 2, TimeUnit.Hour, false, "2 години" },
        { 1, TimeUnit.Day, false, "1 день" },
        { 2, TimeUnit.Day, false, "2 дні" },
        { 0, TimeUnit.Millisecond, false, "0 мілісекунд" },
        { 0, TimeUnit.Millisecond, true, "без часу" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("uk", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("uk", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("uk", unit, timeUnit, toWords, expected);
}
