namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_he
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "לפני שנייה" },
        { "DateFutureSecond2", "בעוד 2 שניות" },
        { "DatePastMinute1", "לפני דקה" },
        { "DateFutureMinute2", "בעוד 2 דקות" },
        { "DatePastHour1", "לפני שעה" },
        { "DateFutureHour2", "בעוד שעתיים" },
        { "DatePastDay1", "אתמול" },
        { "DateFutureDay1", "מחר" },
        { "DatePastDay2", "לפני יומיים" },
        { "DateFutureDay2", "בעוד יומיים" },
        { "DatePastMonth1", "לפני חודש" },
        { "DateFutureMonth2", "בעוד חודשיים" },
        { "DatePastYear1", "לפני שנה" },
        { "DateFutureYear2", "בעוד שנתיים" },
        { "DateNow", "כעת" },
        { "DateNever", "never" },
        { "SpanSecond1", "שנייה" },
        { "SpanSecond2", "שתי שניות" },
        { "SpanMinute1", "דקה" },
        { "SpanMinute2", "שתי דקות" },
        { "SpanHour1", "שעה" },
        { "SpanHour2", "שעתיים" },
        { "SpanDay1", "יום" },
        { "SpanDay2", "יומיים" },
        { "SpanZero", "0 אלפיות שנייה" },
        { "SpanZeroWords", "אין זמן" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("he", caseName, expected);
}
