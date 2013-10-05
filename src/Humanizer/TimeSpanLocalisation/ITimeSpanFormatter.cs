namespace Humanizer.TimeSpanLocalisation
{
    /// <summary>
    /// An interface to allow for totally custom formatting of TimeSpan objects. Use this for
    /// localizations that are more complex. Override <see cref="DefaultTimeSpanFormatter"/> for
    /// less complex localizations
    /// </summary>
    public interface ITimeSpanFormatter
    {
        string MultipleWeeks(int weeks);
        string SingleWeek();
        string MultipleDays(int days);
        string SingleDay();
        string MultipleHours(int hours);
        string SingleHour();
        string MultipleMinutes(int minutes);
        string SingleMinute();
        string MultipleSeconds(int seconds);
        string SingleSecond();
        string MultipleMilliseconds(int milliSeconds);
        string SingleMillisecond();
        string Zero();
    }
}