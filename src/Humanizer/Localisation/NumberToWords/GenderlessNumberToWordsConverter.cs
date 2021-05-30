namespace Humanizer.Localisation.NumberToWords
{
    internal abstract class GenderlessNumberToWordsConverter : INumberToWordsConverter
    {
        /// <summary>
        /// Converts the number to string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public abstract string Convert(long number);

        /// <summary>
        /// Converts the number to string
        /// </summary>
        /// <param name="number"></param>
        /// <param name="addAnd">Whether "and" should be included.</param>
        /// <returns></returns>
        public virtual string Convert(long number, bool addAnd)
        {
            return Convert(number);
        }

        /// <summary>
        /// Converts the number to string ignoring the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <param name="addAnd"></param>
        /// <returns></returns>
        public virtual string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        {
            return Convert(number);
        }


        /// <summary>
        /// Converts the number to ordinal string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public abstract string ConvertToOrdinal(int number);

        /// <summary>
        /// Converts the number to ordinal string ignoring  the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return ConvertToOrdinal(number);
        }
    }
}
