namespace Humanizer.Tests.Localisation.fa;

[UseCulture("fa")]
public class PersianCalendarMonthTests
{
    public static TheoryData<int, string> MonthNames { get; } = new()
    {
        { 1, "ژانویهٔ" },
        { 2, "فوریهٔ" },
        { 3, "مارس" },
        { 4, "آوریل" },
        { 5, "مهٔ" },
        { 6, "ژوئن" },
        { 7, "ژوئیهٔ" },
        { 8, "اوت" },
        { 9, "سپتامبر" },
        { 10, "اکتبر" },
        { 11, "نوامبر" },
        { 12, "دسامبر" }
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