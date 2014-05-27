using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.Quantifiers
{
    internal class DefaultQuantifier : IQuantifier
    {
        public string ToQuantity(string input, int quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric, string format = null, IFormatProvider formatProvider = null)
        {
            var transformedInput = TransformInput(input, quantity, showQuantityAs);

            if (showQuantityAs == ShowQuantityAs.None)
                return transformedInput;

            if (showQuantityAs == ShowQuantityAs.Numeric)
                return string.Format(formatProvider, "{0} {1}", quantity.ToString(format, formatProvider), transformedInput);

            return string.Format("{0} {1}", quantity.ToWords(), transformedInput);
        }

        protected virtual string TransformInput(string input, int quantity, ShowQuantityAs showQuantityAs)
        {
            return quantity == 1
                ? input.Singularize(Plurality.CouldBeEither)
                : input.Pluralize(Plurality.CouldBeEither);

        }
    }
}
