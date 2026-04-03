namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_fil
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "isang segundo ang nakalipas" },
        { 2, TimeUnit.Second, Tense.Future, "2 segundo mula ngayon" },
        { 1, TimeUnit.Minute, Tense.Past, "isang minutong nakalipas" },
        { 2, TimeUnit.Minute, Tense.Future, "2 minuto mula ngayon" },
        { 1, TimeUnit.Hour, Tense.Past, "isang oras ang nakalipas" },
        { 2, TimeUnit.Hour, Tense.Future, "2 oras mula ngayon" },
        { 1, TimeUnit.Day, Tense.Past, "kahapon" },
        { 1, TimeUnit.Day, Tense.Future, "bukas" },
        { 2, TimeUnit.Day, Tense.Past, "2 isang araw ang nakalipas" },
        { 2, TimeUnit.Day, Tense.Future, "2 araw mula ngayon" },
        { 1, TimeUnit.Month, Tense.Past, "isang buwan ang nakalipas" },
        { 2, TimeUnit.Month, Tense.Future, "2 buwan mula ngayon" },
        { 1, TimeUnit.Year, Tense.Past, "isang taong nakalipas" },
        { 2, TimeUnit.Year, Tense.Future, "2 taon mula ngayon" },
        { 0, TimeUnit.Second, Tense.Future, "ngayon" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 segundo" },
        { 2, TimeUnit.Second, false, "2 segundo" },
        { 1, TimeUnit.Minute, false, "1 minuto" },
        { 2, TimeUnit.Minute, false, "2 minuto" },
        { 1, TimeUnit.Hour, false, "1 oras" },
        { 2, TimeUnit.Hour, false, "2 oras" },
        { 1, TimeUnit.Day, false, "1 araw" },
        { 2, TimeUnit.Day, false, "2 araw" },
        { 0, TimeUnit.Millisecond, false, "0 milliseconds" },
        { 0, TimeUnit.Millisecond, true, "walang oras" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("fil", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("fil", "hindi kailanman");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("fil", unit, timeUnit, toWords, expected);
}
