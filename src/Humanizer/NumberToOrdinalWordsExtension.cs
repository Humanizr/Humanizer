using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.NumberToWords;

namespace Humanizer
{
    /// <summary>
    /// Transforms a number into ordinal words; e.g. 1 => first
    /// </summary>
    public static class NumberToOrdinalWordsExtension
    {
        private static readonly IDictionary<string, Func<INumberToWordsConverter>> ConverterFactories =
    new Dictionary<string, Func<INumberToWordsConverter>>
            {
                { "en", () => new EnglishNumberToWordsConverter() },
                { "it", () => new ItalianNumberToWordsConverter() },
            };


        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <returns></returns>
        public static string ToOrdinalWords(this int number)
        {
            return Converter.ConvertToOrdinal(number);
        }

        private static INumberToWordsConverter Converter
        {
            get
            {
                Func<INumberToWordsConverter> converterFactory;
                if (ConverterFactories.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out converterFactory))
                    return converterFactory();

                return new DefaultNumberToWordsConverter();
            }
        }
    }
}
