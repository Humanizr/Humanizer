using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords.Romanian
{
    internal class RomanianCardinalNumberConverter
    {

        /// <summary>
        /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9 for 9.
        /// </summary>
        private readonly string[] _units =
            {
                string.Empty,
                "unu|una|unu",
                "doi|două|două",
                "trei",
                "patru",
                "cinci",
                "șase",
                "șapte",
                "opt",
                "nouă"
            };

        /// <summary>
        /// Lookup table converting teens number to text. Index 0 for 10, index 1 for 11, up to index 9 for 19.
        /// </summary>
        private readonly string[] _teensUnder20NumberToText =
            {
                "zece",
                "unsprezece",
                "doisprezece|douăsprezece|douăsprezece",
                "treisprezece",
                "paisprezece",
                "cincisprezece",
                "șaisprezece",
                "șaptesprezece",
                "optsprezece",
                "nouăsprezece"
            };

        /// <summary>
        /// Lookup table converting tens number to text. Index 2 for 20, index 3 for 30, up to index 9 for 90.
        /// </summary>
        private readonly string[] _tensOver20NumberToText =
            {
                string.Empty,
                string.Empty,
                "douăzeci",
                "treizeci",
                "patruzeci",
                "cincizeci",
                "șaizeci",
                "șaptezeci",
                "optzeci",
                "nouăzeci"
            };

        private readonly string _feminineSingular = "o";
        private readonly string _masculineSingular = "un";

        private readonly string _joinGroups = "și";
        private readonly string _joinAbove20 = "de";
        private readonly string _minusSign = "minus";

        /// <summary>
        /// Enumerates sets of three-digits having distinct conversion to text.
        /// </summary>
        private enum ThreeDigitSets
        {
            /// <summary>
            /// Lowest three-digits set, from 1 to 999.
            /// </summary>
            Units = 0,

            /// <summary>
            /// Three-digits set counting the thousands, from 1'000 to 999'000.
            /// </summary>
            Thousands = 1,

            /// <summary>
            /// Three-digits set counting millions, from 1'000'000 to 999'000'000.
            /// </summary>
            Millions = 2,

            /// <summary>
            /// Three-digits set counting billions, from 1'000'000'000 to 999'000'000'000.
            /// </summary>
            Billions = 3,

            /// <summary>
            /// Three-digits set beyond 999 billions, from 1'000'000'000'000 onward.
            /// </summary>
            More = 4
        }

        public string Convert(int number, GrammaticalGender gender)
        {
            if (number == 0)
            {
                return "zero";
            }

            var words = string.Empty;

            var prefixMinusSign = false;

            if (number < 0)
            {
                prefixMinusSign = true;
                number = -number;
            }

            var _threeDigitParts = SplitEveryThreeDigits(number);

            for (var i = 0; i < _threeDigitParts.Count; i++)
            {

                var currentSet = (ThreeDigitSets)Enum.ToObject(typeof(ThreeDigitSets), i);

                var partToString = GetNextPartConverter(currentSet);

                words = partToString(_threeDigitParts[i], gender).Trim() + " " + words.Trim();
            }

            if (prefixMinusSign)
            {
                words = _minusSign + " " + words;
            }

            // remove extra spaces
            return words.TrimEnd().Replace("  ", " ");
        }

        /// <summary>
        /// Splits a number into a sequence of three-digits numbers,
        /// starting from units, then thousands, millions, and so on.
        /// </summary>
        /// <param name="number">The number to split.</param>
        /// <returns>The sequence of three-digit numbers.</returns>
        private List<int> SplitEveryThreeDigits(int number)
        {
            var parts = new List<int>();
            var rest = number;

            while (rest > 0)
            {
                var threeDigit = rest % 1000;

                parts.Add(threeDigit);

                rest = rest / 1000;
            }

            return parts;
        }

        /// <summary>
        /// During number conversion to text, finds out the converter
        /// to use for the next three-digit set.
        /// </summary>
        /// <returns>The next conversion function to use.</returns>
        private Func<int, GrammaticalGender, string> GetNextPartConverter(ThreeDigitSets currentSet)
        {
            Func<int, GrammaticalGender, string> converter;

            switch (currentSet)
            {
                case ThreeDigitSets.Units:
                    converter = UnitsConverter;
                    break;

                case ThreeDigitSets.Thousands:
                    converter = ThousandsConverter;
                    break;

                case ThreeDigitSets.Millions:
                    converter = MillionsConverter;
                    break;

                case ThreeDigitSets.Billions:
                    converter = BillionsConverter;
                    break;

                case ThreeDigitSets.More:
                    converter = null;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Unknow ThreeDigitSet: " + currentSet);
            }

            return converter;
        }

        /// <summary>
        /// Converts a three-digit set to text.
        /// </summary>
        /// <param name="number">The three-digit set to convert.</param>
        /// <param name="gender">The grammatical gender to convert to.</param>
        /// <param name="thisIsLastSet">True if the current three-digit set is the last in the word.</param>
        /// <returns>The same three-digit set expressed as text.</returns>
        private string ThreeDigitSetConverter(int number, GrammaticalGender gender, bool thisIsLastSet = false)
        {
            if (number == 0)
            {
                return string.Empty;
            }

            // grab lowest two digits
            var tensAndUnits = number % 100;
            // grab third digit
            var hundreds = number / 100;

            // grab also first and second digits separately
            var units = tensAndUnits % 10;
            var tens = tensAndUnits / 10;

            var words = string.Empty;

            // append text for hundreds
            words += HundredsToText(hundreds);

            // append text for tens, only those from twenty upward
            words += ((tens >= 2) ? " " : string.Empty) + _tensOver20NumberToText[tens];

            if (tensAndUnits <= 9)
            {
                // simple case for units, under 10
                words += " " + getPartByGender(_units[tensAndUnits], gender);
            }
            else if (tensAndUnits <= 19)
            {
                // special case for 'teens', from 10 to 19
                words += " " + getPartByGender(_teensUnder20NumberToText[tensAndUnits - 10], gender);
            }
            else
            {
                // exception for zero
                var unitsText = (units == 0 ? string.Empty : " " + (_joinGroups + " " + getPartByGender(_units[units], gender)));

                words += unitsText;
            }

            return words;
        }

        private string getPartByGender(string multiGenderPart, GrammaticalGender gender)
        {
            if (multiGenderPart.Contains("|"))
            {
                var parts = multiGenderPart.Split('|');
                if (gender == GrammaticalGender.Feminine)
                {
                    return parts[1];
                }

                if (gender == GrammaticalGender.Neuter)
                {
                    return parts[2];
                }
                else
                {
                    return parts[0];
                }
            }
            else
            {
                return multiGenderPart;
            }
        }

        private bool IsAbove20(int number)
        {
            return (number >= 20);
        }

        private string HundredsToText(int hundreds)
        {
            if (hundreds == 0)
            {
                return string.Empty;
            }
            else if (hundreds == 1)
            {
                return _feminineSingular + " sută";
            }
            else
            {
                return getPartByGender(_units[hundreds], GrammaticalGender.Feminine) + " sute";
            }
        }

        /// <summary>
        /// Converts a three-digit number, as units, to text.
        /// </summary>
        /// <param name="number">The three-digit number, as units, to convert.</param>
        /// <param name="gender">The grammatical gender to convert to.</param>
        /// <returns>The same three-digit number, as units, expressed as text.</returns>
        private string UnitsConverter(int number, GrammaticalGender gender)
        {
            return ThreeDigitSetConverter(number, gender, true);
        }


        /// <summary>
        /// Converts a thousands three-digit number to text.
        /// </summary>
        /// <param name="number">The three-digit number, as thousands, to convert.</param>
        /// <param name="gender">The grammatical gender to convert to.</param>
        /// <returns>The same three-digit number of thousands expressed as text.</returns>
        private string ThousandsConverter(int number, GrammaticalGender gender)
        {
            if (number == 0)
            {
                return string.Empty;
            }
            else if (number == 1)
            {
                return _feminineSingular + " mie";
            }
            else
            {
                return ThreeDigitSetConverter(number, GrammaticalGender.Feminine) + (IsAbove20(number) ? " " + _joinAbove20 : string.Empty) + " mii";
            }
        }

        // Large numbers (above 10^6) use a combined form of the long and short scales.
        /*
                Singular    Plural            Order     Scale
                -----------------------------------------------
                zece        zeci              10^1      -
                sută        sute              10^2      -
                mie         mii               10^3      -
                milion      milioane          10^6      short/long
                miliard     miliarde          10^9      long
                trilion     trilioane         10^12     short
        */

        /// <summary>
        /// Converts a millions three-digit number to text.
        /// </summary>
        /// <param name="number">The three-digit number, as millions, to convert.</param>
        /// <param name="gender">The grammatical gender to convert to.</param>
        /// <returns>The same three-digit number of millions expressed as text.</returns>
        private string MillionsConverter(int number, GrammaticalGender gender)
        {
            if (number == 0)
            {
                return string.Empty;
            }
            else if (number == 1)
            {
                return _masculineSingular + " milion";
            }
            else
            {
                return ThreeDigitSetConverter(number, GrammaticalGender.Feminine, true) + (IsAbove20(number) ? " " + _joinAbove20 : string.Empty) + " milioane";
            }
        }

        /// <summary>
        /// Converts a billions three-digit number to text.
        /// </summary>
        /// <param name="number">The three-digit number, as billions, to convert.</param>
        /// <param name="gender">The grammatical gender to convert to.</param>
        /// <returns>The same three-digit number of billions expressed as text.</returns>
        private string BillionsConverter(int number, GrammaticalGender gender)
        {
            if (number == 1)
            {
                return _masculineSingular + " miliard";
            }
            else
            {
                return ThreeDigitSetConverter(number, GrammaticalGender.Feminine) + (IsAbove20(number) ? " " + _joinAbove20 : string.Empty) + " miliarde";
            }
        }
    }
}
