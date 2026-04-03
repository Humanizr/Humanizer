namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_pl
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "przed sekundą" },
        { "DateFutureSecond2", "za 2 sekundy" },
        { "DatePastMinute1", "przed minutą" },
        { "DateFutureMinute2", "za 2 minuty" },
        { "DatePastHour1", "przed godziną" },
        { "DateFutureHour2", "za 2 godziny" },
        { "DatePastDay1", "wczoraj" },
        { "DateFutureDay1", "jutro" },
        { "DatePastDay2", "przed 2 dniami" },
        { "DateFutureDay2", "za 2 dni" },
        { "DatePastMonth1", "przed miesiącem" },
        { "DateFutureMonth2", "za 2 miesiące" },
        { "DatePastYear1", "przed rokiem" },
        { "DateFutureYear2", "za 2 lata" },
        { "DateNow", "teraz" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekunda" },
        { "SpanSecond2", "2 sekundy" },
        { "SpanMinute1", "1 minuta" },
        { "SpanMinute2", "2 minuty" },
        { "SpanHour1", "1 godzina" },
        { "SpanHour2", "2 godziny" },
        { "SpanDay1", "1 dzień" },
        { "SpanDay2", "2 dni" },
        { "SpanZero", "0 milisekund" },
        { "SpanZeroWords", "brak czasu" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("pl", caseName, expected);
}
