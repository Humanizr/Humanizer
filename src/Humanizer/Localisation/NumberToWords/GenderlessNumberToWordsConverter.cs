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

        public virtual string Convert(long number, WordForm wordForm)
        {
            return Convert(number);
        }

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

        public virtual string Convert(long number, bool addAnd, WordForm wordForm)
        {
            return Convert(number, wordForm);
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

        public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
        {
            return Convert(number, gender, addAnd);
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

        /// <summary>
        /// Converts integer to named tuple (e.g. 'single', 'double' etc.).
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public virtual string ConvertToTuple(int number)
        {
            return Convert(number);
        }
    }
}
