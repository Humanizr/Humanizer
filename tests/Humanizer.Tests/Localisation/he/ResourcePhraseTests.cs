namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_he
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "לפני שנייה" },
        { 2, TimeUnit.Second, Tense.Future, "בעוד 2 שניות" },
        { 1, TimeUnit.Minute, Tense.Past, "לפני דקה" },
        { 2, TimeUnit.Minute, Tense.Future, "בעוד 2 דקות" },
        { 1, TimeUnit.Hour, Tense.Past, "לפני שעה" },
        { 2, TimeUnit.Hour, Tense.Future, "בעוד שעתיים" },
        { 1, TimeUnit.Day, Tense.Past, "אתמול" },
        { 1, TimeUnit.Day, Tense.Future, "מחר" },
        { 2, TimeUnit.Day, Tense.Past, "לפני יומיים" },
        { 2, TimeUnit.Day, Tense.Future, "בעוד יומיים" },
        { 1, TimeUnit.Month, Tense.Past, "לפני חודש" },
        { 2, TimeUnit.Month, Tense.Future, "בעוד חודשיים" },
        { 1, TimeUnit.Year, Tense.Past, "לפני שנה" },
        { 2, TimeUnit.Year, Tense.Future, "בעוד שנתיים" },
        { 0, TimeUnit.Second, Tense.Future, "כעת" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "שנייה" },
        { 2, TimeUnit.Second, false, "שתי שניות" },
        { 1, TimeUnit.Minute, false, "דקה" },
        { 2, TimeUnit.Minute, false, "שתי דקות" },
        { 1, TimeUnit.Hour, false, "שעה" },
        { 2, TimeUnit.Hour, false, "שעתיים" },
        { 1, TimeUnit.Day, false, "יום" },
        { 2, TimeUnit.Day, false, "יומיים" },
        { 0, TimeUnit.Millisecond, false, "0 אלפיות שנייה" },
        { 0, TimeUnit.Millisecond, true, "אין זמן" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("he", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("he", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("he", unit, timeUnit, toWords, expected);
}
