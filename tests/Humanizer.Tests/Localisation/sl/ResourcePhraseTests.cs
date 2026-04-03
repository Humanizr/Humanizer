namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sl
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "pred eno sekundo" },
        { "DateFutureSecond2", "čez 2 sekundi" },
        { "DatePastMinute1", "pred eno minuto" },
        { "DateFutureMinute2", "čez 2 minuti" },
        { "DatePastHour1", "pred eno uro" },
        { "DateFutureHour2", "čez 2 uri" },
        { "DatePastDay1", "včeraj" },
        { "DateFutureDay1", "jutri" },
        { "DatePastDay2", "pred 2 dnevoma" },
        { "DateFutureDay2", "čez 2 dni" },
        { "DatePastMonth1", "pred enim mesecem" },
        { "DateFutureMonth2", "čez 2 meseca" },
        { "DatePastYear1", "pred enim letom" },
        { "DateFutureYear2", "čez 2 leti" },
        { "DateNow", "sedaj" },
        { "DateNever", "nikoli" },
        { "SpanSecond1", "1 sekunda" },
        { "SpanSecond2", "2 sekundi" },
        { "SpanMinute1", "1 minuta" },
        { "SpanMinute2", "2 minuti" },
        { "SpanHour1", "1 ura" },
        { "SpanHour2", "2 uri" },
        { "SpanDay1", "1 dan" },
        { "SpanDay2", "2 dneva" },
        { "SpanZero", "0 milisekund" },
        { "SpanZeroWords", "nič časa" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("sl", caseName, expected);
}
