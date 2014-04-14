namespace Humanizer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 
    /// </summary>
    public static class RomanNumeralExtensions
    {
        private const int NumberOfRomanNumeralMaps = 13;

        private static readonly IDictionary<string, int> romanNumerals =
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

        private static readonly Regex ValidRomanNumeral = new Regex(
            "^(?i:(?=[MDCLXVI])((M{0,3})((C[DM])|(D?C{0,3}))?((X[LC])|(L?XX{0,2})|L)?((I[VX])|(V?(II{0,2}))|V)?))$",
            RegexOptions.None);

        /// <summary>
        /// Converts Roman numbers into integer
        /// </summary>
        /// <param name="input">Roman number</param>
        /// <returns>Human-readable number</returns>
        public static int FromRoman(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            input = input.Trim().ToUpperInvariant();

            var length = input.Length;

            if ((length == 0) || IsInvalidRomanNumeral(input))
            {
                throw new ArgumentException("Empty or invalid Roman numeral string.", "input");
            }

            var total = 0;
            var i     = length;

            while (i > 0)
            {
                var digit = romanNumerals[input[--i].ToString()];

                if (i > 0)
                {
                    var previousDigit = romanNumerals[input[i - 1].ToString()];

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
        public static string ToRoman(this int input)
        {
            const int MinValue              = 1;
            const int MaxValue              = 3999;
            const int MaxRomanNumeralLength = 15;

            if ((input < MinValue) || (input > MaxValue))
            {
                throw new ArgumentOutOfRangeException();
            }

            var sb = new StringBuilder(MaxRomanNumeralLength);

            foreach (var pair in romanNumerals)
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
