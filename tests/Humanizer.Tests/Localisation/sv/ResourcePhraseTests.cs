namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sv
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "en sekund sedan" },
        { 2, TimeUnit.Second, Tense.Future, "om 2 sekunder" },
        { 1, TimeUnit.Minute, Tense.Past, "en minut sedan" },
        { 2, TimeUnit.Minute, Tense.Future, "om 2 minuter" },
        { 1, TimeUnit.Hour, Tense.Past, "en timme sedan" },
        { 2, TimeUnit.Hour, Tense.Future, "om 2 timmar" },
        { 1, TimeUnit.Day, Tense.Past, "igår" },
        { 1, TimeUnit.Day, Tense.Future, "i morgon" },
        { 2, TimeUnit.Day, Tense.Past, "för 2 dagar sedan" },
        { 2, TimeUnit.Day, Tense.Future, "om 2 dagar" },
        { 1, TimeUnit.Month, Tense.Past, "en månad sedan" },
        { 2, TimeUnit.Month, Tense.Future, "om 2 månader" },
        { 1, TimeUnit.Year, Tense.Past, "ett år sedan" },
        { 2, TimeUnit.Year, Tense.Future, "om 2 år" },
        { 0, TimeUnit.Second, Tense.Future, "nu" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekund" },
        { 2, TimeUnit.Second, false, "2 sekunder" },
        { 1, TimeUnit.Minute, false, "1 minut" },
        { 2, TimeUnit.Minute, false, "2 minuter" },
        { 1, TimeUnit.Hour, false, "1 timma" },
        { 2, TimeUnit.Hour, false, "2 timmar" },
        { 1, TimeUnit.Day, false, "1 dag" },
        { 2, TimeUnit.Day, false, "2 dagar" },
        { 0, TimeUnit.Millisecond, false, "0 millisekunder" },
        { 0, TimeUnit.Millisecond, true, "ingen tid" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("sv", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("sv", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("sv", unit, timeUnit, toWords, expected);
}
