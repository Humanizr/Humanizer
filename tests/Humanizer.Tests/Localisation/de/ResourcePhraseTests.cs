namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_de
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "vor einer Sekunde" },
        { 2, TimeUnit.Second, Tense.Future, "in 2 Sekunden" },
        { 1, TimeUnit.Minute, Tense.Past, "vor einer Minute" },
        { 2, TimeUnit.Minute, Tense.Future, "in 2 Minuten" },
        { 1, TimeUnit.Hour, Tense.Past, "vor einer Stunde" },
        { 2, TimeUnit.Hour, Tense.Future, "in 2 Stunden" },
        { 1, TimeUnit.Day, Tense.Past, "gestern" },
        { 1, TimeUnit.Day, Tense.Future, "morgen" },
        { 2, TimeUnit.Day, Tense.Past, "vor 2 Tagen" },
        { 2, TimeUnit.Day, Tense.Future, "in 2 Tagen" },
        { 1, TimeUnit.Month, Tense.Past, "vor einem Monat" },
        { 2, TimeUnit.Month, Tense.Future, "in 2 Monaten" },
        { 1, TimeUnit.Year, Tense.Past, "vor einem Jahr" },
        { 2, TimeUnit.Year, Tense.Future, "in 2 Jahren" },
        { 0, TimeUnit.Second, Tense.Future, "jetzt" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 Sekunde" },
        { 2, TimeUnit.Second, false, "2 Sekunden" },
        { 1, TimeUnit.Minute, false, "1 Minute" },
        { 2, TimeUnit.Minute, false, "2 Minuten" },
        { 1, TimeUnit.Hour, false, "1 Stunde" },
        { 2, TimeUnit.Hour, false, "2 Stunden" },
        { 1, TimeUnit.Day, false, "1 Tag" },
        { 2, TimeUnit.Day, false, "2 Tage" },
        { 0, TimeUnit.Millisecond, false, "0 Millisekunden" },
        { 0, TimeUnit.Millisecond, true, "Keine Zeit" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("de", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("de", "nie");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("de", unit, timeUnit, toWords, expected);
}
