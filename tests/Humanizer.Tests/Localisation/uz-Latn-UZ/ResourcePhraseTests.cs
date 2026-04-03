namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uz_Latn_UZ
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "bir soniya avval" },
        { 2, TimeUnit.Second, Tense.Future, "2 sekunddan so`ng" },
        { 1, TimeUnit.Minute, Tense.Past, "bir daqiqa avval" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minutdan so`ng" },
        { 1, TimeUnit.Hour, Tense.Past, "bir soat avval" },
        { 2, TimeUnit.Hour, Tense.Future, "2 soatdan so`ng" },
        { 1, TimeUnit.Day, Tense.Past, "kecha" },
        { 1, TimeUnit.Day, Tense.Future, "ertaga" },
        { 2, TimeUnit.Day, Tense.Past, "2 kun avval" },
        { 2, TimeUnit.Day, Tense.Future, "2 kundan so`ng" },
        { 1, TimeUnit.Month, Tense.Past, "bir oy avval" },
        { 2, TimeUnit.Month, Tense.Future, "2 oydan so`ng" },
        { 1, TimeUnit.Year, Tense.Past, "bir yil avval" },
        { 2, TimeUnit.Year, Tense.Future, "2 yildan so`ng" },
        { 0, TimeUnit.Second, Tense.Future, "hozir" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekund" },
        { 2, TimeUnit.Second, false, "2 sekund" },
        { 1, TimeUnit.Minute, false, "1 minut" },
        { 2, TimeUnit.Minute, false, "2 minut" },
        { 1, TimeUnit.Hour, false, "1 soat" },
        { 2, TimeUnit.Hour, false, "2 soat" },
        { 1, TimeUnit.Day, false, "1 kun" },
        { 2, TimeUnit.Day, false, "2 kun" },
        { 0, TimeUnit.Millisecond, false, "0 millisekund" },
        { 0, TimeUnit.Millisecond, true, "vaqt yo`q" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("uz-Latn-UZ", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("uz-Latn-UZ", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("uz-Latn-UZ", unit, timeUnit, toWords, expected);
}
