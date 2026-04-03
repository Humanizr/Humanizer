namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_bn
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "এক সেকেন্ড আগে" },
        { "DateFutureSecond2", "2 সেকেন্ড পর" },
        { "DatePastMinute1", "এক মিনিট আগে" },
        { "DateFutureMinute2", "2 মিনিট পর" },
        { "DatePastHour1", "এক ঘণ্টা আগে" },
        { "DateFutureHour2", "2 ঘণ্টা পর" },
        { "DatePastDay1", "গতকাল" },
        { "DateFutureDay1", "আগামিকাল" },
        { "DatePastDay2", "2 দিন আগে" },
        { "DateFutureDay2", "2 দিন পর" },
        { "DatePastMonth1", "এক মাস আগে" },
        { "DateFutureMonth2", "2 মাস পর" },
        { "DatePastYear1", "এক বছর আগে" },
        { "DateFutureYear2", "2 বছর পর" },
        { "DateNow", "এখন" },
        { "DateNever", "never" },
        { "SpanSecond1", "এক সেকেন্ড" },
        { "SpanSecond2", "2 সেকেন্ড" },
        { "SpanMinute1", "এক মিনিট" },
        { "SpanMinute2", "2 মিনিট" },
        { "SpanHour1", "এক ঘণ্টা" },
        { "SpanHour2", "2 ঘণ্টা" },
        { "SpanDay1", "এক দিন" },
        { "SpanDay2", "2 দিন" },
        { "SpanZero", "0 মিলিসেকেন্ড" },
        { "SpanZeroWords", "শূন্য সময়" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("bn", caseName, expected);
}
