namespace Humanizer.Localisation.NumberToWords
{
    abstract class GenderedNumberToWordsConverter : INumberToWordsConverter
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
        public string Convert(int number)
        {
            return Convert(number, _defaultGender);
        }

        /// <summary>
        /// Converts the number to string using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public abstract string Convert(int number, GrammaticalGender gender);

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
    }
}
