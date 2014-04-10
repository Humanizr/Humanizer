using Humanizer.Localisation.NumberToWords;

namespace Humanizer
{
    /// <summary>
    /// Transforms a number into ordinal words; e.g. 1 => first
    /// </summary>
    public static class NumberToOrdinalWordsExtension
    {
        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <returns></returns>
        public static string ToOrdinalWords(this int number)
        {
            return new EnglishNumberToWordsConverter().ConvertToOrdinal(number);
        }
    }
}
