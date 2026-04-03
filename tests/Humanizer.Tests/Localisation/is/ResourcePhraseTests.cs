namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_is
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "fyrir einni sekúndu" },
        { 2, TimeUnit.Second, Tense.Future, "eftir 2 sekúndur" },
        { 1, TimeUnit.Minute, Tense.Past, "fyrir einni mínútu" },
        { 2, TimeUnit.Minute, Tense.Future, "eftir 2 mínútur" },
        { 1, TimeUnit.Hour, Tense.Past, "fyrir einni klukkustund" },
        { 2, TimeUnit.Hour, Tense.Future, "eftir 2 klukkustundir" },
        { 1, TimeUnit.Day, Tense.Past, "í gær" },
        { 1, TimeUnit.Day, Tense.Future, "á morgun" },
        { 2, TimeUnit.Day, Tense.Past, "fyrir 2 dögum" },
        { 2, TimeUnit.Day, Tense.Future, "eftir 2 daga" },
        { 1, TimeUnit.Month, Tense.Past, "fyrir einum mánuði" },
        { 2, TimeUnit.Month, Tense.Future, "eftir 2 mánuði" },
        { 1, TimeUnit.Year, Tense.Past, "fyrir einu ári" },
        { 2, TimeUnit.Year, Tense.Future, "eftir 2 ár" },
        { 0, TimeUnit.Second, Tense.Future, "núna" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "ein sekúnda" },
        { 2, TimeUnit.Second, false, "2 sekúndur" },
        { 1, TimeUnit.Minute, false, "ein mínúta" },
        { 2, TimeUnit.Minute, false, "2 mínútur" },
        { 1, TimeUnit.Hour, false, "ein klukkustund" },
        { 2, TimeUnit.Hour, false, "2 klukkustundir" },
        { 1, TimeUnit.Day, false, "einn dagur" },
        { 2, TimeUnit.Day, false, "2 dagar" },
        { 0, TimeUnit.Millisecond, false, "0 millisekúndur" },
        { 0, TimeUnit.Millisecond, true, "engin stund" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("is", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("is", "aldrei");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("is", unit, timeUnit, toWords, expected);
}
