﻿// Wrote by Alois de Gouvello https://github.com/aloisdg

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
        static readonly double BigLimit;
        static readonly double SmallLimit;

	    static MetricNumeralExtensions()
	    {
            const int limit = 27;
            BigLimit = Math.Pow(10, limit);
            SmallLimit = Math.Pow(10, -limit);
        }

		/// <summary>
		/// Symbols is a list of every symbols for the Metric system.
		/// </summary>
		private static readonly List<char>[] Symbols =
                {
                    new List<char> { 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' },
                    new List<char> { 'm', 'μ', 'n', 'p', 'f', 'a', 'z', 'y' }
                };

		/// <summary>
		/// Names link a Metric symbol (as key) to its name (as value).
		/// </summary>
		/// <remarks>
		/// We dont support :
		/// {'h', "hecto"},
		/// {'da', "deca" }, // !string
		/// {'d', "deci" },
		/// {'c', "centi"},
		/// </remarks>
		private static readonly Dictionary<char, string> Names = new Dictionary<char, string>
                {
                         {'Y', "yotta" }, {'Z', "zetta" }, {'E', "exa" }, {'P', "peta" }, {'T', "tera" }, {'G', "giga" }, {'M', "mega" }, {'k', "kilo" },
                         {'m', "milli" }, {'μ', "micro" }, {'n', "nano" }, {'p', "pico" }, {'f', "femto" }, {'a', "atto" }, {'z', "zepto" }, {'y', "yocto" }
                };

		/// <summary>
		/// Converts a Metric representation into a number.
		/// </summary>
		/// <remarks>
		/// We don't support input in the format {number}{name} nor {number} {name}.
		/// We only provide a solution for {number}{symbol} and {number} {symbol}.
		/// </remarks>
		/// <param name="input">Metric representation to convert to a number</param>
		/// <example>
		/// <code>
		/// "1k".FromMetric() => 1000d
		/// "123".FromMetric() => 123d
		/// "100m".FromMetric() => 1E-1
		/// </code>
		/// </example>
		/// <returns>A number after a conversion from a Metric representation.</returns>
		public static double FromMetric(this string input)
		{
			input = CleanRepresentation(input);
			return BuildNumber(input, input[input.Length - 1]);
		}

		/// <summary>
		/// Converts a number into a valid and Human-readable Metric representation.
		/// </summary>
		/// <remarks>
		/// Inspired by a snippet from Thom Smith.
		/// See <a href="http://stackoverflow.com/questions/12181024/formatting-a-number-with-a-metric-prefix">this link</a> for more.
		/// </remarks>
		/// <param name="input">Number to convert to a Metric representation.</param>
		/// <param name="hasSpace">True will split the number and the symbol with a whitespace.</param>
		/// <param name="useSymbol">True will use symbol instead of name</param>
		/// <example>
		/// <code>
		/// 1000.ToMetric() => "1k"
		/// 123.ToMetric() => "123"
		/// 1E-1.ToMetric() => "100m"
		/// </code>
		/// </example>
		/// <returns>A valid Metric representation</returns>
		public static string ToMetric(this int input, bool hasSpace = false, bool useSymbol = true)
		{
			return ((double)input).ToMetric(hasSpace, useSymbol);
		}

        /// <summary>
        /// Converts a number into a valid and Human-readable Metric representation.
        /// </summary>
        /// <remarks>
        /// Inspired by a snippet from Thom Smith.
        /// See <a href="http://stackoverflow.com/questions/12181024/formatting-a-number-with-a-metric-prefix">this link</a> for more.
        /// </remarks>
        /// <param name="input">Number to convert to a Metric representation.</param>
        /// <param name="hasSpace">True will split the number and the symbol with a whitespace.</param>
        /// <param name="useSymbol">True will use symbol instead of name</param>
        /// <example>
        /// <code>
        /// 1000d.ToMetric() => "1k"
        /// 123d.ToMetric() => "123"
        /// 1E-1.ToMetric() => "100m"
        /// </code>
        /// </example>
        /// <returns>A valid Metric representation</returns>
        public static string ToMetric(this double input, bool hasSpace = false, bool useSymbol = true)
		{
			if (input.Equals(0))
				return input.ToString();
			if (input.IsOutOfRange())
				throw new ArgumentOutOfRangeException(nameof(input));

			return BuildRepresentation(input, hasSpace, useSymbol);
		}

		/// <summary>
		/// Clean or handle any wrong input
		/// </summary>
		/// <param name="input">Metric representation to clean</param>
		/// <returns>A cleaned representation</returns>
		private static string CleanRepresentation(string input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));

			input = input.Trim();
			input = ReplaceNameBySymbol(input);
			if (input.Length == 0 || input.IsInvalidMetricNumeral())
				throw new ArgumentException("Empty or invalid Metric string.", nameof(input));

			return input.Replace(" ", string.Empty);
		}

		/// <summary>
		/// Build a number from a metric representation or from a number
		/// </summary>
		/// <param name="input">A Metric representation to parse to a number</param>
		/// <param name="last">The last character of input</param>
		/// <returns>A number build from a Metric representation</returns>
		private static double BuildNumber(string input, char last)
		{
			return char.IsLetter(last)
				? BuildMetricNumber(input, last)
				: double.Parse(input);
		}

		/// <summary>
		/// Build a number from a metric representation
		/// </summary>
		/// <param name="input">A Metric representation to parse to a number</param>
		/// <param name="last">The last character of input</param>
		/// <returns>A number build from a Metric representation</returns>
		private static double BuildMetricNumber(string input, char last)
		{
			Func<List<char>, double> getExponent = symbols => (symbols.IndexOf(last) + 1) * 3;
			var number = double.Parse(input.Remove(input.Length - 1));
            var exponent = Math.Pow(10, Symbols[0].Contains(last)
				? getExponent(Symbols[0])
				: -getExponent(Symbols[1]));
			return number * exponent;
		}

		/// <summary>
		/// Replace every symbol's name by its symbol representation.
		/// </summary>
		/// <param name="input">Metric representation with a name or a symbol</param>
		/// <returns>A metric representation with a symbol</returns>
		private static string ReplaceNameBySymbol(string input)
		{
			return Names.Aggregate(input, (current, name) =>
				current.Replace(name.Value, name.Key.ToString()));
		}

		/// <summary>
		/// Build a Metric representation of the number.
		/// </summary>
		/// <param name="input">Number to convert to a Metric representation.</param>
		/// <param name="hasSpace">True will split the number and the symbol with a whitespace.</param>
		/// <param name="useSymbol">True will use symbol instead of name</param>
		/// <returns>A number in a Metric representation</returns>
		private static string BuildRepresentation(double input, bool hasSpace, bool useSymbol)
		{
			var exponent = (int)Math.Floor(Math.Log10(Math.Abs(input)) / 3);
			return exponent.Equals(0)
				? input.ToString()
				: BuildMetricRepresentation(input, exponent, hasSpace, useSymbol);
		}

		/// <summary>
		/// Build a Metric representation of the number.
		/// </summary>
		/// <param name="input">Number to convert to a Metric representation.</param>
		/// <param name="exponent">Exponent of the number in a scientific notation</param>
		/// <param name="hasSpace">True will split the number and the symbol with a whitespace.</param>
		/// <param name="useSymbol">True will use symbol instead of name</param>
		/// <returns>A number in a Metric representation</returns>
		private static string BuildMetricRepresentation(double input, int exponent, bool hasSpace, bool useSymbol)
		{
			var number = input * Math.Pow(1000, -exponent);
			var symbol = Math.Sign(exponent) == 1
				? Symbols[0][exponent - 1]
				: Symbols[1][-exponent - 1];
			return number
				+ (hasSpace ? " " : string.Empty)
				+ GetUnit(symbol, useSymbol);
		}

		/// <summary>
		/// Get the unit from a symbol of from the symbol's name.
		/// </summary>
		/// <param name="symbol">The symbol linked to the unit</param>
		/// <param name="useSymbol">True will use symbol instead of name</param>
		/// <returns>A symbol or a symbol's name</returns>
		private static string GetUnit(char symbol, bool useSymbol)
		{
			return useSymbol ? symbol.ToString() : Names[symbol];
		}

		/// <summary>
		/// Check if a Metric representation is out of the valid range.
		/// </summary>
		/// <param name="input">A Metric representation who might be out of the valid range.</param>
		/// <returns>True if input is out of the valid range.</returns>
		private static bool IsOutOfRange(this double input)
		{
			Func<double, double, bool> outside = (min, max) => !(max > input && input > min);

			return (Math.Sign(input) == 1 && outside(SmallLimit, BigLimit))
			       || (Math.Sign(input) == -1 && outside(-BigLimit, -SmallLimit));
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
			return !double.TryParse(isSymbol ? input.Remove(index) : input, out number);
		}
	}
}