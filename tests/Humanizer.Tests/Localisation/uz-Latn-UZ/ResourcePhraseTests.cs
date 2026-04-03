namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uz_Latn_UZ
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "bir soniya avval" },
        { "DateFutureSecond2", "2 sekunddan so`ng" },
        { "DatePastMinute1", "bir daqiqa avval" },
        { "DateFutureMinute2", "2 minutdan so`ng" },
        { "DatePastHour1", "bir soat avval" },
        { "DateFutureHour2", "2 soatdan so`ng" },
        { "DatePastDay1", "kecha" },
        { "DateFutureDay1", "ertaga" },
        { "DatePastDay2", "2 kun avval" },
        { "DateFutureDay2", "2 kundan so`ng" },
        { "DatePastMonth1", "bir oy avval" },
        { "DateFutureMonth2", "2 oydan so`ng" },
        { "DatePastYear1", "bir yil avval" },
        { "DateFutureYear2", "2 yildan so`ng" },
        { "DateNow", "hozir" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekund" },
        { "SpanSecond2", "2 sekund" },
        { "SpanMinute1", "1 minut" },
        { "SpanMinute2", "2 minut" },
        { "SpanHour1", "1 soat" },
        { "SpanHour2", "2 soat" },
        { "SpanDay1", "1 kun" },
        { "SpanDay2", "2 kun" },
        { "SpanZero", "0 millisekund" },
        { "SpanZeroWords", "vaqt yo`q" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("uz-Latn-UZ", caseName, expected);
}
