namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_mt
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "sekonda ilu" },
        { 2, TimeUnit.Second, Tense.Future, "2 sekondi oħra" },
        { 1, TimeUnit.Minute, Tense.Past, "minuta ilu" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minuti oħra" },
        { 1, TimeUnit.Hour, Tense.Past, "siegħa ilu" },
        { 2, TimeUnit.Hour, Tense.Future, "sagħtejn oħra" },
        { 1, TimeUnit.Day, Tense.Past, "il-bieraħ" },
        { 1, TimeUnit.Day, Tense.Future, "għada" },
        { 2, TimeUnit.Day, Tense.Past, "jumejn ilu" },
        { 2, TimeUnit.Day, Tense.Future, "pitgħada" },
        { 1, TimeUnit.Month, Tense.Past, "xahar ilu" },
        { 2, TimeUnit.Month, Tense.Future, "xahrejn oħra" },
        { 1, TimeUnit.Year, Tense.Past, "sena ilu" },
        { 2, TimeUnit.Year, Tense.Future, "sentejn oħra" },
        { 0, TimeUnit.Second, Tense.Future, "issa" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "sekonda" },
        { 2, TimeUnit.Second, false, "2 sekondi" },
        { 1, TimeUnit.Minute, false, "minuta" },
        { 2, TimeUnit.Minute, false, "2 minuti" },
        { 1, TimeUnit.Hour, false, "siegħa" },
        { 2, TimeUnit.Hour, false, "sagħtejn" },
        { 1, TimeUnit.Day, false, "ġurnata" },
        { 2, TimeUnit.Day, false, "jumejn" },
        { 0, TimeUnit.Millisecond, false, "0 millisekondi" },
        { 0, TimeUnit.Millisecond, true, "xejn" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("mt", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("mt", "qatt");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("mt", unit, timeUnit, toWords, expected);
}
