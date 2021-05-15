using System.Globalization;
using System.Linq;

namespace Humanizer
{
    /// <summary>
    /// A portal to string transformation using IStringTransformer
    /// </summary>
    public static class To
    {
        /// <summary>
        /// Transforms a string using the provided transformers. Transformations are applied in the provided order.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="transformers"></param>
        /// <returns></returns>
        public static string Transform(this string input, params IStringTransformer[] transformers)
        {
            return transformers.Aggregate(input, (current, stringTransformer) => stringTransformer.Transform(current));
        }

        /// <summary>
        /// Transforms a string using the provided transformers. Transformations are applied in the provided order.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="culture"></param>
        /// <param name="transformers"></param>
        /// <returns></returns>
        public static string Transform(this string input, CultureInfo culture, params ICulturedStringTransformer[] transformers)
        {
            return transformers.Aggregate(input, (current, stringTransformer) => stringTransformer.Transform(current, culture));
        }

        /// <summary>
        /// Changes string to title case
        /// </summary>
        /// <example>
        /// "INvalid caSEs arE corrected" -> "Invalid Cases Are Corrected"
        /// </example>
        public static ICulturedStringTransformer TitleCase
        {
            get
            {
                return new ToTitleCase();
            }
        }

        /// <summary>
        /// Changes the string to lower case
        /// </summary>
        /// <example>
        /// "Sentence casing" -> "sentence casing"
        /// </example>
        public static ICulturedStringTransformer LowerCase
        {
            get
            {
                return new ToLowerCase();
            }
        }

        /// <summary>
        /// Changes the string to upper case
        /// </summary>
        /// <example>
        /// "lower case statement" -> "LOWER CASE STATEMENT"
        /// </example>
        public static ICulturedStringTransformer UpperCase
        {
            get
            {
                return new ToUpperCase();
            }
        }

        /// <summary>
        /// Changes the string to sentence case
        /// </summary>
        /// <example>
        /// "lower case statement" -> "Lower case statement"
        /// </example>
        public static ICulturedStringTransformer SentenceCase
        {
            get
            {
                return new ToSentenceCase();
            }
        }
    }
}
