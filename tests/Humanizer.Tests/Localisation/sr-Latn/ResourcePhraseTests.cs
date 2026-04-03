namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sr_Latn
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "pre sekund" },
        { "DateFutureSecond2", "za 2 sekunde" },
        { "DatePastMinute1", "pre minut" },
        { "DateFutureMinute2", "za 2 minuta" },
        { "DatePastHour1", "pre sat vremena" },
        { "DateFutureHour2", "za 2 sata" },
        { "DatePastDay1", "juče" },
        { "DateFutureDay1", "sutra" },
        { "DatePastDay2", "pre 2 dana" },
        { "DateFutureDay2", "za 2 dana" },
        { "DatePastMonth1", "pre mesec dana" },
        { "DateFutureMonth2", "za 2 meseca" },
        { "DatePastYear1", "pre godinu dana" },
        { "DateFutureYear2", "za 2 godine" },
        { "DateNow", "sada" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 sekunda" },
        { "SpanSecond2", "2 sekunde" },
        { "SpanMinute1", "1 minut" },
        { "SpanMinute2", "2 minuta" },
        { "SpanHour1", "1 sat" },
        { "SpanHour2", "2 sata" },
        { "SpanDay1", "1 dan" },
        { "SpanDay2", "2 dana" },
        { "SpanZero", "0 milisekundi" },
        { "SpanZeroWords", "bez proteklog vremena" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("sr-Latn", caseName, expected);
}
