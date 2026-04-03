namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_hu
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "egy másodperce" },
        { "DateFutureSecond2", "2 másodperc múlva" },
        { "DatePastMinute1", "egy perce" },
        { "DateFutureMinute2", "2 perc múlva" },
        { "DatePastHour1", "egy órája" },
        { "DateFutureHour2", "2 óra múlva" },
        { "DatePastDay1", "tegnap" },
        { "DateFutureDay1", "holnap" },
        { "DatePastDay2", "2 napja" },
        { "DateFutureDay2", "2 nap múlva" },
        { "DatePastMonth1", "egy hónapja" },
        { "DateFutureMonth2", "2 hónap múlva" },
        { "DatePastYear1", "egy éve" },
        { "DateFutureYear2", "2 év múlva" },
        { "DateNow", "most" },
        { "DateNever", "soha" },
        { "SpanSecond1", "1 másodperc" },
        { "SpanSecond2", "2 másodperc" },
        { "SpanMinute1", "1 perc" },
        { "SpanMinute2", "2 perc" },
        { "SpanHour1", "1 óra" },
        { "SpanHour2", "2 óra" },
        { "SpanDay1", "1 nap" },
        { "SpanDay2", "2 nap" },
        { "SpanZero", "0 ezredmásodperc" },
        { "SpanZeroWords", "nincs idő" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("hu", caseName, expected);
}
