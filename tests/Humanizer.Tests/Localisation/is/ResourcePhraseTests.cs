namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_is
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "fyrir einni sekúndu" },
        { "DateFutureSecond2", "eftir 2 sekúndur" },
        { "DatePastMinute1", "fyrir einni mínútu" },
        { "DateFutureMinute2", "eftir 2 mínútur" },
        { "DatePastHour1", "fyrir einni klukkustund" },
        { "DateFutureHour2", "eftir 2 klukkustundir" },
        { "DatePastDay1", "í gær" },
        { "DateFutureDay1", "á morgun" },
        { "DatePastDay2", "fyrir 2 dögum" },
        { "DateFutureDay2", "eftir 2 daga" },
        { "DatePastMonth1", "fyrir einum mánuði" },
        { "DateFutureMonth2", "eftir 2 mánuði" },
        { "DatePastYear1", "fyrir einu ári" },
        { "DateFutureYear2", "eftir 2 ár" },
        { "DateNow", "núna" },
        { "DateNever", "aldrei" },
        { "SpanSecond1", "ein sekúnda" },
        { "SpanSecond2", "2 sekúndur" },
        { "SpanMinute1", "ein mínúta" },
        { "SpanMinute2", "2 mínútur" },
        { "SpanHour1", "ein klukkustund" },
        { "SpanHour2", "2 klukkustundir" },
        { "SpanDay1", "einn dagur" },
        { "SpanDay2", "2 dagar" },
        { "SpanZero", "0 millisekúndur" },
        { "SpanZeroWords", "engin stund" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("is", caseName, expected);
}
