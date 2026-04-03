namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_de
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "vor einer Sekunde" },
        { "DateFutureSecond2", "in 2 Sekunden" },
        { "DatePastMinute1", "vor einer Minute" },
        { "DateFutureMinute2", "in 2 Minuten" },
        { "DatePastHour1", "vor einer Stunde" },
        { "DateFutureHour2", "in 2 Stunden" },
        { "DatePastDay1", "gestern" },
        { "DateFutureDay1", "morgen" },
        { "DatePastDay2", "vor 2 Tagen" },
        { "DateFutureDay2", "in 2 Tagen" },
        { "DatePastMonth1", "vor einem Monat" },
        { "DateFutureMonth2", "in 2 Monaten" },
        { "DatePastYear1", "vor einem Jahr" },
        { "DateFutureYear2", "in 2 Jahren" },
        { "DateNow", "jetzt" },
        { "DateNever", "nie" },
        { "SpanSecond1", "1 Sekunde" },
        { "SpanSecond2", "2 Sekunden" },
        { "SpanMinute1", "1 Minute" },
        { "SpanMinute2", "2 Minuten" },
        { "SpanHour1", "1 Stunde" },
        { "SpanHour2", "2 Stunden" },
        { "SpanDay1", "1 Tag" },
        { "SpanDay2", "2 Tage" },
        { "SpanZero", "0 Millisekunden" },
        { "SpanZeroWords", "Keine Zeit" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("de", caseName, expected);
}
