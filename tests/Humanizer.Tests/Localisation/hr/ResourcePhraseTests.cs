namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hr
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "prije jedne sekunde" },
        { "DateFutureSecond2", "za 2 sekunde" },
        { "DatePastMinute1", "prije jedne minute" },
        { "DateFutureMinute2", "za 2 minute" },
        { "DatePastHour1", "prije sat vremena" },
        { "DateFutureHour2", "za 2 sata" },
        { "DatePastDay1", "jučer" },
        { "DateFutureDay1", "sutra" },
        { "DatePastDay2", "prije 2 dana" },
        { "DateFutureDay2", "2 days from now" },
        { "DatePastMonth1", "prije mjesec dana" },
        { "DateFutureMonth2", "za 2 mjeseca" },
        { "DatePastYear1", "prije godinu dana" },
        { "DateFutureYear2", "za 2 godine" },
        { "DateNow", "upravo sada" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekunda" },
        { "SpanSecond2", "2 sekunde" },
        { "SpanMinute1", "1 minuta" },
        { "SpanMinute2", "2 minute" },
        { "SpanHour1", "1 sat" },
        { "SpanHour2", "2 sata" },
        { "SpanDay1", "1 dan" },
        { "SpanDay2", "2 dana" },
        { "SpanZero", "0 milisekundi" },
        { "SpanZeroWords", "bez podatka o vremenu" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("hr", caseName, expected);
}
