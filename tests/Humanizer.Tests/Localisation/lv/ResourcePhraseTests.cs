namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lv
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "pirms sekundes" },
        { 2, TimeUnit.Second, Tense.Future, "pēc 2 sekundēm" },
        { 1, TimeUnit.Minute, Tense.Past, "pirms minūtes" },
        { 2, TimeUnit.Minute, Tense.Future, "pēc 2 minūtēm" },
        { 1, TimeUnit.Hour, Tense.Past, "pirms stundas" },
        { 2, TimeUnit.Hour, Tense.Future, "pēc 2 stundām" },
        { 1, TimeUnit.Day, Tense.Past, "vakardien" },
        { 1, TimeUnit.Day, Tense.Future, "rītdien" },
        { 2, TimeUnit.Day, Tense.Past, "pirms 2 dienām" },
        { 2, TimeUnit.Day, Tense.Future, "pēc 2 dienām" },
        { 1, TimeUnit.Month, Tense.Past, "pirms mēneša" },
        { 2, TimeUnit.Month, Tense.Future, "pēc 2 mēnešiem" },
        { 1, TimeUnit.Year, Tense.Past, "pirms gada" },
        { 2, TimeUnit.Year, Tense.Future, "pēc 2 gadiem" },
        { 0, TimeUnit.Second, Tense.Future, "tagad" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunde" },
        { 2, TimeUnit.Second, false, "2 sekundes" },
        { 1, TimeUnit.Minute, false, "1 minūte" },
        { 2, TimeUnit.Minute, false, "2 minūtes" },
        { 1, TimeUnit.Hour, false, "1 stunda" },
        { 2, TimeUnit.Hour, false, "2 stundas" },
        { 1, TimeUnit.Day, false, "1 diena" },
        { 2, TimeUnit.Day, false, "2 dienas" },
        { 0, TimeUnit.Millisecond, false, "0 milisekundes" },
        { 0, TimeUnit.Millisecond, true, "bez laika" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("lv", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("lv", "nekad");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("lv", unit, timeUnit, toWords, expected);
}
