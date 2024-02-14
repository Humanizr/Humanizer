﻿namespace Humanizer.Localisation.NumberToWords
{
    internal abstract class GenderlessNumberToWordsConverter : INumberToWordsConverter
    {
        /// <summary>
        /// Converts the number to string
        /// </summary>
        public abstract string Convert(long number);

        /// <summary>
        /// Converts the number to a specific string form
        /// </summary>
        public string Convert(long number, WordForm wordForm)
        {
            return Convert(number);
        }

        /// <summary>
        /// Converts the number to string
        /// </summary>
        /// <param name="addAnd">Whether "and" should be included.</param>
        public virtual string Convert(long number, bool addAnd)
        {
            return Convert(number);
        }

        /// <summary>
        /// Converts the number to a specific string form
        /// </summary>
        /// <param name="addAnd">Whether "and" should be included.</param>
        public string Convert(long number, bool addAnd, WordForm wordForm)
        {
            return Convert(number, wordForm);
        }

        /// <summary>
        /// Converts the number to string ignoring the provided grammatical gender
        /// </summary>
        public virtual string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        {
            return Convert(number);
        }

        /// <summary>
        /// Converts the number to a specific string form ignoring the provided grammatical gender
        /// </summary>
        public virtual string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
        {
            return Convert(number, addAnd, wordForm);
        }

        /// <summary>
        /// Converts the number to ordinal string
        /// </summary>
        public abstract string ConvertToOrdinal(int number);

        /// <summary>
        /// Converts the number to ordinal string ignoring the provided grammatical gender
        /// </summary>
        public string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return ConvertToOrdinal(number);
        }

        /// <summary>
        /// Converts the number to a specific ordinal string form.
        /// </summary>
        public virtual string ConvertToOrdinal(int number, WordForm wordForm)
        {
            return ConvertToOrdinal(number);
        }

        /// <summary>
        /// Converts the number to a specific ordinal string form ignoring the provided grammatical gender.
        /// </summary>
        public virtual string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
        {
            return ConvertToOrdinal(number, wordForm);
        }

        /// <summary>
        /// Converts integer to named tuple (e.g. 'single', 'double' etc.).
        /// </summary>
        public virtual string ConvertToTuple(int number)
        {
            return Convert(number);
        }
    }
}
