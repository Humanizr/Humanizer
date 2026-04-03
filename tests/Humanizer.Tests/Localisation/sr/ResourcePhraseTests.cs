namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sr
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "пре секунд" },
        { 2, TimeUnit.Second, Tense.Future, "за 2 секунде" },
        { 1, TimeUnit.Minute, Tense.Past, "пре минут" },
        { 2, TimeUnit.Minute, Tense.Future, "за 2 минута" },
        { 1, TimeUnit.Hour, Tense.Past, "пре сат времена" },
        { 2, TimeUnit.Hour, Tense.Future, "за 2 сата" },
        { 1, TimeUnit.Day, Tense.Past, "јуче" },
        { 1, TimeUnit.Day, Tense.Future, "сутра" },
        { 2, TimeUnit.Day, Tense.Past, "пре 2 дана" },
        { 2, TimeUnit.Day, Tense.Future, "за 2 дана" },
        { 1, TimeUnit.Month, Tense.Past, "пре месец дана" },
        { 2, TimeUnit.Month, Tense.Future, "за 2 месеца" },
        { 1, TimeUnit.Year, Tense.Past, "пре годину дана" },
        { 2, TimeUnit.Year, Tense.Future, "за 2 године" },
        { 0, TimeUnit.Second, Tense.Future, "сада" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 секунда" },
        { 2, TimeUnit.Second, false, "2 секунде" },
        { 1, TimeUnit.Minute, false, "1 минут" },
        { 2, TimeUnit.Minute, false, "2 минута" },
        { 1, TimeUnit.Hour, false, "1 сат" },
        { 2, TimeUnit.Hour, false, "2 сата" },
        { 1, TimeUnit.Day, false, "1 дан" },
        { 2, TimeUnit.Day, false, "2 дана" },
        { 0, TimeUnit.Millisecond, false, "0 милисекунди" },
        { 0, TimeUnit.Millisecond, true, "без протеклог времена" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("sr", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("sr", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("sr", unit, timeUnit, toWords, expected);
}
