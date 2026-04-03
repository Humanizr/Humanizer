namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fa
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "یک ثانیه پیش" },
        { "DateFutureSecond2", "2 ثانیه بعد" },
        { "DatePastMinute1", "یک دقیقه پیش" },
        { "DateFutureMinute2", "2 دقیقه بعد" },
        { "DatePastHour1", "یک ساعت پیش" },
        { "DateFutureHour2", "2 ساعت بعد" },
        { "DatePastDay1", "دیروز" },
        { "DateFutureDay1", "فردا" },
        { "DatePastDay2", "2 روز پیش" },
        { "DateFutureDay2", "2 روز بعد" },
        { "DatePastMonth1", "یک ماه پیش" },
        { "DateFutureMonth2", "2 ماه بعد" },
        { "DatePastYear1", "یک سال پیش" },
        { "DateFutureYear2", "2 سال بعد" },
        { "DateNow", "الآن" },
        { "DateNever", "never" },
        { "SpanSecond1", "یک ثانیه" },
        { "SpanSecond2", "2 ثانیه" },
        { "SpanMinute1", "یک دقیقه" },
        { "SpanMinute2", "2 دقیقه" },
        { "SpanHour1", "یک ساعت" },
        { "SpanHour2", "2 ساعت" },
        { "SpanDay1", "یک روز" },
        { "SpanDay2", "2 روز" },
        { "SpanZero", "0 میلی ثانیه" },
        { "SpanZeroWords", "الآن" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("fa", caseName, expected);
}
