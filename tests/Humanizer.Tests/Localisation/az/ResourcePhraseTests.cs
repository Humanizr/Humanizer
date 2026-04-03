namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_az
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "bir saniyə əvvəl" },
        { 2, TimeUnit.Second, Tense.Future, "2 saniyə sonra" },
        { 1, TimeUnit.Minute, Tense.Past, "bir dəqiqə əvvəl" },
        { 2, TimeUnit.Minute, Tense.Future, "2 dəqiqə sonra" },
        { 1, TimeUnit.Hour, Tense.Past, "bir saat əvvəl" },
        { 2, TimeUnit.Hour, Tense.Future, "2 saat sonra" },
        { 1, TimeUnit.Day, Tense.Past, "dünən" },
        { 1, TimeUnit.Day, Tense.Future, "sabah" },
        { 2, TimeUnit.Day, Tense.Past, "2 gün əvvəl" },
        { 2, TimeUnit.Day, Tense.Future, "2 gün sonra" },
        { 1, TimeUnit.Month, Tense.Past, "bir ay əvvəl" },
        { 2, TimeUnit.Month, Tense.Future, "2 ay sonra" },
        { 1, TimeUnit.Year, Tense.Past, "bir il əvvəl" },
        { 2, TimeUnit.Year, Tense.Future, "2 il sonra" },
        { 0, TimeUnit.Second, Tense.Future, "indi" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 saniyə" },
        { 2, TimeUnit.Second, false, "2 saniyə" },
        { 1, TimeUnit.Minute, false, "1 dəqiqə" },
        { 2, TimeUnit.Minute, false, "2 dəqiqə" },
        { 1, TimeUnit.Hour, false, "1 saat" },
        { 2, TimeUnit.Hour, false, "2 saat" },
        { 1, TimeUnit.Day, false, "1 gün" },
        { 2, TimeUnit.Day, false, "2 gün" },
        { 0, TimeUnit.Millisecond, false, "0 millisaniyə" },
        { 0, TimeUnit.Millisecond, true, "zaman fərqi yoxdur" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("az", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("az", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("az", unit, timeUnit, toWords, expected);
}
