namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_nb
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "ett sekund siden" },
        { 2, TimeUnit.Second, Tense.Future, "2 sekunder fra nå" },
        { 1, TimeUnit.Minute, Tense.Past, "ett minutt siden" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minutter fra nå" },
        { 1, TimeUnit.Hour, Tense.Past, "en time siden" },
        { 2, TimeUnit.Hour, Tense.Future, "2 timer fra nå" },
        { 1, TimeUnit.Day, Tense.Past, "i går" },
        { 1, TimeUnit.Day, Tense.Future, "i morgen" },
        { 2, TimeUnit.Day, Tense.Past, "2 dager siden" },
        { 2, TimeUnit.Day, Tense.Future, "2 dager fra nå" },
        { 1, TimeUnit.Month, Tense.Past, "en måned siden" },
        { 2, TimeUnit.Month, Tense.Future, "2 måneder fra nå" },
        { 1, TimeUnit.Year, Tense.Past, "ett år siden" },
        { 2, TimeUnit.Year, Tense.Future, "2 år fra nå" },
        { 0, TimeUnit.Second, Tense.Future, "nå" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekund" },
        { 2, TimeUnit.Second, false, "2 sekunder" },
        { 1, TimeUnit.Minute, false, "1 minutt" },
        { 2, TimeUnit.Minute, false, "2 minutter" },
        { 1, TimeUnit.Hour, false, "1 time" },
        { 2, TimeUnit.Hour, false, "2 timer" },
        { 1, TimeUnit.Day, false, "1 dag" },
        { 2, TimeUnit.Day, false, "2 dager" },
        { 0, TimeUnit.Millisecond, false, "0 millisekunder" },
        { 0, TimeUnit.Millisecond, true, "ingen tid" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("nb", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("nb", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("nb", unit, timeUnit, toWords, expected);
}
