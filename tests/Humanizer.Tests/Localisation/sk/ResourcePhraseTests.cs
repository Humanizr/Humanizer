namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sk
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "pred sekundou" },
        { "DateFutureSecond2", "o 2 sekundy" },
        { "DatePastMinute1", "pred minútou" },
        { "DateFutureMinute2", "o 2 minúty" },
        { "DatePastHour1", "pred hodinou" },
        { "DateFutureHour2", "o 2 hodiny" },
        { "DatePastDay1", "včera" },
        { "DateFutureDay1", "zajtra" },
        { "DatePastDay2", "pred 2 dňami" },
        { "DateFutureDay2", "o 2 dni" },
        { "DatePastMonth1", "pred mesiacom" },
        { "DateFutureMonth2", "o 2 mesiace" },
        { "DatePastYear1", "pred rokom" },
        { "DateFutureYear2", "o 2 roky" },
        { "DateNow", "teraz" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekunda" },
        { "SpanSecond2", "2 sekundy" },
        { "SpanMinute1", "1 minúta" },
        { "SpanMinute2", "2 minúty" },
        { "SpanHour1", "1 hodina" },
        { "SpanHour2", "2 hodiny" },
        { "SpanDay1", "1 deň" },
        { "SpanDay2", "2 dni" },
        { "SpanZero", "0 milisekúnd" },
        { "SpanZeroWords", "žiadny čas" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("sk", caseName, expected);
}
