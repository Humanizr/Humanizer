namespace Humanizer.Localisation.NumberToWords
{
    internal abstract class GenderedNumberToWordsConverter : INumberToWordsConverter
    {
        private readonly GrammaticalGender _defaultGender;

        protected GenderedNumberToWordsConverter(GrammaticalGender defaultGender = GrammaticalGender.Masculine)
        {
            _defaultGender = defaultGender;
        }

        /// <summary>
        /// Converts the number to string using the locale's default grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string Convert(long number)
        {
            return Convert(number, _defaultGender);
        }

        /// <summary>
        /// Converts the number to string using the locale's default gramatical gender and adds "and"
        /// </summary>
        /// <param name="number"></param>
        /// <param name="addAnd">Whether "and" should be included.</param>
        /// <returns></returns>
        public string Convert(long number, bool addAnd)
        {
            return Convert(number, _defaultGender);
        }

        /// <summary>
        /// Converts the number to string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <param name="addAnd"></param>
        /// <returns></returns>
        public abstract string Convert(long number, GrammaticalGender gender, bool addAnd = true);


        /// <summary>
        /// Converts the number to ordinal string using the locale's default grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string ConvertToOrdinal(int number)
        {
            return ConvertToOrdinal(number, _defaultGender);
        }

        /// <summary>
        /// Converts the number to ordinal string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public abstract string ConvertToOrdinal(int number, GrammaticalGender gender);

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
