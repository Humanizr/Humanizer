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
        /// Converts the number to specific string form using the locale's default grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        public string Convert(long number, WordForm wordForm)
        {
            return Convert(number, wordForm, _defaultGender);
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
        /// Converts the number to a specific string form using the locale's default gramatical gender and adds "and"
        /// </summary>
        /// <param name="number"></param>
        /// <param name="addAnd"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        public string Convert(long number, bool addAnd, WordForm wordForm)
        {
            return Convert(number, wordForm, _defaultGender, addAnd);
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
        /// Converts the number to a specific string form using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="wordForm"></param>
        /// <param name="gender"></param>
        /// <param name="addAnd"></param>
        /// <returns></returns>
        public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
        {
            return Convert(number, gender, addAnd);
        }

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
        /// Converts the number to specific ordinal string form
        /// </summary>
        /// <param name="number"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        public string ConvertToOrdinal(int number, WordForm wordForm)
        {
            return ConvertToOrdinal(number, _defaultGender, wordForm);
        }

        /// <summary>
        /// Converts the number to specific ordinal string form using the provided grammatical gender
        /// </summary>
        /// <param name="number"></param>
        /// <param name="gender"></param>
        /// <param name="wordForm"></param>
        /// <returns></returns>
        public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
        {
            return ConvertToOrdinal(number, gender);
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
