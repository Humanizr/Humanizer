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

namespace Humanizer
{
        /// <summary>
        /// Contains extension methods for changing a number to Metric representation (ToMetric)
        /// and from Metric representation back to the number (FromMetric)
        /// </summary>
        public static class MetricNumeralExtensions
        {
                /// <summary>
                /// Symbols is a list of every symbols for the Metric system.
                /// </summary>
                private static readonly char[][] Symbols =
                {
                        new[] { 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' },
                        new[] { 'm', '\u03bc', 'n', 'p', 'f', 'a', 'z', 'y' }
                };

                //private static readonly Dictionary<char, string> Names = new Dictionary<char, string>()
                //{
                //         {"Y", "yotta" },
                //         {"Z", "zetta" },
                //         {"E", "exa" },
                //         {"P", "peta" },
                //         {"T", "tera" },
                //         {"G", "giga" },
                //         {"M", "mega" },
                //         {"k", "kilo" },
                //         //{"h", "hecto"},
                //         //{"e", "deca" }, // "da" is read as 'e'
                //         //{"d", "deci" },
                //         //{"c", "centi"},
                //         {"m", "milli" },
                //         {"μ", "micro" },
                //         {"n", "nano" },
                //         {"p", "pico" },
                //         {"f", "femto" },
                //         {"a", "atto" },
                //         {"z", "zepto" },
                //         {"y", "yocto" }
                //};



                /// <summary>
                /// Converts a Metric representation into a number.
                /// </summary>
                /// <param name="input">Metric representation to convert to a number</param>
                /// <example>
                /// "1k".FromMetric() => 1000d
                /// "123".FromMetric() => 123d
                /// "100m".FromMetric() => 1E-1
                /// </example>
                /// <returns>A number after a conversion from a Metric representation.</returns>
                public static double FromMetric(this string input)
                {
                        if (input == null) throw new ArgumentNullException("input");
                        input = input.Trim();
                        if (input.Length == 0 || input.IsInvalidMetricNumeral())
                                throw new ArgumentException("Empty or invalid Metric string.", "input");
                        input = input.Replace(" ", String.Empty);
                        var last = input[input.Length - 1];
                        if (!Char.IsLetter(last)) return Double.Parse(input);
                        Func<char[], double> getExponent = symbols => (symbols.IndexOf(last) + 1) * 3;
                        var number = Double.Parse(input.Remove(input.Length - 1));
                        var exponent = Math.Pow(10, Symbols[0].Contains(last) ? getExponent(Symbols[0]) : -getExponent(Symbols[1]));
                        return number * exponent;
                }

                /// <summary>
                /// Converts a number into a valid and Human-readable Metric representation.
                /// </summary>
                /// <remarks>
                /// Inspired by a snippet from Thom Smith.
                /// <see cref="http://stackoverflow.com/questions/12181024/formatting-a-number-with-a-metric-prefix"/>
                /// </remarks>
                /// <param name="input">Number to convert to a Metric representation.</param>
                /// <param name="isSplitedBySpace">True will split the number and the symbol with a whitespace.</param>
                /// <example>
                /// 1000d.ToMetric() => "1k"
                /// 123d.ToMetric() => "123"
                /// 1E-1.ToMetric() => "100m"
                /// </example>
                /// <returns>A valid Metric representation</returns>
                public static string ToMetric(this double input, bool isSplitedBySpace = false)
                {
                        if (input.Equals(0)) return input.ToString();
                        if (input.IsOutOfRange()) throw new ArgumentOutOfRangeException("input");
                        var exponent = (int)Math.Floor(Math.Log10(Math.Abs(input)) / 3);
                        if (exponent == 0) return input.ToString();
                        return input * Math.Pow(1000, -exponent)
                                + (isSplitedBySpace ? " " : String.Empty)
                                + (Math.Sign(exponent) == 1 ? Symbols[0][exponent - 1] : Symbols[1][-exponent - 1]);
                }

                /// <summary>
                /// Check if a Metric representation is out of the valid range.
                /// </summary>
                /// <param name="input">A Metric representation who might be out of the valid range.</param>
                /// <returns>True if input is out of the valid range.</returns>
                private static bool IsOutOfRange(this double input)
                {
                        const int limit = 27;
                        var bigLimit = Math.Pow(10, limit);
                        var smallLimit = Math.Pow(10, -limit);
                        Func<double, double, bool> outside = (min, max) => !(max > input && input > min);
                        return (Math.Sign(input) == 1 && outside(smallLimit, bigLimit))
                               || (Math.Sign(input) == -1 && outside(-bigLimit, -smallLimit));
                }

                /// <summary>
                /// Check if a string is not a valid Metric representation.
                /// A valid representation is in the format "{0}{1}" or "{0} {1}"
                /// where {0} is a number and {1} is an allowed symbol.
                /// </summary>
                /// <remarks>
                /// ToDo: Performance: Use (string input, out number) to escape the double use of Parse()
                /// </remarks>
                /// <param name="input">A string who might contain a invalid Metric representation.</param>
                /// <returns>True if input is not a valid Metric representation.</returns>
                private static bool IsInvalidMetricNumeral(this string input)
                {
                        double number;
                        var index = input.Length - 1;
                        var last = input[index];
                        var isSymbol = Symbols[0].Contains(last) || Symbols[1].Contains(last);
                        return !Double.TryParse(isSymbol ? input.Remove(index) : input, out number);
                }

                /// <summary>
                /// Reports the zero-based index of the first occurrence of the specified Unicode
                /// character in this string.
                /// </summary>
                /// <param name="chars">The string containing the value.</param>
                /// <param name="value">A Unicode character to seek.</param>
                /// <returns>
                /// The zero-based index position of value if that character is found, or -1 if it is not.
                /// </returns>
                private static int IndexOf(this ICollection<char> chars, char value)
                {
                        for (var i = 0; i < chars.Count; i++)
                                if (chars.ElementAt(i).Equals(value))
                                        return i;
                        return -1;
                }
        }
}