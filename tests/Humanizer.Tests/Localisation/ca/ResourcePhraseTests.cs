namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ca
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "fa un segon" },
        { "DateFutureSecond2", "d'aquí 2 segons" },
        { "DatePastMinute1", "fa un minut" },
        { "DateFutureMinute2", "d'aquí 2 minuts" },
        { "DatePastHour1", "fa una hora" },
        { "DateFutureHour2", "d'aquí 2 hores" },
        { "DatePastDay1", "ahir" },
        { "DateFutureDay1", "demà" },
        { "DatePastDay2", "fa 2 dies" },
        { "DateFutureDay2", "d'aquí 2 dies" },
        { "DatePastMonth1", "fa un mes" },
        { "DateFutureMonth2", "d'aquí 2 mesos" },
        { "DatePastYear1", "fa un any" },
        { "DateFutureYear2", "d'aquí 2 anys" },
        { "DateNow", "ara" },
        { "DateNever", "mai" },
        { "SpanSecond1", "1 segon" },
        { "SpanSecond2", "2 segons" },
        { "SpanMinute1", "1 minut" },
        { "SpanMinute2", "2 minuts" },
        { "SpanHour1", "1 hora" },
        { "SpanHour2", "2 hores" },
        { "SpanDay1", "1 dia" },
        { "SpanDay2", "2 dies" },
        { "SpanZero", "0 mil·lisegons" },
        { "SpanZeroWords", "res" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("ca", caseName, expected);
}
