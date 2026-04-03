namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fr
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "il y a une seconde" },
        { "DateFutureSecond2", "dans 2 secondes" },
        { "DatePastMinute1", "il y a une minute" },
        { "DateFutureMinute2", "dans 2 minutes" },
        { "DatePastHour1", "il y a une heure" },
        { "DateFutureHour2", "dans 2 heures" },
        { "DatePastDay1", "hier" },
        { "DateFutureDay1", "demain" },
        { "DatePastDay2", "avant-hier" },
        { "DateFutureDay2", "après-demain" },
        { "DatePastMonth1", "il y a un mois" },
        { "DateFutureMonth2", "dans 2 mois" },
        { "DatePastYear1", "il y a un an" },
        { "DateFutureYear2", "dans 2 ans" },
        { "DateNow", "maintenant" },
        { "DateNever", "jamais" },
        { "SpanSecond1", "1 seconde" },
        { "SpanSecond2", "2 secondes" },
        { "SpanMinute1", "1 minute" },
        { "SpanMinute2", "2 minutes" },
        { "SpanHour1", "1 heure" },
        { "SpanHour2", "2 heures" },
        { "SpanDay1", "1 jour" },
        { "SpanDay2", "2 jours" },
        { "SpanZero", "0 milliseconde" },
        { "SpanZeroWords", "temps nul" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("fr", caseName, expected);
}
