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
        string DateHumanize_NotYet();
        string DateHumanize_SingleMonthAgo();
        string DateHumanize_SingleSecondAgo();
        string DateHumanize_SingleYearAgo();
        string DateHumanize_SingleDayAgo();
    }
}
