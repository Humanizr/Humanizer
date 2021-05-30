namespace Humanizer.Localisation.NumberToWords
{
    /// <summary>
    /// An interface you should implement to localise ToWords and ToOrdinalWords methods
    /// </summary>
    public interface INumberToWordsConverter
    {
        /// <summary>
        /// Converts the number to string using the locale's default grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        string Convert(long number);

        /// <summary>
        /// Converts the number to string using the locale's default grammatical gender with or without adding 'And'
        /// </summary>
        /// <param name="number"></param>
        /// <param name="addAnd">Specify with our without adding "And"</param>
        /// <returns></returns>
        string Convert(long number, bool addAnd);

        /// <summary>
        /// Converts the number to string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <param name="addAnd"></param>
        /// <returns></returns>
        string Convert(long number, GrammaticalGender gender, bool addAnd = true);

        /// <summary>
        /// Converts the number to ordinal string using the locale's default grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        string ConvertToOrdinal(int number);

        /// <summary>
        /// Converts the number to ordinal string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        string ConvertToOrdinal(int number, GrammaticalGender gender);
    }
}
