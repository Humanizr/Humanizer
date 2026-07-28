namespace Humanizer.Tests.Localisation.ta;

[UseCulture("ta")]
public class TamilCalendarMonthTests
{
    public static TheoryData<int, string> MonthNames { get; } = new()
    {
        { 1, "ஜனவரி" },
        { 2, "பிப்ரவரி" },
        { 3, "மார்ச்" },
        { 4, "ஏப்ரல்" },
        { 5, "மே" },
        { 6, "ஜூன்" },
        { 7, "ஜூலை" },
        { 8, "ஆகஸ்ட்" },
        { 9, "செப்டம்பர்" },
        { 10, "அக்டோபர்" },
        { 11, "நவம்பர்" },
        { 12, "டிசம்பர்" }
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