namespace Humanizer;

/// <summary>
/// Implement this interface if your language has complex rules around dealing with numbers.
/// For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and
/// in Arabic 2 days is يومين not 2 يوم
/// </summary>
public interface IFormatter
{
    string DateHumanize_Now();

    string DateHumanize_Never();

    /// <summary>
    /// Returns the string representation of the provided DateTime
    /// </summary>
    string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit);

    /// <summary>
    /// 0 seconds
    /// </summary>
    /// <returns>Returns 0 seconds as the string representation of Zero TimeSpan</returns>
    string TimeSpanHumanize_Zero();

    /// <summary>
    /// Returns the string representation of the provided TimeSpan
    /// </summary>
    string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false);

    /// <summary>
    /// Returns the age format that converts a humanized TimeSpan string to an age expression.
    /// For instance, in English that format adds the " old" suffix, so that "40 years" becomes "40 years old".
    /// </summary>
    /// <returns>Age format</returns>
    string TimeSpanHumanize_Age();

    /// <summary>
    /// Returns the string representation of the provided DataUnit, either as a symbol or full word
    /// </summary>
    /// <param name="dataUnit">Data unit</param>
    /// <param name="count">Number of said units, to adjust for singular/plural forms</param>
    /// <param name="toSymbol">Indicates whether the data unit should be expressed as symbol or full word</param>
    /// <returns>String representation of the provided DataUnit</returns>
    string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true);

    /// <summary>
    /// Returns the symbol for the given TimeUnit
    /// </summary>
    /// <param name="timeUnit">Time unit</param>
    /// <returns>String representation of the provided TimeUnit</returns>
    string TimeUnitHumanize(TimeUnit timeUnit);
}