namespace Humanizer
{
    public static class ToQuantityExtensions
    {
        /// <summary>
        /// "request".ToQuantity(0) => "0 requests", "request".ToQuantity(1) => "1 request", "request".ToQuantity(2) => "2 requests"
        /// </summary>
        /// <param name="input"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static string ToQuantity(this string input, int quantity)
        {
            if (quantity == 1)
                return string.Format("{0} {1}", quantity, input);

            return string.Format("{0} {1}", quantity, input.Pluralize());
        }
    }
}
