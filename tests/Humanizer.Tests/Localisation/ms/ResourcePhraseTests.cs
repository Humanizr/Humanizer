namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ms
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "sesaat yang lalu" },
        { "DateFutureSecond2", "2 saat dari sekarang" },
        { "DatePastMinute1", "seminit yang lalu" },
        { "DateFutureMinute2", "2 minit dari sekarang" },
        { "DatePastHour1", "sejam yang lalu" },
        { "DateFutureHour2", "2 jam dari sekarang" },
        { "DatePastDay1", "semalam" },
        { "DateFutureDay1", "esok" },
        { "DatePastDay2", "2 hari yang lalu" },
        { "DateFutureDay2", "2 hari dari sekarang" },
        { "DatePastMonth1", "sebulan yang lalu" },
        { "DateFutureMonth2", "2 bulan dari sekarang" },
        { "DatePastYear1", "setahun yang lalu" },
        { "DateFutureYear2", "2 tahun dari sekarang" },
        { "DateNow", "sekarang" },
        { "DateNever", "tidak pernah" },
        { "SpanSecond1", "1 saat" },
        { "SpanSecond2", "2 saat" },
        { "SpanMinute1", "1 minit" },
        { "SpanMinute2", "2 minit" },
        { "SpanHour1", "1 jam" },
        { "SpanHour2", "2 jam" },
        { "SpanDay1", "1 hari" },
        { "SpanDay2", "2 hari" },
        { "SpanZero", "0 milisaat" },
        { "SpanZeroWords", "tiada masa" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ms", caseName, expected);
}
