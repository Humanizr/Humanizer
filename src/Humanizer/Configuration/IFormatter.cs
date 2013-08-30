namespace Humanizer.Configuration
{
    /// <summary>
    /// Implement this interface if your language has complex rules around dealing with numbers. 
    /// For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and 
    /// in Arabic 2 days is يومين not 2 يوم
    /// </summary>
    public interface IFormatter
    {
        string DateHumanize__days_ago(int numberOfDays);
        string DateHumanize__hours_ago(int numberOfHours);
        string DateHumanize__minutes_ago(int numberOfMinutes);
        string DateHumanize__months_ago(int numberOfMonths);
        string DateHumanize__seconds_ago(int numberOfSeconds);
        string DateHumanize__years_ago(int numberOfYears);
        string DateHumanize_a_minute_ago();
        string DateHumanize_an_hour_ago();
        string DateHumanize_not_yet();
        string DateHumanize_one_month_ago();
        string DateHumanize_one_second_ago();
        string DateHumanize_one_year_ago();
        string DateHumanize_yesterday();
    }
}
