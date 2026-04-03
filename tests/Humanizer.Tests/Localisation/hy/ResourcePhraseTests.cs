namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hy
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "մեկ վայրկյան առաջ" },
        { "DateFutureSecond2", "2 վայրկյանից" },
        { "DatePastMinute1", "մեկ րոպե առաջ" },
        { "DateFutureMinute2", "2 րոպեից" },
        { "DatePastHour1", "մեկ ժամ առաջ" },
        { "DateFutureHour2", "2 ժամից" },
        { "DatePastDay1", "երեկ" },
        { "DateFutureDay1", "վաղը" },
        { "DatePastDay2", "2 օր առաջ" },
        { "DateFutureDay2", "2 օրից" },
        { "DatePastMonth1", "մեկ ամիս առաջ" },
        { "DateFutureMonth2", "2 ամսից" },
        { "DatePastYear1", "մեկ տարի առաջ" },
        { "DateFutureYear2", "2 տարուց" },
        { "DateNow", "հիմա" },
        { "DateNever", "never" },
        { "SpanSecond1", "մեկ վայրկյան" },
        { "SpanSecond2", "2 վայրկյան" },
        { "SpanMinute1", "մեկ րոպե" },
        { "SpanMinute2", "2 րոպե" },
        { "SpanHour1", "մեկ ժամ" },
        { "SpanHour2", "2 ժամ" },
        { "SpanDay1", "մեկ օր" },
        { "SpanDay2", "2 օր" },
        { "SpanZero", "0 միլիվայրկյան" },
        { "SpanZeroWords", "ժամանակը բացակայում է" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("hy", caseName, expected);
}
