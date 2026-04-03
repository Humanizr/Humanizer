namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_cs
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "před sekundou" },
        { "DateFutureSecond2", "za 2 sekundy" },
        { "DatePastMinute1", "před minutou" },
        { "DateFutureMinute2", "za 2 minuty" },
        { "DatePastHour1", "před hodinou" },
        { "DateFutureHour2", "za 2 hodiny" },
        { "DatePastDay1", "včera" },
        { "DateFutureDay1", "zítra" },
        { "DatePastDay2", "před 2 dny" },
        { "DateFutureDay2", "za 2 dny" },
        { "DatePastMonth1", "před měsícem" },
        { "DateFutureMonth2", "za 2 měsíce" },
        { "DatePastYear1", "před rokem" },
        { "DateFutureYear2", "za 2 roky" },
        { "DateNow", "teď" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekunda" },
        { "SpanSecond2", "2 sekundy" },
        { "SpanMinute1", "1 minuta" },
        { "SpanMinute2", "2 minuty" },
        { "SpanHour1", "1 hodina" },
        { "SpanHour2", "2 hodiny" },
        { "SpanDay1", "1 den" },
        { "SpanDay2", "2 dny" },
        { "SpanZero", "0 milisekund" },
        { "SpanZeroWords", "není čas" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("cs", caseName, expected);
}
