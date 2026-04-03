namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_el
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "πριν από ένα δευτερόλεπτο" },
        { "DateFutureSecond2", "2 δευτερόλεπτα από τώρα" },
        { "DatePastMinute1", "πριν από ένα λεπτό" },
        { "DateFutureMinute2", "2 λεπτά από τώρα" },
        { "DatePastHour1", "πριν από μία ώρα" },
        { "DateFutureHour2", "2 ώρες από τώρα" },
        { "DatePastDay1", "χθες" },
        { "DateFutureDay1", "αύριο" },
        { "DatePastDay2", "πριν από 2 ημέρες" },
        { "DateFutureDay2", "2 ημέρες από τώρα" },
        { "DatePastMonth1", "πριν από έναν μήνα" },
        { "DateFutureMonth2", "2 μήνες από τώρα" },
        { "DatePastYear1", "πριν από έναν χρόνο" },
        { "DateFutureYear2", "2 χρόνια από τώρα" },
        { "DateNow", "τώρα" },
        { "DateNever", "ποτέ" },
        { "SpanSecond1", "1 δευτερόλεπτο" },
        { "SpanSecond2", "2 δευτερόλεπτα" },
        { "SpanMinute1", "1 λεπτό" },
        { "SpanMinute2", "2 λεπτά" },
        { "SpanHour1", "1 ώρα" },
        { "SpanHour2", "2 ώρες" },
        { "SpanDay1", "1 μέρα" },
        { "SpanDay2", "2 μέρες" },
        { "SpanZero", "0 χιλιοστά του δευτερολέπτου" },
        { "SpanZeroWords", "μηδέν χρόνος" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("el", caseName, expected);
}
