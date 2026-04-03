namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sl
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "pred eno sekundo" },
        { 2, TimeUnit.Second, Tense.Future, "čez 2 sekundi" },
        { 1, TimeUnit.Minute, Tense.Past, "pred eno minuto" },
        { 2, TimeUnit.Minute, Tense.Future, "čez 2 minuti" },
        { 1, TimeUnit.Hour, Tense.Past, "pred eno uro" },
        { 2, TimeUnit.Hour, Tense.Future, "čez 2 uri" },
        { 1, TimeUnit.Day, Tense.Past, "včeraj" },
        { 1, TimeUnit.Day, Tense.Future, "jutri" },
        { 2, TimeUnit.Day, Tense.Past, "pred 2 dnevoma" },
        { 2, TimeUnit.Day, Tense.Future, "čez 2 dni" },
        { 1, TimeUnit.Month, Tense.Past, "pred enim mesecem" },
        { 2, TimeUnit.Month, Tense.Future, "čez 2 meseca" },
        { 1, TimeUnit.Year, Tense.Past, "pred enim letom" },
        { 2, TimeUnit.Year, Tense.Future, "čez 2 leti" },
        { 0, TimeUnit.Second, Tense.Future, "sedaj" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekunda" },
        { 2, TimeUnit.Second, false, "2 sekundi" },
        { 1, TimeUnit.Minute, false, "1 minuta" },
        { 2, TimeUnit.Minute, false, "2 minuti" },
        { 1, TimeUnit.Hour, false, "1 ura" },
        { 2, TimeUnit.Hour, false, "2 uri" },
        { 1, TimeUnit.Day, false, "1 dan" },
        { 2, TimeUnit.Day, false, "2 dneva" },
        { 0, TimeUnit.Millisecond, false, "0 milisekund" },
        { 0, TimeUnit.Millisecond, true, "nič časa" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("sl", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("sl", "nikoli");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("sl", unit, timeUnit, toWords, expected);
}
