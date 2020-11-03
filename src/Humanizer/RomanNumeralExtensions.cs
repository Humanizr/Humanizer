// Done by Jesse Slicer https://github.com/jslicer
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer
{
    /// <summary>
    /// Contains extension methods for changing a number to Roman representation (ToRoman) and from Roman representation back to the number (FromRoman)
    /// </summary>
    public static class RomanNumeralExtensions
    {
        private const int NumberOfRomanNumeralMaps = 13;

        private static readonly IDictionary<string, int> RomanNumerals =
            new Dictionary<string, int>(NumberOfRomanNumeralMaps)
            {
                { "M",  1000 },
                { "CM", 900 },
                { "D",  500 },
                { "CD", 400 },
                { "C",  100 },
                { "XC", 90 },
                { "L",  50 },
                { "XL", 40 },
                { "X",  10 },
                { "IX", 9 },
                { "V",  5 },
                { "IV", 4 },
                { "I",  1 }
            };

        private static readonly Regex ValidRomanNumeral =
            new Regex(
                "^(?i:(?=[MDCLXVI])((M{0,3})((C[DM])|(D?C{0,3}))?((X[LC])|(L?XX{0,2})|L)?((I[VX])|(V?(II{0,2}))|V)?))$",
                RegexOptionsUtil.Compiled);

        /// <summary>
        /// Converts Roman numbers into integer
        /// </summary>
        /// <param name="input">Roman number</param>
        /// <returns>Human-readable number</returns>
        public static int FromRoman(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.Trim().ToUpperInvariant();

            var length = input.Length;

            if ((length == 0) || IsInvalidRomanNumeral(input))
            {
                throw new ArgumentException("Empty or invalid Roman numeral string.", nameof(input));
            }

            var total = 0;
            var i = length;

            while (i > 0)
            {
                var digit = RomanNumerals[input[--i].ToString()];

                if (i > 0)
                {
                    var previousDigit = RomanNumerals[input[i - 1].ToString()];

                    if (previousDigit < digit)
                    {
                        digit -= previousDigit;
                        i--;
                    }
                }

                total += digit;
            }

            return total;
        }

        /// <summary>
        /// Converts the input to Roman number
        /// </summary>
        /// <param name="input">Integer input</param>
        /// <returns>Roman number</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the input is smaller than 1 or larger than 3999</exception>
        public static string ToRoman(this int input)
        {
            const int minValue = 1;
            const int maxValue = 3999;
            const int maxRomanNumeralLength = 15;

            if ((input < minValue) || (input > maxValue))
            {
                throw new ArgumentOutOfRangeException();
            }

            var sb = new StringBuilder(maxRomanNumeralLength);

            foreach (var pair in RomanNumerals)
            {
                while (input / pair.Value > 0)
                {
                    sb.Append(pair.Key);
                    input -= pair.Value;
                }
            }

            return sb.ToString();
        }

        private static bool IsInvalidRomanNumeral(string input)
        {
            return !ValidRomanNumeral.IsMatch(input);
        }
    }
}