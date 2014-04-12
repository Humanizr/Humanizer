namespace Humanizer.Localisation.NumberToWords
{
    /// <summary>
    /// An abstraction to convert number to words
    /// </summary>
    internal interface INumberToWordsConverter
    {
        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <returns></returns>
        string Convert(int number);

        /// <summary>
        /// for Russian locale
        /// 1.ToWords(GrammaticalGender.Masculine) -> "один"
        /// 1.ToWords(GrammaticalGender.Feminine) -> "одна"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        string Convert(int number, GrammaticalGender gender);

        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <returns></returns>
        string ConvertToOrdinal(int number);
    }
}