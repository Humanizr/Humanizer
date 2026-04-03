namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_id
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "sedetik yang lalu" },
        { "DateFutureSecond2", "2 detik dari sekarang" },
        { "DatePastMinute1", "semenit yang lalu" },
        { "DateFutureMinute2", "2 menit dari sekarang" },
        { "DatePastHour1", "sejam yang lalu" },
        { "DateFutureHour2", "2 jam dari sekarang" },
        { "DatePastDay1", "kemarin" },
        { "DateFutureDay1", "besok" },
        { "DatePastDay2", "2 hari yang lalu" },
        { "DateFutureDay2", "2 hari dari sekarang" },
        { "DatePastMonth1", "sebulan yang lalu" },
        { "DateFutureMonth2", "2 bulan dari sekarang" },
        { "DatePastYear1", "setahun yang lalu" },
        { "DateFutureYear2", "2 tahun dari sekarang" },
        { "DateNow", "sekarang" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 detik" },
        { "SpanSecond2", "2 detik" },
        { "SpanMinute1", "1 menit" },
        { "SpanMinute2", "2 menit" },
        { "SpanHour1", "1 jam" },
        { "SpanHour2", "2 jam" },
        { "SpanDay1", "1 hari" },
        { "SpanDay2", "2 hari" },
        { "SpanZero", "0 milidetik" },
        { "SpanZeroWords", "waktu kosong" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("id", caseName, expected);
}
