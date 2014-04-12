using System.Collections.Generic;
using System.Globalization;

    /// <summary>
    /// Transform a number into words; e.g. 1 => one
    /// </summary>
    public static class NumberToWordsExtension {
        private static readonly IDictionary<string, Func<INumberToWordsConverter>> ConverterFactories =
            new Dictionary<string, Func<INumberToWordsConverter>>
            {
                { "en", () => new EnglishNumberToWordsConverter() },
                { "ar", () => new ArabicNumberToWordsConverter() },
                { "fa", () => new FarsiNumberToWordsConverter() },
                { "es", () => new SpanishNumberToWordsConverter() },
                { "pl", () => new PolishNumberToWordsConverter() }
                { "pt-BR", () => new BrazilianPortugueseNumberToWordsConverter() },
            };

        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number">Number to be turned to words</param>
        /// <returns></returns>
        public static string ToWords(this int number) 
        {
            return Converter.Convert(number);
        }

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

                if (ConverterFactories.TryGetValue(CultureInfo.CurrentUICulture.Name, out converterFactory))
                    return converterFactory();

                if (ConverterFactories.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out converterFactory))
                    return converterFactory();

                return new DefaultNumberToWordsConverter();
            }
        }
    }
}
