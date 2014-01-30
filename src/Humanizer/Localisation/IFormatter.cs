namespace Humanizer.Localisation
{
    /// <summary>
    /// Implement this interface if your language has complex rules around dealing with numbers. 
    /// For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and 
    /// in Arabic 2 days is يومين not 2 يوم
    /// </summary>
    public interface IFormatter
    {
        string DateHumanize_MultipleDaysAgo(int numberOfDays);
        string DateHumanize_MultipleHoursAgo(int numberOfHours);
        string DateHumanize_MultipleMinutesAgo(int numberOfMinutes);
        string DateHumanize_MultipleMonthsAgo(int numberOfMonths);
        string DateHumanize_MultipleSecondsAgo(int numberOfSeconds);
        string DateHumanize_MultipleYearsAgo(int numberOfYears);
        string DateHumanize_SingleMinuteAgo();
        string DateHumanize_SingleHourAgo();
        string DateHumanize_SingleMonthAgo();
        string DateHumanize_SingleSecondAgo();
        string DateHumanize_SingleYearAgo();
        string DateHumanize_SingleDayAgo();
        string DateHumanize_MultipleDaysFromNow(int numberOfDays);
        string DateHumanize_MultipleHoursFromNow(int numberOfHours);
        string DateHumanize_MultipleMinutesFromNow(int numberOfMinutes);
        string DateHumanize_MultipleMonthsFromNow(int numberOfMonths);
        string DateHumanize_MultipleSecondsFromNow(int numberOfSeconds);
        string DateHumanize_MultipleYearsFromNow(int numberOfYears);
        string DateHumanize_SingleMinuteFromNow();
        string DateHumanize_SingleHourFromNow();
        string DateHumanize_Now();
        string DateHumanize_SingleMonthFromNow();
        string DateHumanize_SingleSecondFromNow();
        string DateHumanize_SingleYearFromNow();
        string DateHumanize_SingleDayFromNow();
        string TimeSpanHumanize_MultipleWeeks(int weeks);
        string TimeSpanHumanize_SingleWeek();
        string TimeSpanHumanize_MultipleDays(int days);
        string TimeSpanHumanize_SingleDay();
        string TimeSpanHumanize_MultipleHours(int hours);
        string TimeSpanHumanize_SingleHour();
        string TimeSpanHumanize_MultipleMinutes(int minutes);
        string TimeSpanHumanize_SingleMinute();
        string TimeSpanHumanize_MultipleSeconds(int seconds);
        string TimeSpanHumanize_SingleSecond();
        string TimeSpanHumanize_MultipleMilliseconds(int milliSeconds);
        string TimeSpanHumanize_SingleMillisecond();
        string TimeSpanHumanize_Zero();
    }
}
