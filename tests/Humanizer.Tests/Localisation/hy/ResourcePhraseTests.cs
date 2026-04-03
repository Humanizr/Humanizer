namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hy
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "մեկ վայրկյան առաջ" },
        { 2, TimeUnit.Second, Tense.Future, "2 վայրկյանից" },
        { 1, TimeUnit.Minute, Tense.Past, "մեկ րոպե առաջ" },
        { 2, TimeUnit.Minute, Tense.Future, "2 րոպեից" },
        { 1, TimeUnit.Hour, Tense.Past, "մեկ ժամ առաջ" },
        { 2, TimeUnit.Hour, Tense.Future, "2 ժամից" },
        { 1, TimeUnit.Day, Tense.Past, "երեկ" },
        { 1, TimeUnit.Day, Tense.Future, "վաղը" },
        { 2, TimeUnit.Day, Tense.Past, "2 օր առաջ" },
        { 2, TimeUnit.Day, Tense.Future, "2 օրից" },
        { 1, TimeUnit.Month, Tense.Past, "մեկ ամիս առաջ" },
        { 2, TimeUnit.Month, Tense.Future, "2 ամսից" },
        { 1, TimeUnit.Year, Tense.Past, "մեկ տարի առաջ" },
        { 2, TimeUnit.Year, Tense.Future, "2 տարուց" },
        { 0, TimeUnit.Second, Tense.Future, "հիմա" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "մեկ վայրկյան" },
        { 2, TimeUnit.Second, false, "2 վայրկյան" },
        { 1, TimeUnit.Minute, false, "մեկ րոպե" },
        { 2, TimeUnit.Minute, false, "2 րոպե" },
        { 1, TimeUnit.Hour, false, "մեկ ժամ" },
        { 2, TimeUnit.Hour, false, "2 ժամ" },
        { 1, TimeUnit.Day, false, "մեկ օր" },
        { 2, TimeUnit.Day, false, "2 օր" },
        { 0, TimeUnit.Millisecond, false, "0 միլիվայրկյան" },
        { 0, TimeUnit.Millisecond, true, "ժամանակը բացակայում է" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("hy", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("hy", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("hy", unit, timeUnit, toWords, expected);
}
