﻿using Humanizer.Configuration;
using Humanizer.Localisation.Quantifier;
using System.Globalization;
using System.Threading;
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
            return Configurator.Quantifier.ToQuantity(input, quantity, showQuantityAs);
        }
    }
}
