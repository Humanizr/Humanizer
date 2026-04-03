namespace Humanizer.Tests.Localisation;

public class ResourcePhraseTests_th
{
    public static TheoryData<string, string> Cases => new()
    {
        { "DatePastSecond1", "หนึ่งวินาทีที่แล้ว" },
        { "DateFutureSecond2", "2 วินาทีจากนี้" },
        { "DatePastMinute1", "หนึ่งนาทีที่แล้ว" },
        { "DateFutureMinute2", "2 นาทีจากนี้" },
        { "DatePastHour1", "หนึ่งชั่วโมงที่แล้ว" },
        { "DateFutureHour2", "2 ชั่วโมงจากนี้" },
        { "DatePastDay1", "เมื่อวานนี้" },
        { "DateFutureDay1", "พรุ่งนี้" },
        { "DatePastDay2", "2 วันที่แล้ว" },
        { "DateFutureDay2", "2 วันจากนี้" },
        { "DatePastMonth1", "หนึ่งเดือนที่แล้ว" },
        { "DateFutureMonth2", "2 เดือนจากนี้" },
        { "DatePastYear1", "หนึ่งปีที่แล้ว" },
        { "DateFutureYear2", "2 ปีจากนี้" },
        { "DateNow", "ขณะนี้" },
        { "DateNever", "ไม่เคย" },
        { "SpanSecond1", "1 วินาที" },
        { "SpanSecond2", "2 วินาที" },
        { "SpanMinute1", "1 นาที" },
        { "SpanMinute2", "2 นาที" },
        { "SpanHour1", "1 ชั่วโมง" },
        { "SpanHour2", "2 ชั่วโมง" },
        { "SpanDay1", "1 วัน" },
        { "SpanDay2", "2 วัน" },
        { "SpanZero", "0 มิลลิวินาที" },
        { "SpanZeroWords", "ไม่มีเวลา" },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void UsesExpectedResourceBackedPhrases(string caseName, string expected) =>
        LocaleResourcePhraseAssertions.Verify("th", caseName, expected);
}
