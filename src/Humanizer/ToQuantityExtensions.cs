namespace Humanizer
{
    public static class ToQuantityExtensions
    {
        /// <summary>
        /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
        /// </summary>
        /// <param name="input">The word to be prefixes</param>
        /// <param name="quantity">The quantity of the word</param>
        /// <example>
        /// "request".ToQuantity(0) => "0 requests"
        /// "request".ToQuantity(1) => "1 request"
        /// "request".ToQuantity(2) => "2 requests"
        /// "men".ToQuantity(2) => "2 men"
        /// "men".ToQuantity(1) => "1 man"
        /// </example>
        /// <returns></returns>
        public static string ToQuantity(this string input, int quantity)
        {
            return string.Format("{0} {1}", quantity, quantity == 1 ? input.Singularize(Plurality.CouldBeEither) : input.Pluralize(Plurality.CouldBeEither));
        }
    }
}
