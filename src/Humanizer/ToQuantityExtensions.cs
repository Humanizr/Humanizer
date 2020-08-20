using System;
namespace Humanizer
{
    /// <summary>
    /// Enumerates the ways of displaying a quantity value when converting
    /// a word to a quantity string.
    /// </summary>
    public enum ShowQuantityAs
    {
        /// <summary>
        /// Indicates that no quantity will be included in the formatted string.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the quantity will be included in the output, formatted
        /// as its numeric value (e.g. "1").
        /// </summary>
        Numeric,

        /// <summary>
        /// Incidates that the quantity will be included in the output, formatted as
        /// words (e.g. 123 => "one hundred and twenty three").
        /// </summary>
        Words
    }

    /// <summary>
    /// Provides extensions for formatting a <see cref="string"/> word as a quantity.
    /// </summary>
    public static class ToQuantityExtensions
    {
        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixed</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <param name="showQuantityAs">How to show the quantity. Numeric by default</param>
        /// <example>
        /// "request".ToQuantity(0) => "0 requests"
        /// "request".ToQuantity(1) => "1 request"
        /// "request".ToQuantity(2) => "2 requests"
        /// "men".ToQuantity(2) => "2 men"
        /// "process".ToQuantity(1200, ShowQuantityAs.Words) => "one thousand two hundred processes"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, int quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric)
        {
            return input.ToQuantity(quantity, showQuantityAs, format: null, formatProvider: null);
        }

        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixed</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <example>
        /// "request".ToQuantity(0) => "0 requests"
        /// "request".ToQuantity(10000, format: "N0") => "10,000 requests"
        /// "request".ToQuantity(1, format: "N0") => "1 request"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, int quantity, string format, IFormatProvider formatProvider = null)
        {
            return input.ToQuantity(quantity, showQuantityAs: ShowQuantityAs.Numeric, format: format, formatProvider: formatProvider);
        }

        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixed</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <param name="showQuantityAs">How to show the quantity. Numeric by default</param>
        /// <example>
        /// "request".ToQuantity(0) => "0 requests"
        /// "request".ToQuantity(1) => "1 request"
        /// "request".ToQuantity(2) => "2 requests"
        /// "men".ToQuantity(2) => "2 men"
        /// "process".ToQuantity(1200, ShowQuantityAs.Words) => "one thousand two hundred processes"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, long quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric)
        {
            return input.ToQuantity(quantity, showQuantityAs, format: null, formatProvider: null);
        }

        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixed</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <example>
        /// "request".ToQuantity(0) => "0 requests"
        /// "request".ToQuantity(10000, format: "N0") => "10,000 requests"
        /// "request".ToQuantity(1, format: "N0") => "1 request"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, long quantity, string format, IFormatProvider formatProvider = null)
        {
            return input.ToQuantity(quantity, showQuantityAs: ShowQuantityAs.Numeric, format: format, formatProvider: formatProvider);
        }

        private static string ToQuantity(this string input, long quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric, string format = null, IFormatProvider formatProvider = null)
        {
            var transformedInput = quantity == 1
                ? input.Singularize(inputIsKnownToBePlural: false)
                : input.Pluralize(inputIsKnownToBeSingular: false);

            if (showQuantityAs == ShowQuantityAs.None)
            {
                return transformedInput;
            }

            if (showQuantityAs == ShowQuantityAs.Numeric)
            {
                return string.Format(formatProvider, "{0} {1}", quantity.ToString(format, formatProvider), transformedInput);
            }

            return string.Format("{0} {1}", quantity.ToWords(), transformedInput);
        }
        
        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixed</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <example>
        /// "request".ToQuantity(0.2) => "0.2 requests"
        /// "request".ToQuantity(10.6, format: "N0") => "10.6 requests"
        /// "request".ToQuantity(1.0, format: "N0") => "1 request"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, double quantity, string format = null, IFormatProvider formatProvider = null)
        {
            var transformedInput = quantity == 1
                ? input.Singularize(inputIsKnownToBePlural: false)
                : input.Pluralize(inputIsKnownToBeSingular: false);

            return string.Format(formatProvider, "{0} {1}", quantity.ToString(format, formatProvider), transformedInput);

        }

        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixed</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <example>
        /// "request".ToQuantity(0.2) => "0.2 requests"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, double quantity)
        {
            return ToQuantity(input, quantity, null, null);
        }
    }
}
