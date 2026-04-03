namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_af
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "1 sekonde terug" },
        { 2, TimeUnit.Second, Tense.Future, "oor 2 sekondes" },
        { 1, TimeUnit.Minute, Tense.Past, "1 minuut terug" },
        { 2, TimeUnit.Minute, Tense.Future, "oor 2 minute" },
        { 1, TimeUnit.Hour, Tense.Past, "1 uur terug" },
        { 2, TimeUnit.Hour, Tense.Future, "oor 2 ure" },
        { 1, TimeUnit.Day, Tense.Past, "gister" },
        { 1, TimeUnit.Day, Tense.Future, "môre" },
        { 2, TimeUnit.Day, Tense.Past, "2 dae gelede" },
        { 2, TimeUnit.Day, Tense.Future, "oor 2 dae" },
        { 1, TimeUnit.Month, Tense.Past, "1 maand gelede" },
        { 2, TimeUnit.Month, Tense.Future, "oor 2 maande" },
        { 1, TimeUnit.Year, Tense.Past, "1 jaar gelede" },
        { 2, TimeUnit.Year, Tense.Future, "oor 2 jaar" },
        { 0, TimeUnit.Second, Tense.Future, "nou" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 sekond" },
        { 2, TimeUnit.Second, false, "2 sekondes" },
        { 1, TimeUnit.Minute, false, "1 minuut" },
        { 2, TimeUnit.Minute, false, "2 minute" },
        { 1, TimeUnit.Hour, false, "1 uur" },
        { 2, TimeUnit.Hour, false, "2 ure" },
        { 1, TimeUnit.Day, false, "1 dag" },
        { 2, TimeUnit.Day, false, "2 dae" },
        { 0, TimeUnit.Millisecond, false, "0 millisekondes" },
        { 0, TimeUnit.Millisecond, true, "geen tyd" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("af", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("af", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("af", unit, timeUnit, toWords, expected);
}
