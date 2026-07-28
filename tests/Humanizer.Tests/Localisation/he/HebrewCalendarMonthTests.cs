namespace Humanizer.Tests.Localisation.he;

[UseCulture("he")]
public class HebrewCalendarMonthTests
{
    public static TheoryData<int, string> MonthNames { get; } = new()
    {
        { 1, "בינואר" },
        { 2, "בפברואר" },
        { 3, "במרץ" },
        { 4, "באפריל" },
        { 5, "במאי" },
        { 6, "ביוני" },
        { 7, "ביולי" },
        { 8, "באוגוסט" },
        { 9, "בספטמבר" },
        { 10, "באוקטובר" },
        { 11, "בנובמבר" },
        { 12, "בדצמבר" }
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