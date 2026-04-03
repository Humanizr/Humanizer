namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ku
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "چرکەیەک لەمەوبەر" },
        { "DateFutureSecond2", "2 چرکەی دیکە" },
        { "DatePastMinute1", "خولەکێک لەمەوبەر" },
        { "DateFutureMinute2", "2 خولەکی دیکە" },
        { "DatePastHour1", "کاتژمێرێک لەمەوبەر" },
        { "DateFutureHour2", "2 کاتژمێری دیکە" },
        { "DatePastDay1", "دوێنێ" },
        { "DateFutureDay1", "بەیانی" },
        { "DatePastDay2", "2 ڕۆژ لەمەوبەر" },
        { "DateFutureDay2", "2 ڕۆژی دیکە" },
        { "DatePastMonth1", "مانگێک لەمەوبەر" },
        { "DateFutureMonth2", "2 مانگی دیکە" },
        { "DatePastYear1", "ساڵێک لەمەوبەر" },
        { "DateFutureYear2", "2 ساڵی دیکە" },
        { "DateNow", "ئێستا" },
        { "DateNever", "هەرگیز" },
        { "SpanSecond1", "1 چرکە" },
        { "SpanSecond2", "2 چرکە" },
        { "SpanMinute1", "1 خولەک" },
        { "SpanMinute2", "2 خولەک" },
        { "SpanHour1", "1 کاتژمێر" },
        { "SpanHour2", "2 کاتژمێر" },
        { "SpanDay1", "1 ڕۆژ" },
        { "SpanDay2", "2 ڕۆژ" },
        { "SpanZero", "0 میلیچرکە" },
        { "SpanZeroWords", "ئێستا" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ku", caseName, expected);
}
