using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.Quantifiers
{
    public interface IQuantifier
    {
        string ToQuantity(string input, int quantity, ShowQuantityAs showQuantityAs, string format, IFormatProvider formatProvider);
    }
}
