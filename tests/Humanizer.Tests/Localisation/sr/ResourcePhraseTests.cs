namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_sr
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "пре секунд" },
        { "DateFutureSecond2", "за 2 секунде" },
        { "DatePastMinute1", "пре минут" },
        { "DateFutureMinute2", "за 2 минута" },
        { "DatePastHour1", "пре сат времена" },
        { "DateFutureHour2", "за 2 сата" },
        { "DatePastDay1", "јуче" },
        { "DateFutureDay1", "сутра" },
        { "DatePastDay2", "пре 2 дана" },
        { "DateFutureDay2", "за 2 дана" },
        { "DatePastMonth1", "пре месец дана" },
        { "DateFutureMonth2", "за 2 месеца" },
        { "DatePastYear1", "пре годину дана" },
        { "DateFutureYear2", "за 2 године" },
        { "DateNow", "сада" },
        { "DateNever", "never" },
        { "SpanSecond1", "1 секунда" },
        { "SpanSecond2", "2 секунде" },
        { "SpanMinute1", "1 минут" },
        { "SpanMinute2", "2 минута" },
        { "SpanHour1", "1 сат" },
        { "SpanHour2", "2 сата" },
        { "SpanDay1", "1 дан" },
        { "SpanDay2", "2 дана" },
        { "SpanZero", "0 милисекунди" },
        { "SpanZeroWords", "без протеклог времена" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("sr", caseName, expected);
}
