using System.Linq;
using Humanizer.Transformer;

namespace Humanizer
{
    /// <summary>
    /// A portal to string transformation using IStringTransformer
    /// </summary>
    public static class Transformers
    {
        /// <summary>
        /// Transforms a string using the provided transformers. Transformations are applied in the provided order.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="transformers"></param>
        /// <returns></returns>
        public static string TransformWith(this string input, params IStringTransformer[] transformers)
        {
            return transformers.Aggregate(input, (current, stringTransformer) => stringTransformer.Transform(current));
        }

        public static IStringTransformer ToTitleCase
        {
            get
            {
                return new ToTitleCase();
            }
        }

        public static IStringTransformer ToLowerCase
        {
            get
            {
                return new ToLowerCase();
            }
        }

        public static IStringTransformer ToUpperCase
        {
            get
            {
                return new ToUpperCase();
            }
        }

        public static IStringTransformer ToSentenceCase
        {
            get
            {
                return new ToSentenceCase();
            }
        }
    }
}
