using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.Quantifiers
{
    /// <summary>
    /// The interface used to localise the ToQuantity method
    /// </summary>
    public interface IQuantifier
    {
        /// <summary>
        /// Convert input to quantity
        /// </summary>
        /// <param name="input"></param>
        /// <param name="quantity"></param>
        /// <param name="showQuantityAs"></param>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string ToQuantity(string input, int quantity, ShowQuantityAs showQuantityAs, string format, IFormatProvider formatProvider);
    }
}
