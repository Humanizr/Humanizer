namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ru
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "секунду назад" },
        { "DateFutureSecond2", "через 2 секунды" },
        { "DatePastMinute1", "минуту назад" },
        { "DateFutureMinute2", "через 2 минуты" },
        { "DatePastHour1", "час назад" },
        { "DateFutureHour2", "через 2 часа" },
        { "DatePastDay1", "вчера" },
        { "DateFutureDay1", "завтра" },
        { "DatePastDay2", "2 дня назад" },
        { "DateFutureDay2", "через 2 дня" },
        { "DatePastMonth1", "месяц назад" },
        { "DateFutureMonth2", "через 2 месяца" },
        { "DatePastYear1", "год назад" },
        { "DateFutureYear2", "через 2 года" },
        { "DateNow", "сейчас" },
        { "DateNever", "никогда" },
        { "SpanSecond1", "1 секунда" },
        { "SpanSecond2", "2 секунды" },
        { "SpanMinute1", "1 минута" },
        { "SpanMinute2", "2 минуты" },
        { "SpanHour1", "1 час" },
        { "SpanHour2", "2 часа" },
        { "SpanDay1", "1 день" },
        { "SpanDay2", "2 дня" },
        { "SpanZero", "0 миллисекунд" },
        { "SpanZeroWords", "нет времени" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ru", caseName, expected);
}
