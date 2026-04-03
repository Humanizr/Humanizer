namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_bg
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "преди секунда" },
        { "DateFutureSecond2", "след 2 секунди" },
        { "DatePastMinute1", "преди минута" },
        { "DateFutureMinute2", "след 2 минути" },
        { "DatePastHour1", "преди час" },
        { "DateFutureHour2", "след 2 часа" },
        { "DatePastDay1", "вчера" },
        { "DateFutureDay1", "утре" },
        { "DatePastDay2", "преди 2 дена" },
        { "DateFutureDay2", "след 2 дена" },
        { "DatePastMonth1", "преди месец" },
        { "DateFutureMonth2", "след 2 месеца" },
        { "DatePastYear1", "преди година" },
        { "DateFutureYear2", "след 2 години" },
        { "DateNow", "сега" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 секунда" },
        { "SpanSecond2", "2 секунди" },
        { "SpanMinute1", "1 минута" },
        { "SpanMinute2", "2 минути" },
        { "SpanHour1", "1 час" },
        { "SpanHour2", "2 часа" },
        { "SpanDay1", "1 ден" },
        { "SpanDay2", "2 дена" },
        { "SpanZero", "0 милисекунди" },
        { "SpanZeroWords", "няма време" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("bg", caseName, expected);
}
