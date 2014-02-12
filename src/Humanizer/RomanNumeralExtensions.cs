using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer
{
    public static class RomanNumeralExtensions
    {
        private const int NumberOfRomanNumeralMaps = 13;

        private static readonly Dictionary<string, int> romanNumerals =
            new Dictionary<string, int>(NumberOfRomanNumeralMaps)
            {
                { "M", 1000 }, 
                { "CM", 900 }, 
                { "D", 500 }, 
                { "CD", 400 }, 
                { "C", 100 }, 
                { "XC", 90 }, 
                { "L", 50 }, 
                { "XL", 40 }, 
                { "X", 10 }, 
                { "IX", 9 }, 
                { "V", 5 }, 
                { "IV", 4 }, 
                { "I", 1 }
            };


        private static readonly Regex validRomanNumeral = new Regex("^(?i:(?=[MDCLXVI])((M{0,3})((C[DM])|(D?C{0,3}))" + "?((X[LC])|(L?XX{0,2})|L)?((I[VX])|(V?(II{0,2}))|V)?))$", RegexOptions.None);

        public static bool IsValidRomanNumeral(this string value)
        {
            return validRomanNumeral.IsMatch(value);
        }

        public static int FromRoman(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            value = value.ToUpperInvariant().Trim();


            var length = value.Length;


            if ((length == 0) || !value.IsValidRomanNumeral())
            {
                throw new ArgumentException("Empty or invalid Roman numeral string.", "value");
            }


            var total = 0;
            var i = length;


            while (i > 0)
            {
                var digit = romanNumerals[value[--i].ToString()];


                if (i > 0)
                {
                    var previousDigit = romanNumerals[value[i - 1].ToString()];


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

        public static string ToRoman(this int value)
        {
            const int MinValue = 1;
            const int MaxValue = 3999;


            if ((value < MinValue) || (value > MaxValue))
            {
                throw new ArgumentOutOfRangeException();
            }


            const int MaxRomanNumeralLength = 15;
            var sb = new StringBuilder(MaxRomanNumeralLength);


            foreach (var pair in romanNumerals)
            {
                while (value / pair.Value > 0)
                {
                    sb.Append(pair.Key);
                    value -= pair.Value;
                }
            }


            return sb.ToString();
        }
    }
}
