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
        /// Converts the number to a specific string form using the locale's default grammatical gender.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        string Convert(long number, WordForm wordForm);

        /// <summary>
        /// Converts the number to string using the locale's default grammatical gender with or without adding 'And'
        /// </summary>
        /// <param name="number"></param>
        /// <param name="addAnd">Specify with our without adding "And"</param>
        /// <returns></returns>
        string Convert(long number, bool addAnd);

        /// <summary>
        /// Converts the number to a specific string form using the locale's default grammatical gender with or without adding 'And'
        /// </summary>
        /// <param name="number"></param>
        /// <param name="addAnd"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        string Convert(long number, bool addAnd, WordForm wordForm);

        /// <summary>
        /// Converts the number to string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <param name="addAnd"></param>
        /// <returns></returns>
        string Convert(long number, GrammaticalGender gender, bool addAnd = true);

        /// <summary>
        /// Converts the number to a specific string form using the provided grammatical gender.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="wordForm"></param>
        /// <param name="gender"></param>
        /// <param name="addAnd"></param>
        /// <returns></returns>
        string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true);

        /// <summary>
        /// Converts the number to ordinal string using the locale's default grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        string ConvertToOrdinal(int number);

        /// <summary>
        /// Converts the number to a specific ordinal string form using the locale's default grammatical gender.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        string ConvertToOrdinal(int number, WordForm wordForm);

        /// <summary>
        /// Converts the number to ordinal string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        string ConvertToOrdinal(int number, GrammaticalGender gender);

        /// <summary>
        /// Converts the number to a specific ordinal string form using the provided grammatical gender.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm);

        /// <summary>
        /// Converts integer to named tuple (e.g. 'single', 'double' etc.).
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        string ConvertToTuple(int number);
    }
}
