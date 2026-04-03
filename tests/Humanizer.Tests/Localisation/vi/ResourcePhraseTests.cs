namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_vi
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "cách đây một giây" },
        { "DateFutureSecond2", "2 giây nữa" },
        { "DatePastMinute1", "cách đây một phút" },
        { "DateFutureMinute2", "2 phút nữa" },
        { "DatePastHour1", "cách đây một tiếng" },
        { "DateFutureHour2", "2 tiếng nữa" },
        { "DatePastDay1", "hôm qua" },
        { "DateFutureDay1", "ngày mai" },
        { "DatePastDay2", "cách đây 2 ngày" },
        { "DateFutureDay2", "2 ngày nữa" },
        { "DatePastMonth1", "cách đây một tháng" },
        { "DateFutureMonth2", "2 tháng nữa" },
        { "DatePastYear1", "cách đây một năm" },
        { "DateFutureYear2", "2 năm nữa" },
        { "DateNow", "bây giờ" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 giây" },
        { "SpanSecond2", "2 giây" },
        { "SpanMinute1", "1 phút" },
        { "SpanMinute2", "2 phút" },
        { "SpanHour1", "1 giờ" },
        { "SpanHour2", "2 giờ" },
        { "SpanDay1", "1 ngày" },
        { "SpanDay2", "2 ngày" },
        { "SpanZero", "0 phần ngàn giây" },
        { "SpanZeroWords", "không giờ" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("vi", caseName, expected);
}
