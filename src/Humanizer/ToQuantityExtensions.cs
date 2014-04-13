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
        /// <param name="input">The word to be prefixes</param>
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
            var transformedInput = quantity == 1
                ? input.Singularize(Plurality.CouldBeEither)
                : input.Pluralize(Plurality.CouldBeEither);

            if (showQuantityAs == ShowQuantityAs.None)
                return transformedInput;

            if (showQuantityAs == ShowQuantityAs.Numeric)
                return string.Format("{0} {1}", quantity, transformedInput);

            return string.Format("{0} {1}", quantity.ToWords(), transformedInput);
        }
    }
}
