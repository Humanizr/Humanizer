namespace Humanizer.Localisation
{
    /// <summary>
    /// Implement this interface if your language has complex rules around dealing with numbers. 
    /// For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and 
    /// in Arabic 2 days is يومين not 2 يوم
    /// </summary>
    public interface IFormatter
    {
        string DateHumanize_Now();
        string DateHumanize_Seconds(int seconds = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past);
        string DateHumanize_Minutes(int minutes = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past);
        string DateHumanize_Hours(int hours = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past);
        string DateHumanize_Days(int days = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past);
        string DateHumanize_Months(int months = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past);
        string DateHumanize_Years(int years = 1, TimeUnitTense timeUnitTense = TimeUnitTense.Past);

        string TimeSpanHumanize_Zero();
        string TimeSpanHumanize_Milliseconds(int milliSeconds = 1);
        string TimeSpanHumanize_Seconds(int seconds = 1);
        string TimeSpanHumanize_Minutes(int minutes = 1);
        string TimeSpanHumanize_Hours(int hours = 1);
        string TimeSpanHumanize_Days(int days = 1);
        string TimeSpanHumanize_Weeks(int weeks = 1);
    }
}
