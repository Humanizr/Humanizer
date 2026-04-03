namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_tr
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "bir saniye önce" },
        { 2, TimeUnit.Second, Tense.Future, "2 saniye sonra" },
        { 1, TimeUnit.Minute, Tense.Past, "bir dakika önce" },
        { 2, TimeUnit.Minute, Tense.Future, "2 dakika sonra" },
        { 1, TimeUnit.Hour, Tense.Past, "bir saat önce" },
        { 2, TimeUnit.Hour, Tense.Future, "2 saat sonra" },
        { 1, TimeUnit.Day, Tense.Past, "dün" },
        { 1, TimeUnit.Day, Tense.Future, "yarın" },
        { 2, TimeUnit.Day, Tense.Past, "2 gün önce" },
        { 2, TimeUnit.Day, Tense.Future, "2 gün sonra" },
        { 1, TimeUnit.Month, Tense.Past, "bir ay önce" },
        { 2, TimeUnit.Month, Tense.Future, "2 ay sonra" },
        { 1, TimeUnit.Year, Tense.Past, "bir yıl önce" },
        { 2, TimeUnit.Year, Tense.Future, "2 yıl sonra" },
        { 0, TimeUnit.Second, Tense.Future, "şimdi" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 saniye" },
        { 2, TimeUnit.Second, false, "2 saniye" },
        { 1, TimeUnit.Minute, false, "1 dakika" },
        { 2, TimeUnit.Minute, false, "2 dakika" },
        { 1, TimeUnit.Hour, false, "1 saat" },
        { 2, TimeUnit.Hour, false, "2 saat" },
        { 1, TimeUnit.Day, false, "1 gün" },
        { 2, TimeUnit.Day, false, "2 gün" },
        { 0, TimeUnit.Millisecond, false, "0 milisaniye" },
        { 0, TimeUnit.Millisecond, true, "zaman farkı yok" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("tr", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("tr", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("tr", unit, timeUnit, toWords, expected);
}
