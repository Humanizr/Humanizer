namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lb
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "virun enger Sekonn" },
        { 2, TimeUnit.Second, Tense.Future, "an 2 Sekonnen" },
        { 1, TimeUnit.Minute, Tense.Past, "virun enger Minutt" },
        { 2, TimeUnit.Minute, Tense.Future, "an 2 Minutten" },
        { 1, TimeUnit.Hour, Tense.Past, "virun enger Stonn" },
        { 2, TimeUnit.Hour, Tense.Future, "an 2 Stonnen" },
        { 1, TimeUnit.Day, Tense.Past, "gëschter" },
        { 1, TimeUnit.Day, Tense.Future, "muer" },
        { 2, TimeUnit.Day, Tense.Past, "virgëschter" },
        { 2, TimeUnit.Day, Tense.Future, "iwwermuer" },
        { 1, TimeUnit.Month, Tense.Past, "virun engem Mount" },
        { 2, TimeUnit.Month, Tense.Future, "an 2 Méint" },
        { 1, TimeUnit.Year, Tense.Past, "virun engem Joer" },
        { 2, TimeUnit.Year, Tense.Future, "an 2 Joer" },
        { 0, TimeUnit.Second, Tense.Future, "elo" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 Sekonn" },
        { 2, TimeUnit.Second, false, "2 Sekonnen" },
        { 1, TimeUnit.Minute, false, "1 Minutt" },
        { 2, TimeUnit.Minute, false, "2 Minutten" },
        { 1, TimeUnit.Hour, false, "1 Stonn" },
        { 2, TimeUnit.Hour, false, "2 Stonnen" },
        { 1, TimeUnit.Day, false, "1 Dag" },
        { 2, TimeUnit.Day, false, "2 Deeg" },
        { 0, TimeUnit.Millisecond, false, "0 Millisekonnen" },
        { 0, TimeUnit.Millisecond, true, "Keng Zäit" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("lb", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("lb", "ni");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("lb", unit, timeUnit, toWords, expected);
}
