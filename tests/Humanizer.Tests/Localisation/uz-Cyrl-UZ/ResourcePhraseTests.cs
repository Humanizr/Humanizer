namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uz_Cyrl_UZ
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "бир сония аввал" },
        { 2, TimeUnit.Second, Tense.Future, "2 секунддан сўнг" },
        { 1, TimeUnit.Minute, Tense.Past, "бир дақиқа аввал" },
        { 2, TimeUnit.Minute, Tense.Future, "2 минутдан сўнг" },
        { 1, TimeUnit.Hour, Tense.Past, "бир соат аввал" },
        { 2, TimeUnit.Hour, Tense.Future, "2 соатдан сўнг" },
        { 1, TimeUnit.Day, Tense.Past, "кеча" },
        { 1, TimeUnit.Day, Tense.Future, "эртага" },
        { 2, TimeUnit.Day, Tense.Past, "2 кун аввал" },
        { 2, TimeUnit.Day, Tense.Future, "2 кундан сўнг" },
        { 1, TimeUnit.Month, Tense.Past, "бир ой аввал" },
        { 2, TimeUnit.Month, Tense.Future, "2 ойдан сўнг" },
        { 1, TimeUnit.Year, Tense.Past, "бир йил аввал" },
        { 2, TimeUnit.Year, Tense.Future, "2 йилдан сўнг" },
        { 0, TimeUnit.Second, Tense.Future, "ҳозир" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 секунд" },
        { 2, TimeUnit.Second, false, "2 секунд" },
        { 1, TimeUnit.Minute, false, "1 минут" },
        { 2, TimeUnit.Minute, false, "2 минут" },
        { 1, TimeUnit.Hour, false, "1 соат" },
        { 2, TimeUnit.Hour, false, "2 соат" },
        { 1, TimeUnit.Day, false, "1 кун" },
        { 2, TimeUnit.Day, false, "2 кун" },
        { 0, TimeUnit.Millisecond, false, "0 миллисекунд" },
        { 0, TimeUnit.Millisecond, true, "вақт йўқ" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("uz-Cyrl-UZ", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("uz-Cyrl-UZ", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("uz-Cyrl-UZ", unit, timeUnit, toWords, expected);
}
