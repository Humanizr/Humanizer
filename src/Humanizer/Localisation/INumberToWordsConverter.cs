namespace Humanizer.Localisation
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
    }
}