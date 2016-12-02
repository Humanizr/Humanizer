// Wrote by Alois de Gouvello https://github.com/aloisdg

// The MIT License (MIT)

// Copyright (c) 2015 Alois de Gouvello

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    internal class EsperantoNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap =
        {
            "nul", "unu", "du", "tri", "kvar", "kvin", "ses", "sep", "ok", "naŭ"
        };

        private static readonly Dictionary<int, string> TensMap = new Dictionary<int, string>
        {
            { 1000000000, " miliardo" }, { 1000000 , " miliono" }, { 1000, " mil" }, { 100, "cent" }, { 10, "dek" }, { 1, null }
        };

        private const string Minus = "minus";

        public override string ConvertToOrdinal(int number)
        {
            return Convert(number) + "a";
        }

        public override string Convert(int number)
        {
            var words = number < 0 ? Minus + " " : string.Empty;
            number = Math.Abs (number);
            words += number <= 1 ? UnitsMap[number] : ConvertInside(number);
            return words;
        }

        private static string ConvertInside(int number)
        {
            if (number == 1)
                return string.Empty;

            var parts = new List<string> ();
            foreach (var item in TensMap.Where(x => number / x.Key > 0))
            {
                parts.Add(!string.IsNullOrEmpty(item.Value)
                    ? $"{ConvertInside (number / item.Key)}{TrimMil (item, number)}"
                    : UnitsMap[number]);
                number %= item.Key;
            }

            var toWords = string.Join(" ", parts.ToArray ()).TrimStart(' ');
            return toWords;
        }

        private static string TrimMil(KeyValuePair<int, string> item, int number)
        {
            if (item.Key != 1000)
                return item.Value;
            var quotient = number / item.Key % 10;
            return quotient < 2 || quotient > 9
                ? item.Value
                : item.Value.TrimStart(' ');
        }
    }
}
