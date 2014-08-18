using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords.Italian
{
    class ItalianOrdinalNumberCruncher
    {
        public ItalianOrdinalNumberCruncher(int number, GrammaticalGender gender)
        {
            _fullNumber = number;
            _gender = gender;
            _genderSuffix = (gender == GrammaticalGender.Feminine ? "a" : "o");
        }
        
        public string Convert()
        {
            // it's easier to treat zero as a completely distinct case
            if (_fullNumber == 0)
                return "zero";

            if (_fullNumber <= 9)
            {
                // units ordinals, 1 to 9, are totally different than the rest: treat them as a distinct case
                return _unitsUnder10NumberToText[_fullNumber] + _genderSuffix;
            }

            ItalianCardinalNumberCruncher cardinalCruncher = new ItalianCardinalNumberCruncher(_fullNumber, _gender);

            string words = cardinalCruncher.Convert();

            int tensAndUnits = _fullNumber % 100;

            if (tensAndUnits == 10)
            {
                // for numbers ending in 10, cardinal and ordinal endings are different, suffix doesn't work
                words = words.Remove(words.Length - _lengthOf10AsCardinal) + "decim" + _genderSuffix;
            }
            else
            {
                // truncate last vowel
                words = words.Remove(words.Length - 1);

                int units = _fullNumber % 10;

                // reintroduce *unaccented* last vowel in some corner cases
                if (units == 3)
                    words += 'e';
                else if (units == 6)
                    words += 'i';

                int lowestThreeDigits = _fullNumber % 1000;
                int lowestSixDigits = _fullNumber % 1000000;
                int lowestNineDigits = _fullNumber % 1000000000;

                if (lowestNineDigits == 0)
                {
                    // if exact billions, cardinal number words are joined
                    words = words.Replace(" miliard", "miliard");

                    // if 1 billion, numeral prefix is removed completely
                    if (_fullNumber == 1000000000)
                    {
                        words = words.Replace("un", String.Empty);
                    }
                }
                else if (lowestSixDigits == 0)
                {
                    // if exact millions, cardinal number words are joined
                    words = words.Replace(" milion", "milion");

                    // if 1 million, numeral prefix is removed completely
                    if (_fullNumber == 1000000)
                    {
                        words = words.Replace("un", String.Empty);
                    }
                }
                else if (lowestThreeDigits == 0 && _fullNumber > 1000)
                {
                    // if exact thousands, double the final 'l', apart from 1000 already having that
                    words += 'l';
                }

                // append common ordinal suffix
                words += "esim" + _genderSuffix;
            }

            return words;
        }

        protected readonly int _fullNumber;
        protected readonly GrammaticalGender _gender;
        private readonly string _genderSuffix;

        /// <summary>
        /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9.
        /// </summary>
        protected static string[] _unitsUnder10NumberToText = new string[]
        {
            String.Empty,
            "prim",
            "second",
            "terz",
            "quart",
            "quint",
            "sest",
            "settim",
            "ottav",
            "non"
        };

        protected static int _lengthOf10AsCardinal = "dieci".Length;
    }
}