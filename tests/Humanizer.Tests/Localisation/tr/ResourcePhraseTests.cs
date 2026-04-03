namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_tr
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "bir saniye önce" },
        { "DateFutureSecond2", "2 saniye sonra" },
        { "DatePastMinute1", "bir dakika önce" },
        { "DateFutureMinute2", "2 dakika sonra" },
        { "DatePastHour1", "bir saat önce" },
        { "DateFutureHour2", "2 saat sonra" },
        { "DatePastDay1", "dün" },
        { "DateFutureDay1", "yarın" },
        { "DatePastDay2", "2 gün önce" },
        { "DateFutureDay2", "2 gün sonra" },
        { "DatePastMonth1", "bir ay önce" },
        { "DateFutureMonth2", "2 ay sonra" },
        { "DatePastYear1", "bir yıl önce" },
        { "DateFutureYear2", "2 yıl sonra" },
        { "DateNow", "şimdi" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 saniye" },
        { "SpanSecond2", "2 saniye" },
        { "SpanMinute1", "1 dakika" },
        { "SpanMinute2", "2 dakika" },
        { "SpanHour1", "1 saat" },
        { "SpanHour2", "2 saat" },
        { "SpanDay1", "1 gün" },
        { "SpanDay2", "2 gün" },
        { "SpanZero", "0 milisaniye" },
        { "SpanZeroWords", "zaman farkı yok" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("tr", caseName, expected);
}
