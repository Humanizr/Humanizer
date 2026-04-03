namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_id
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "sedetik yang lalu" },
        { 2, TimeUnit.Second, Tense.Future, "2 detik dari sekarang" },
        { 1, TimeUnit.Minute, Tense.Past, "semenit yang lalu" },
        { 2, TimeUnit.Minute, Tense.Future, "2 menit dari sekarang" },
        { 1, TimeUnit.Hour, Tense.Past, "sejam yang lalu" },
        { 2, TimeUnit.Hour, Tense.Future, "2 jam dari sekarang" },
        { 1, TimeUnit.Day, Tense.Past, "kemarin" },
        { 1, TimeUnit.Day, Tense.Future, "besok" },
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
        { 1, TimeUnit.Second, false, "1 detik" },
        { 2, TimeUnit.Second, false, "2 detik" },
        { 1, TimeUnit.Minute, false, "1 menit" },
        { 2, TimeUnit.Minute, false, "2 menit" },
        { 1, TimeUnit.Hour, false, "1 jam" },
        { 2, TimeUnit.Hour, false, "2 jam" },
        { 1, TimeUnit.Day, false, "1 hari" },
        { 2, TimeUnit.Day, false, "2 hari" },
        { 0, TimeUnit.Millisecond, false, "0 milidetik" },
        { 0, TimeUnit.Millisecond, true, "waktu kosong" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("id", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("id", "never");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("id", unit, timeUnit, toWords, expected);
}
