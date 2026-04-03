namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_az
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "bir saniyə əvvəl" },
        { "DateFutureSecond2", "2 saniyə sonra" },
        { "DatePastMinute1", "bir dəqiqə əvvəl" },
        { "DateFutureMinute2", "2 dəqiqə sonra" },
        { "DatePastHour1", "bir saat əvvəl" },
        { "DateFutureHour2", "2 saat sonra" },
        { "DatePastDay1", "dünən" },
        { "DateFutureDay1", "sabah" },
        { "DatePastDay2", "2 gün əvvəl" },
        { "DateFutureDay2", "2 gün sonra" },
        { "DatePastMonth1", "bir ay əvvəl" },
        { "DateFutureMonth2", "2 ay sonra" },
        { "DatePastYear1", "bir il əvvəl" },
        { "DateFutureYear2", "2 il sonra" },
        { "DateNow", "indi" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 saniyə" },
        { "SpanSecond2", "2 saniyə" },
        { "SpanMinute1", "1 dəqiqə" },
        { "SpanMinute2", "2 dəqiqə" },
        { "SpanHour1", "1 saat" },
        { "SpanHour2", "2 saat" },
        { "SpanDay1", "1 gün" },
        { "SpanDay2", "2 gün" },
        { "SpanZero", "0 millisaniyə" },
        { "SpanZeroWords", "zaman fərqi yoxdur" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("az", caseName, expected);
}
