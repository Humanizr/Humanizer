namespace Humanizer.Localisation.Formatters
{
    /// <summary>
    /// Implement this interface if your language has complex rules around dealing with numbers. 
    /// For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and 
    /// in Arabic 2 days is يومين not 2 يوم
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Now
        /// </summary>
        /// <returns>Returns Now</returns>
        string DateHumanize_Now();

        /// <summary>
        /// Never
        /// </summary>
        /// <returns>Returns Never</returns>
        string DateHumanize_Never();

        /// <summary>
        /// Returns the string representation of the provided DateTime
        /// </summary>
        /// <param name="timeUnit"></param>
        /// <param name="timeUnitTense"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit);

        /// <summary>
        /// 0 seconds
        /// </summary>
        /// <returns>Returns 0 seconds as the string representation of Zero TimeSpan</returns>
        string TimeSpanHumanize_Zero();

        /// <summary>
        /// Returns the string representation of the provided TimeSpan
        /// </summary>
        /// <param name="timeUnit"></param>
        /// <param name="unit"></param>
        /// <param name="toWords"></param>
        /// <returns></returns>
        string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false);

        /// <summary>
        /// Returns the age format that converts a humanized TimeSpan string to an age expression.
        /// For instance, in English that format adds the " old" suffix, so that "40 years" becomes "40 years old".
        /// </summary>
        /// <returns>Age format</returns>
        string TimeSpanHumanize_Age();

        /// <summary>
        /// Returns the age format that converts a humanized TimeSpan string to an hyphenated age expression,
        /// before spaces are actually replaced by hyphens.
        /// </summary>
        /// <returns>Pre-hyphenated age format</returns>
        /// <remarks>
        /// In all languages but English an empty string is returned, as the notion of hyphenated age does not exist.
        /// </remarks>
        string TimeSpanHumanize_PreHyphenatedAge();

        /// <summary>
        /// Returns the string representation of the provided DataUnit, either as a symbol or full word
        /// </summary>
        /// <param name="dataUnit">Data unit</param>
        /// <param name="count">Number of said units, to adjust for singular/plural forms</param>
        /// <param name="toSymbol">Indicates whether the data unit should be expressed as symbol or full word</param>
        /// <returns>String representation of the provided DataUnit</returns>
        string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true);
    }
}
