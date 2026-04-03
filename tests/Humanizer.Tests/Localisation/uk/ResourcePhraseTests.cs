namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_uk
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "секунду тому" },
        { "DateFutureSecond2", "через 2 секунди" },
        { "DatePastMinute1", "хвилину тому" },
        { "DateFutureMinute2", "через 2 хвилини" },
        { "DatePastHour1", "годину тому" },
        { "DateFutureHour2", "через 2 години" },
        { "DatePastDay1", "вчора" },
        { "DateFutureDay1", "завтра" },
        { "DatePastDay2", "2 дні тому" },
        { "DateFutureDay2", "через 2 дні" },
        { "DatePastMonth1", "місяць тому" },
        { "DateFutureMonth2", "через 2 місяці" },
        { "DatePastYear1", "рік тому" },
        { "DateFutureYear2", "через 2 роки" },
        { "DateNow", "зараз" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 секунда" },
        { "SpanSecond2", "2 секунди" },
        { "SpanMinute1", "1 хвилина" },
        { "SpanMinute2", "2 хвилини" },
        { "SpanHour1", "1 година" },
        { "SpanHour2", "2 години" },
        { "SpanDay1", "1 день" },
        { "SpanDay2", "2 дні" },
        { "SpanZero", "0 мілісекунд" },
        { "SpanZeroWords", "без часу" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("uk", caseName, expected);
}
