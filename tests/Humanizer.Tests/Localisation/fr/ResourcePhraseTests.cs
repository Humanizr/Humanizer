namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fr
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "il y a une seconde" },
        { 2, TimeUnit.Second, Tense.Future, "dans 2 secondes" },
        { 1, TimeUnit.Minute, Tense.Past, "il y a une minute" },
        { 2, TimeUnit.Minute, Tense.Future, "dans 2 minutes" },
        { 1, TimeUnit.Hour, Tense.Past, "il y a une heure" },
        { 2, TimeUnit.Hour, Tense.Future, "dans 2 heures" },
        { 1, TimeUnit.Day, Tense.Past, "hier" },
        { 1, TimeUnit.Day, Tense.Future, "demain" },
        { 2, TimeUnit.Day, Tense.Past, "avant-hier" },
        { 2, TimeUnit.Day, Tense.Future, "après-demain" },
        { 1, TimeUnit.Month, Tense.Past, "il y a un mois" },
        { 2, TimeUnit.Month, Tense.Future, "dans 2 mois" },
        { 1, TimeUnit.Year, Tense.Past, "il y a un an" },
        { 2, TimeUnit.Year, Tense.Future, "dans 2 ans" },
        { 0, TimeUnit.Second, Tense.Future, "maintenant" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 seconde" },
        { 2, TimeUnit.Second, false, "2 secondes" },
        { 1, TimeUnit.Minute, false, "1 minute" },
        { 2, TimeUnit.Minute, false, "2 minutes" },
        { 1, TimeUnit.Hour, false, "1 heure" },
        { 2, TimeUnit.Hour, false, "2 heures" },
        { 1, TimeUnit.Day, false, "1 jour" },
        { 2, TimeUnit.Day, false, "2 jours" },
        { 0, TimeUnit.Millisecond, false, "0 milliseconde" },
        { 0, TimeUnit.Millisecond, true, "temps nul" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("fr", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("fr", "jamais");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("fr", unit, timeUnit, toWords, expected);
}
