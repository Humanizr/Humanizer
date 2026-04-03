namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_da
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "et sekund siden" },
        { 2, TimeUnit.Second, Tense.Future, "2 sekunder fra nu" },
        { 1, TimeUnit.Minute, Tense.Past, "et minut siden" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minutter fra nu" },
        { 1, TimeUnit.Hour, Tense.Past, "en time siden" },
        { 2, TimeUnit.Hour, Tense.Future, "2 timer fra nu" },
        { 1, TimeUnit.Day, Tense.Past, "i går" },
        { 1, TimeUnit.Day, Tense.Future, "i morgen" },
        { 2, TimeUnit.Day, Tense.Past, "2 dage siden" },
        { 2, TimeUnit.Day, Tense.Future, "2 dage fra nu" },
        { 1, TimeUnit.Month, Tense.Past, "en måned siden" },
        { 2, TimeUnit.Month, Tense.Future, "2 måneder fra nu" },
        { 1, TimeUnit.Year, Tense.Past, "et år siden" },
        { 2, TimeUnit.Year, Tense.Future, "2 år fra nu" },
        { 0, TimeUnit.Second, Tense.Future, "nu" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "et sekund" },
        { 2, TimeUnit.Second, false, "2 sekunder" },
        { 1, TimeUnit.Minute, false, "et minut" },
        { 2, TimeUnit.Minute, false, "2 minutter" },
        { 1, TimeUnit.Hour, false, "en time" },
        { 2, TimeUnit.Hour, false, "2 timer" },
        { 1, TimeUnit.Day, false, "en dag" },
        { 2, TimeUnit.Day, false, "2 dage" },
        { 0, TimeUnit.Millisecond, false, "0 millisekunder" },
        { 0, TimeUnit.Millisecond, true, "ingen tid" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("da", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("da", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("da", unit, timeUnit, toWords, expected);
}
