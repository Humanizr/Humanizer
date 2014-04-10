namespace Humanizer.Localisation.Formatters
{
    /// <summary>
    /// Implement this interface if your language has complex rules around dealing with numbers. 
    /// For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and 
    /// in Arabic 2 days is يومين not 2 يوم
    /// </summary>
    public interface IFormatter
    {
        string DateHumanize_Now();
        string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit);
        
        string TimeSpanHumanize_Zero();
        string TimeSpanHumanize(TimeUnit timeUnit, int unit);
    }
}
