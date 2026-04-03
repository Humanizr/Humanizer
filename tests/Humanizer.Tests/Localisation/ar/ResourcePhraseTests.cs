namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ar
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "منذ ثانية واحدة" },
        { "DateFutureSecond2", "في غضون ثانيتين من الآن" },
        { "DatePastMinute1", "منذ دقيقة واحدة" },
        { "DateFutureMinute2", "في غضون دقيقتين من الآن" },
        { "DatePastHour1", "منذ ساعة واحدة" },
        { "DateFutureHour2", "في غضون ساعتين من الآن" },
        { "DatePastDay1", "أمس" },
        { "DateFutureDay1", "في غضون يوم واحد من الآن" },
        { "DatePastDay2", "منذ يومين" },
        { "DateFutureDay2", "في غضون يومين من الآن" },
        { "DatePastMonth1", "منذ شهر واحد" },
        { "DateFutureMonth2", "في غضون شهرين من الآن" },
        { "DatePastYear1", "العام السابق" },
        { "DateFutureYear2", "في غضون سنتين من الآن" },
        { "DateNow", "الآن" },
        { "DateNever", "never" },
        { "SpanSecond1", "ثانية واحدة" },
        { "SpanSecond2", "ثانيتين" },
        { "SpanMinute1", "دقيقة واحدة" },
        { "SpanMinute2", "دقيقتين" },
        { "SpanHour1", "ساعة واحدة" },
        { "SpanHour2", "ساعتين" },
        { "SpanDay1", "يوم واحد" },
        { "SpanDay2", "يومين" },
        { "SpanZero", "0 جزء من الثانية" },
        { "SpanZeroWords", "حالاً" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ar", caseName, expected);
}
