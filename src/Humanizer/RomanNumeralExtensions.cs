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

        private static readonly IDictionary<string, int> LatinRomanNumerals =
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

        private static readonly IDictionary<string, int> RomanNumerals =
            new Dictionary<string, int>(NumberOfRomanNumeralMaps)
            {
                { "Ⅿ",  1000 },
                { "ⅭⅯ", 900 },
                { "Ⅾ",  500 },
                { "ⅭⅮ", 400 },
                { "Ⅽ",  100 },
                { "ⅩⅭ", 90 },
                { "Ⅼ",  50 },
                { "ⅩⅬ", 40 },
                { "Ⅹ",  10 },
                { "ⅠⅩ", 9 },
                { "Ⅴ",  5 },
                { "ⅠⅤ", 4 },
                { "Ⅰ",  1 }
            };

        private static readonly IDictionary<int, string> PreComposedRomanNumerals =
            new Dictionary<int, string>(NumberOfRomanNumeralMaps)
            {
                { 12, "Ⅻ" },
                { 11, "Ⅺ" },
                { 9, "Ⅸ" },
                { 8, "Ⅷ" },
                { 7, "Ⅶ" },
                { 6, "Ⅵ" },
                { 4, "Ⅳ" },
                { 3, "Ⅲ" },
                { 2, "Ⅱ" }
            };

        private static readonly Regex ValidRomanNumeral =
            new Regex(
                "^(?i:(?=[MDCLXVI])((M{0,3})((C[DM])|(D?C{0,3}))?((X[LC])|(L?XX{0,2})|L)?((I[VX])|(V?(II{0,2}))|V)?))$",
                RegexOptionsUtil.Compiled);

        private static readonly Regex RomanNumeralCharacters =
            new Regex(
                "[ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅬⅭⅮⅯ]",
                RegexOptionsUtil.Compiled);

        /// <summary>
        /// Converts Roman numbers into integer
        /// </summary>
        /// <param name="input">Roman number</param>
        /// <returns>Human-readable number</returns>
        public static int FromRoman(this string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            input = RomanNumeralCharacters.Replace(input.Trim().ToUpperInvariant(), match =>
            {
                switch (match.Value)
                {
                    case "Ⅰ":
                        return "I";
                    case "Ⅱ":
                        return "II";
                    case "Ⅲ":
                        return "III";
                    case "Ⅳ":
                        return "IV";
                    case "Ⅴ":
                        return "V";
                    case "Ⅵ":
                        return "VI";
                    case "Ⅶ":
                        return "VII";
                    case "Ⅷ":
                        return "VIII";
                    case "Ⅸ":
                        return "IX";
                    case "Ⅹ":
                        return "X";
                    case "Ⅺ":
                        return "XI";
                    case "Ⅼ":
                        return "L";
                    case "Ⅽ":
                        return "C";
                    case "Ⅾ":
                        return "D";
                    case "Ⅿ":
                        return "M";
                    default:
                        return match.Value;
                }
            });

            var length = input.Length;

            if ((length == 0) || IsInvalidRomanNumeral(input))
                throw new ArgumentException("Empty or invalid Roman numeral string.", "input");

            var total = 0;
            var i     = length;

            while (i > 0)
            {
                var digit = LatinRomanNumerals[input[--i].ToString()];

                if (i > 0)
                {
                    var previousDigit = LatinRomanNumerals[input[i - 1].ToString()];

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
        /// <param name="characters">The characters used for Roman numerals</param>
        /// <returns>Roman number</returns>
        public static string ToRoman(this int input, RomanNumeralCharacters characters = Humanizer.RomanNumeralCharacters.Latin)
        {
            const int minValue = 1;
            const int maxValue = 3999;
            const int maxRomanNumeralLength = 15;

            if ((input < minValue) || (input > maxValue))
                throw new ArgumentOutOfRangeException();

            if (characters == Humanizer.RomanNumeralCharacters.RomanNumeralsPreComposed && PreComposedRomanNumerals.ContainsKey(input))
                return PreComposedRomanNumerals[input];

            var sb = new StringBuilder(maxRomanNumeralLength);

            foreach (var pair in characters == Humanizer.RomanNumeralCharacters.Latin ? LatinRomanNumerals : RomanNumerals)
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

    public enum RomanNumeralCharacters
    {
        Latin,
        RomanNumerals,
        RomanNumeralsPreComposed
    }
}