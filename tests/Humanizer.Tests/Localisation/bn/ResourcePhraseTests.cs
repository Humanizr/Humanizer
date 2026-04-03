namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_bn
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "এক সেকেন্ড আগে" },
        { 2, TimeUnit.Second, Tense.Future, "2 সেকেন্ড পর" },
        { 1, TimeUnit.Minute, Tense.Past, "এক মিনিট আগে" },
        { 2, TimeUnit.Minute, Tense.Future, "2 মিনিট পর" },
        { 1, TimeUnit.Hour, Tense.Past, "এক ঘণ্টা আগে" },
        { 2, TimeUnit.Hour, Tense.Future, "2 ঘণ্টা পর" },
        { 1, TimeUnit.Day, Tense.Past, "গতকাল" },
        { 1, TimeUnit.Day, Tense.Future, "আগামিকাল" },
        { 2, TimeUnit.Day, Tense.Past, "2 দিন আগে" },
        { 2, TimeUnit.Day, Tense.Future, "2 দিন পর" },
        { 1, TimeUnit.Month, Tense.Past, "এক মাস আগে" },
        { 2, TimeUnit.Month, Tense.Future, "2 মাস পর" },
        { 1, TimeUnit.Year, Tense.Past, "এক বছর আগে" },
        { 2, TimeUnit.Year, Tense.Future, "2 বছর পর" },
        { 0, TimeUnit.Second, Tense.Future, "এখন" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "এক সেকেন্ড" },
        { 2, TimeUnit.Second, false, "2 সেকেন্ড" },
        { 1, TimeUnit.Minute, false, "এক মিনিট" },
        { 2, TimeUnit.Minute, false, "2 মিনিট" },
        { 1, TimeUnit.Hour, false, "এক ঘণ্টা" },
        { 2, TimeUnit.Hour, false, "2 ঘণ্টা" },
        { 1, TimeUnit.Day, false, "এক দিন" },
        { 2, TimeUnit.Day, false, "2 দিন" },
        { 0, TimeUnit.Millisecond, false, "0 মিলিসেকেন্ড" },
        { 0, TimeUnit.Millisecond, true, "শূন্য সময়" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("bn", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("bn", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("bn", unit, timeUnit, toWords, expected);
}
