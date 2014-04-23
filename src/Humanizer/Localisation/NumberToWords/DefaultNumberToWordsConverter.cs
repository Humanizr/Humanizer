namespace Humanizer.Localisation.NumberToWords
{
    internal class DefaultNumberToWordsConverter : INumberToWordsConverter
    {
        /// <summary>
        /// for Russian locale
        /// 1.ToWords(GrammaticalGender.Masculine) -> "один"
        /// 1.ToWords(GrammaticalGender.Feminine) -> "одна"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        public virtual string Convert(int number, GrammaticalGender gender)
        {
            return Convert(number);
        }

        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <returns></returns>
        public virtual string Convert(int number)
        {
            return number.ToString();
        }

        /// <summary>
        /// for Brazilian Portuguese
        /// 1.ToOrdinalWords(GrammaticalGender.Masculine) -> "primeiro"
        /// 1.ToOrdinalWords(GrammaticalGender.Feminine) -> "primeira"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        public virtual string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return ConvertToOrdinal(number);
        }

        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <returns></returns>
        public virtual string ConvertToOrdinal(int number)
        {
            return number.ToString();
        }
    }
}