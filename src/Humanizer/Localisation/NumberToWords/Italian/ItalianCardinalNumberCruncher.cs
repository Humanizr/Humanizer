using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords.Italian
{
    class ItalianCardinalNumberCruncher
    {
        public ItalianCardinalNumberCruncher(int number, GrammaticalGender gender)
        {
            _fullNumber = number;
            _threeDigitParts = SplitEveryThreeDigits(number);
            _gender = gender;
            _nextSet = ThreeDigitSets.Units;
        }
        
        public string Convert()
        {
            // it's easier to treat zero as a completely distinct case
            if (_fullNumber == 0)
                return "zero";

            string words = String.Empty;

            foreach (int part in _threeDigitParts)
            {
                Func<int, string> partToString = GetNextPartConverter();
                
                words = partToString(part) + words;
            }
            
            // remove trailing spaces if there are only millions or billions
            return words.TrimEnd();
        }
        
        protected readonly int _fullNumber;
        protected readonly List<int> _threeDigitParts;
        protected readonly GrammaticalGender _gender;
        
        protected ThreeDigitSets _nextSet;
        
        /// <summary>
        /// Splits a number into a sequence of three-digits numbers, starting 
        /// from units, then thousands, millions, and so on.
        /// </summary>
        /// <param name="number">The number to split.</param>
        /// <returns>The sequence of three-digit numbers.</returns>
        protected static List<int> SplitEveryThreeDigits(int number)
        {
            List<int> parts = new List<int>();
            int rest = number;
            
            while (rest > 0)
            {
                int threeDigit = rest % 1000;
                
                parts.Add(threeDigit);
                
                rest = (int)(rest / 1000);
            }
            
            return parts;
        }
        
        /// <summary>
        /// During number conversion to text, finds out the converter to use
        /// for the next three-digit set.
        /// </summary>
        /// <returns>The next conversion function to use.</returns>
        public Func<int, string> GetNextPartConverter()
        {
            Func<int, string> converter;
            
            switch (_nextSet)
            {
                case ThreeDigitSets.Units:
                    converter = UnitsConverter;
                    _nextSet = ThreeDigitSets.Thousands;
                    break;
                    
                case ThreeDigitSets.Thousands:
                    converter = ThousandsConverter;
                    _nextSet = ThreeDigitSets.Millions;
                    break;
                    
                case ThreeDigitSets.Millions:
                    converter = MillionsConverter;
                    _nextSet = ThreeDigitSets.Billions;
                    break;
                    
                case ThreeDigitSets.Billions:
                    converter = BillionsConverter;
                    _nextSet = ThreeDigitSets.More;
                    break;
                    
                case ThreeDigitSets.More:
                    converter = null;
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException("Unknow ThreeDigitSet: " + _nextSet);
            }
            
            return converter;
        }
        
        /// <summary>
        /// Converts a three-digit set to text.
        /// </summary>
        /// <param name="number">The three-digit set to convert.</param>
        /// <param name="thisIsLastSet">True if the current three-digit set is the last in the word.</param>
        /// <returns>The same three-digit set expressed as text.</returns>
        protected static string ThreeDigitSetConverter(int number, bool thisIsLastSet = false)
        {
            if (number == 0) 
                return String.Empty;
              
            // grab lowest two digits
            int tensAndUnits = number % 100;
            // grab third digit
            int hundreds = (int)(number / 100);
            
            // grab also first and second digits separately
            int units = tensAndUnits % 10;
            int tens = (int)(tensAndUnits / 10);
            
            string words = String.Empty;
            
            // append text for hundreds
            words += _hundredNumberToText[hundreds];
            
            // append text for tens, only those from twenty upward
            words += _tensOver20NumberToText[tens];
            
            if (tensAndUnits <= 9)
            {
                // simple case for units, under 10
                words += _unitsNumberToText[tensAndUnits];
            }
            else if (tensAndUnits <= 19)
            {
                // special case for 'teens', from 10 to 19
                words += _teensUnder20NumberToText[tensAndUnits - 10];
            }
            else
            {
                // just append units text, with some corner cases
                
                // truncate tens last vowel before 'uno' (1) and 'otto' (8)                    
                if (units == 1 || units == 8)
                {
                    words = words.Remove(words.Length - 1);
                }
                
                // if this is the last set, an accent could be due
                string unitsText = (thisIsLastSet && units == 3 ? "tré" : _unitsNumberToText[units]);
                
                words += unitsText;
            }
            
            return words;
        }
        
        /// <summary>
        /// Converts a three-digit number, as units, to text.
        /// </summary>
        /// <param name="number">The three-digit number, as units, to convert.</param>
        /// <returns>The same three-digit number, as units, expressed as text.</returns>
        protected string UnitsConverter(int number)
        {
            // being a unique case, it's easier to treat unity feminine gender as a completely distinct case
            if (_gender == GrammaticalGender.Feminine && _fullNumber == 1)
                return "una";
                
            return ThreeDigitSetConverter(number, true);
        }
        
        /// <summary>
        /// Converts a thousands three-digit number to text.
        /// </summary>
        /// <param name="number">The three-digit number, as thousands, to convert.</param>
        /// <returns>The same three-digit number of thousands expressed as text.</returns>
        protected static string ThousandsConverter(int number)
        {
            if (number == 0) 
                return String.Empty;
              
            if (number == 1)
                return "mille";
            
            return ThreeDigitSetConverter(number) + "mila";
        }
        
        /// <summary>
        /// Converts a millions three-digit number to text.
        /// </summary>
        /// <param name="number">The three-digit number, as millions, to convert.</param>
        /// <returns>The same three-digit number of millions expressed as text.</returns>
        protected static string MillionsConverter(int number)
        {
            if (number == 0) 
                return String.Empty;
              
            if (number == 1)
                return "un milione ";
              
            return ThreeDigitSetConverter(number, true) + " milioni ";
        }
        
        /// <summary>
        /// Converts a billions three-digit number to text.
        /// </summary>
        /// <param name="number">The three-digit number, as billions, to convert.</param>
        /// <returns>The same three-digit number of billions expressed as text.</returns>
        protected static string BillionsConverter(int number)
        {
            if (number == 1)
                return "un miliardo ";
              
            return ThreeDigitSetConverter(number) + " miliardi ";
        }
        
        /// <summary>
        /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9.
        /// </summary>
        protected static string[] _unitsNumberToText = new string[]
        {
            String.Empty,
            "uno",
            "due",
            "tre",
            "quattro",
            "cinque",
            "sei",
            "sette",
            "otto",
            "nove"
        };
        
        /// <summary>
        /// Lookup table converting tens number to text. Index 2 for 20, index 3 for 30, up to index 9 for 90.
        /// </summary>
        protected static string[] _tensOver20NumberToText = new string[]
        {
            String.Empty,
            String.Empty,
            "venti",
            "trenta",
            "quaranta",
            "cinquanta",
            "sessanta",
            "settanta",
            "ottanta",
            "novanta"
        };
        
        /// <summary>
        /// Lookup table converting teens number to text. Index 0 for 10, index 1 for 11, up to index 9 for 19.
        /// </summary>
        protected static string[] _teensUnder20NumberToText = new string[]
        {
            "dieci",
            "undici",
            "dodici",
            "tredici",
            "quattordici",
            "quindici",
            "sedici",
            "diciassette",
            "diciotto",
            "diciannove"
        };
        
        /// <summary>
        /// Lookup table converting hundreds number to text. Index 0 for no hundreds, index 1 for 100, up to index 9.
        /// </summary>
        protected static string[] _hundredNumberToText = new string[]
        {
            String.Empty,
            "cento",
            "duecento",
            "trecento",
            "quattrocento",
            "cinquecento",
            "seicento",
            "settecento",
            "ottocento",
            "novecento"
        };
        
        /// <summary>
        /// Enumerates sets of three-digits having distinct conversion to text.
        /// </summary>
        protected enum ThreeDigitSets
        {
            /// <summary>
            /// Lowest three-digits set, from 1 to 999.
            /// </summary>
            Units,
            
            /// <summary>
            /// Three-digits set counting the thousands, from 1'000 to 999'000.
            /// </summary>
            Thousands,
            
            /// <summary>
            /// Three-digits set counting millions, from 1'000'000 to 999'000'000.
            /// </summary>
            Millions,
            
            /// <summary>
            /// Three-digits set counting billions, from 1'000'000'000 to 999'000'000'000.
            /// </summary>
            Billions,
            
            /// <summary>
            /// Three-digits set beyond 999 billions, from 1'000'000'000'000 onward.
            /// </summary>
            More
        }
    }
}

