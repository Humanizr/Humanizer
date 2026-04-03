namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_th
{
    public static TheoryData<int, TimeUnit, Tense, string> DateHumanizeCases => new()
    {
        { 1, TimeUnit.Second, Tense.Past, "หนึ่งวินาทีที่แล้ว" },
        { 2, TimeUnit.Second, Tense.Future, "2 วินาทีจากนี้" },
        { 1, TimeUnit.Minute, Tense.Past, "หนึ่งนาทีที่แล้ว" },
        { 2, TimeUnit.Minute, Tense.Future, "2 นาทีจากนี้" },
        { 1, TimeUnit.Hour, Tense.Past, "หนึ่งชั่วโมงที่แล้ว" },
        { 2, TimeUnit.Hour, Tense.Future, "2 ชั่วโมงจากนี้" },
        { 1, TimeUnit.Day, Tense.Past, "เมื่อวานนี้" },
        { 1, TimeUnit.Day, Tense.Future, "พรุ่งนี้" },
        { 2, TimeUnit.Day, Tense.Past, "2 วันที่แล้ว" },
        { 2, TimeUnit.Day, Tense.Future, "2 วันจากนี้" },
        { 1, TimeUnit.Month, Tense.Past, "หนึ่งเดือนที่แล้ว" },
        { 2, TimeUnit.Month, Tense.Future, "2 เดือนจากนี้" },
        { 1, TimeUnit.Year, Tense.Past, "หนึ่งปีที่แล้ว" },
        { 2, TimeUnit.Year, Tense.Future, "2 ปีจากนี้" },
        { 0, TimeUnit.Second, Tense.Future, "ขณะนี้" },
    };

    public static TheoryData<int, TimeUnit, bool, string> TimeSpanHumanizeCases => new()
    {
        { 1, TimeUnit.Second, false, "1 วินาที" },
        { 2, TimeUnit.Second, false, "2 วินาที" },
        { 1, TimeUnit.Minute, false, "1 นาที" },
        { 2, TimeUnit.Minute, false, "2 นาที" },
        { 1, TimeUnit.Hour, false, "1 ชั่วโมง" },
        { 2, TimeUnit.Hour, false, "2 ชั่วโมง" },
        { 1, TimeUnit.Day, false, "1 วัน" },
        { 2, TimeUnit.Day, false, "2 วัน" },
        { 0, TimeUnit.Millisecond, false, "0 มิลลิวินาที" },
        { 0, TimeUnit.Millisecond, true, "ไม่มีเวลา" },
    };

    [Theory]
    [MemberData(nameof(DateHumanizeCases))]
    public void UsesExpectedDateHumanizePhrases(int unit, TimeUnit timeUnit, Tense tense, string expected) =>
        LocaleResourcePhraseAssertions.VerifyDateHumanize("th", unit, timeUnit, tense, expected);

    [Fact]
    public void UsesExpectedNullDateHumanizePhrase() =>
        LocaleResourcePhraseAssertions.VerifyNullDateHumanize("th", "ไม่เคย");

    [Theory]
    [MemberData(nameof(TimeSpanHumanizeCases))]
    public void UsesExpectedTimeSpanHumanizePhrases(int unit, TimeUnit timeUnit, bool toWords, string expected) =>
        LocaleResourcePhraseAssertions.VerifyTimeSpanHumanize("th", unit, timeUnit, toWords, expected);
}
