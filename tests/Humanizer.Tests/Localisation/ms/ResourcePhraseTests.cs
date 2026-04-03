namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ms
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "sesaat yang lalu" },
        { 2, TimeUnit.Second, Tense.Future, "2 saat dari sekarang" },
        { 1, TimeUnit.Minute, Tense.Past, "seminit yang lalu" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minit dari sekarang" },
        { 1, TimeUnit.Hour, Tense.Past, "sejam yang lalu" },
        { 2, TimeUnit.Hour, Tense.Future, "2 jam dari sekarang" },
        { 1, TimeUnit.Day, Tense.Past, "semalam" },
        { 1, TimeUnit.Day, Tense.Future, "esok" },
        { 2, TimeUnit.Day, Tense.Past, "2 hari yang lalu" },
        { 2, TimeUnit.Day, Tense.Future, "2 hari dari sekarang" },
        { 1, TimeUnit.Month, Tense.Past, "sebulan yang lalu" },
        { 2, TimeUnit.Month, Tense.Future, "2 bulan dari sekarang" },
        { 1, TimeUnit.Year, Tense.Past, "setahun yang lalu" },
        { 2, TimeUnit.Year, Tense.Future, "2 tahun dari sekarang" },
        { 0, TimeUnit.Second, Tense.Future, "sekarang" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 saat" },
        { 2, TimeUnit.Second, false, "2 saat" },
        { 1, TimeUnit.Minute, false, "1 minit" },
        { 2, TimeUnit.Minute, false, "2 minit" },
        { 1, TimeUnit.Hour, false, "1 jam" },
        { 2, TimeUnit.Hour, false, "2 jam" },
        { 1, TimeUnit.Day, false, "1 hari" },
        { 2, TimeUnit.Day, false, "2 hari" },
        { 0, TimeUnit.Millisecond, false, "0 milisaat" },
        { 0, TimeUnit.Millisecond, true, "tiada masa" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ms", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ms", "tidak pernah");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ms", unit, timeUnit, toWords, expected);
}
