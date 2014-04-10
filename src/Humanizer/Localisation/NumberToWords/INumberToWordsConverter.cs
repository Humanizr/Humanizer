namespace Humanizer.Localisation.NumberToWords
{
    /// <summary>
    /// An abstraction to convert number to words
    /// </summary>
    public interface INumberToWordsConverter
    {
        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <returns></returns>
        string Convert(int number);

        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <returns></returns>
        string ConvertToOrdinal(int number);
    }
}