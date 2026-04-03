namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_ku
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "چرکەیەک لەمەوبەر" },
        { 2, TimeUnit.Second, Tense.Future, "2 چرکەی دیکە" },
        { 1, TimeUnit.Minute, Tense.Past, "خولەکێک لەمەوبەر" },
        { 2, TimeUnit.Minute, Tense.Future, "2 خولەکی دیکە" },
        { 1, TimeUnit.Hour, Tense.Past, "کاتژمێرێک لەمەوبەر" },
        { 2, TimeUnit.Hour, Tense.Future, "2 کاتژمێری دیکە" },
        { 1, TimeUnit.Day, Tense.Past, "دوێنێ" },
        { 1, TimeUnit.Day, Tense.Future, "بەیانی" },
        { 2, TimeUnit.Day, Tense.Past, "2 ڕۆژ لەمەوبەر" },
        { 2, TimeUnit.Day, Tense.Future, "2 ڕۆژی دیکە" },
        { 1, TimeUnit.Month, Tense.Past, "مانگێک لەمەوبەر" },
        { 2, TimeUnit.Month, Tense.Future, "2 مانگی دیکە" },
        { 1, TimeUnit.Year, Tense.Past, "ساڵێک لەمەوبەر" },
        { 2, TimeUnit.Year, Tense.Future, "2 ساڵی دیکە" },
        { 0, TimeUnit.Second, Tense.Future, "ئێستا" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 چرکە" },
        { 2, TimeUnit.Second, false, "2 چرکە" },
        { 1, TimeUnit.Minute, false, "1 خولەک" },
        { 2, TimeUnit.Minute, false, "2 خولەک" },
        { 1, TimeUnit.Hour, false, "1 کاتژمێر" },
        { 2, TimeUnit.Hour, false, "2 کاتژمێر" },
        { 1, TimeUnit.Day, false, "1 ڕۆژ" },
        { 2, TimeUnit.Day, false, "2 ڕۆژ" },
        { 0, TimeUnit.Millisecond, false, "0 میلیچرکە" },
        { 0, TimeUnit.Millisecond, true, "ئێستا" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("ku", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("ku", "هەرگیز");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("ku", unit, timeUnit, toWords, expected);
}
