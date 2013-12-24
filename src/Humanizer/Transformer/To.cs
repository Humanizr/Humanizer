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

        public static IStringTransformer TitleCase
        {
            get
            {
                return new ToTitleCase();
            }
        }

        public static IStringTransformer LowerCase
        {
            get
            {
                return new ToLowerCase();
            }
        }

        public static IStringTransformer UpperCase
        {
            get
            {
                return new ToUpperCase();
            }
        }

        public static IStringTransformer SentenceCase
        {
            get
            {
                return new ToSentenceCase();
            }
        }
    }
}
