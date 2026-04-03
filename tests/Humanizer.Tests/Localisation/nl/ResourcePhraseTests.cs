namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_nl
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "1 seconde geleden" },
        { 2, TimeUnit.Second, Tense.Future, "over 2 seconden" },
        { 1, TimeUnit.Minute, Tense.Past, "1 minuut geleden" },
        { 2, TimeUnit.Minute, Tense.Future, "over 2 minuten" },
        { 1, TimeUnit.Hour, Tense.Past, "1 uur geleden" },
        { 2, TimeUnit.Hour, Tense.Future, "over 2 uur" },
        { 1, TimeUnit.Day, Tense.Past, "gisteren" },
        { 1, TimeUnit.Day, Tense.Future, "morgen" },
        { 2, TimeUnit.Day, Tense.Past, "2 dagen geleden" },
        { 2, TimeUnit.Day, Tense.Future, "over 2 dagen" },
        { 1, TimeUnit.Month, Tense.Past, "1 maand geleden" },
        { 2, TimeUnit.Month, Tense.Future, "over 2 maanden" },
        { 1, TimeUnit.Year, Tense.Past, "1 jaar geleden" },
        { 2, TimeUnit.Year, Tense.Future, "over 2 jaar" },
        { 0, TimeUnit.Second, Tense.Future, "nu" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 seconde" },
        { 2, TimeUnit.Second, false, "2 seconden" },
        { 1, TimeUnit.Minute, false, "1 minuut" },
        { 2, TimeUnit.Minute, false, "2 minuten" },
        { 1, TimeUnit.Hour, false, "1 uur" },
        { 2, TimeUnit.Hour, false, "2 uur" },
        { 1, TimeUnit.Day, false, "1 dag" },
        { 2, TimeUnit.Day, false, "2 dagen" },
        { 0, TimeUnit.Millisecond, false, "0 milliseconden" },
        { 0, TimeUnit.Millisecond, true, "geen tijd" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("nl", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("nl", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("nl", unit, timeUnit, toWords, expected);
}
