namespace Humanizer.Tests.Localisation.bn;

[UseCulture("bn")]
public class BengaliCalendarMonthTests
{
    public static TheoryData<int, string> MonthNames { get; } = new()
    {
        { 1, "জানুয়ারী" },
        { 2, "ফেব্রুয়ারী" },
        { 3, "মার্চ" },
        { 4, "এপ্রিল" },
        { 5, "মে" },
        { 6, "জুন" },
        { 7, "জুলাই" },
        { 8, "আগস্ট" },
        { 9, "সেপ্টেম্বর" },
        { 10, "অক্টোবর" },
        { 11, "নভেম্বর" },
        { 12, "ডিসেম্বর" }
    };

    [Theory]
    [MemberData(nameof(MonthNames))]
    public void DateTimeToOrdinalWords_UsesAuthoredMonthNames(int month, string monthName)
    {
        Assert.Equal($"15 {monthName} 2024", new DateTime(2024, month, 15).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [MemberData(nameof(MonthNames))]
    public void DateOnlyToOrdinalWords_UsesAuthoredMonthNames(int month, string monthName)
    {
        Assert.Equal($"15 {monthName} 2024", new DateOnly(2024, month, 15).ToOrdinalWords());
    }
#endif
}