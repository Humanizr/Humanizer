namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_lt
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "prieš vieną sekundę" },
        { "DateFutureSecond2", "po 2 sekundžių" },
        { "DatePastMinute1", "prieš minutę" },
        { "DateFutureMinute2", "po 2 minučių" },
        { "DatePastHour1", "prieš valandą" },
        { "DateFutureHour2", "po 2 valandų" },
        { "DatePastDay1", "vakar" },
        { "DateFutureDay1", "rytoj" },
        { "DatePastDay2", "prieš 2 dienas" },
        { "DateFutureDay2", "po 2 dienų" },
        { "DatePastMonth1", "prieš vieną mėnesį" },
        { "DateFutureMonth2", "po 2 mėnesių" },
        { "DatePastYear1", "prieš vienerius metus" },
        { "DateFutureYear2", "po 2 metų" },
        { "DateNow", "dabar" },
        { "DateNever", "niekada" },
        { "SpanSecond1", "1 sekundė" },
        { "SpanSecond2", "2 sekundės" },
        { "SpanMinute1", "1 minutė" },
        { "SpanMinute2", "2 minutės" },
        { "SpanHour1", "1 valanda" },
        { "SpanHour2", "2 valandos" },
        { "SpanDay1", "1 diena" },
        { "SpanDay2", "2 dienos" },
        { "SpanZero", "0 milisekundžių" },
        { "SpanZeroWords", "nėra laiko" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("lt", caseName, expected);
}
