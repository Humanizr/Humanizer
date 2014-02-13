using Humanizer.Pluralization;
namespace Humanizer
{
    public enum ShowQuantityAs
    {
        None = 0,
        Numeric,
        Words
    }

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
        /// "men".ToQuantity(1) => "1 man"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, int quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric)
        {
            var service = new EnglishPluralizationService();

            var transformedInput = quantity == 1 ? service.Singularize(input) : service.Pluralize(input);

            if (showQuantityAs == ShowQuantityAs.None)
                return transformedInput;

            if (showQuantityAs == ShowQuantityAs.Numeric)
                return string.Format("{0} {1}", quantity, transformedInput);

            return string.Format("{0} {1}", quantity.ToWords(), transformedInput);
        }
    }
}
