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
        }
        
        public string Convert()
        {
            string words = String.Empty;
                
            if (_fullNumber < 1000)
            {
                words = UnitsConverter(_fullNumber);
            }
            
            return words;
        }
        
        protected string UnitsConverter(int number)
        {
            return ThreeDigitSetConverter(number);
        }

        protected static string ThreeDigitSetConverter(int number)
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
                
                words += _unitsNumberToText[units];
            }
            
            return words;
        }
        
        protected readonly int _fullNumber;
        protected readonly GrammaticalGender _gender;
        
        /// <summary>
        /// Lookup table converting units number to text. Index 1 for 1, index 2 for 2, up to index 9.
        /// </summary>
        protected static string[] _unitsNumberToText = new string[]
        {
            String.Empty,
            "primo",
            "secondo",
            "terzo",
            "quarto",
            "quinto",
            "sesto",
            "settimo",
            "ottavo",
            "nono"
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
            "decimo",
            "undicesimo",
            "dodicesimo",
            "tredicesimo",
            "quattordicesimo",
            "quindicesimo",
            "sedicesimo",
            "diciassettesimo",
            "diciottesimo",
            "diciannovesimo"
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
    }
}