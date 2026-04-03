namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_el
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "πριν από ένα δευτερόλεπτο" },
        { 2, TimeUnit.Second, Tense.Future, "2 δευτερόλεπτα από τώρα" },
        { 1, TimeUnit.Minute, Tense.Past, "πριν από ένα λεπτό" },
        { 2, TimeUnit.Minute, Tense.Future, "2 λεπτά από τώρα" },
        { 1, TimeUnit.Hour, Tense.Past, "πριν από μία ώρα" },
        { 2, TimeUnit.Hour, Tense.Future, "2 ώρες από τώρα" },
        { 1, TimeUnit.Day, Tense.Past, "χθες" },
        { 1, TimeUnit.Day, Tense.Future, "αύριο" },
        { 2, TimeUnit.Day, Tense.Past, "πριν από 2 ημέρες" },
        { 2, TimeUnit.Day, Tense.Future, "2 ημέρες από τώρα" },
        { 1, TimeUnit.Month, Tense.Past, "πριν από έναν μήνα" },
        { 2, TimeUnit.Month, Tense.Future, "2 μήνες από τώρα" },
        { 1, TimeUnit.Year, Tense.Past, "πριν από έναν χρόνο" },
        { 2, TimeUnit.Year, Tense.Future, "2 χρόνια από τώρα" },
        { 0, TimeUnit.Second, Tense.Future, "τώρα" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 δευτερόλεπτο" },
        { 2, TimeUnit.Second, false, "2 δευτερόλεπτα" },
        { 1, TimeUnit.Minute, false, "1 λεπτό" },
        { 2, TimeUnit.Minute, false, "2 λεπτά" },
        { 1, TimeUnit.Hour, false, "1 ώρα" },
        { 2, TimeUnit.Hour, false, "2 ώρες" },
        { 1, TimeUnit.Day, false, "1 μέρα" },
        { 2, TimeUnit.Day, false, "2 μέρες" },
        { 0, TimeUnit.Millisecond, false, "0 χιλιοστά του δευτερολέπτου" },
        { 0, TimeUnit.Millisecond, true, "μηδέν χρόνος" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("el", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("el", "ποτέ");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("el", unit, timeUnit, toWords, expected);
}
