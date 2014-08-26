using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class ItalianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        public override string Convert(int number, GrammaticalGender gender)
        {
            if (number == 0) 
                return "zero";

            if (number < 0)
                return "meno " + Convert(Math.Abs(number), gender);
        
            ItalianNumberCruncher cruncher = new ItalianNumberCruncher(number);
            
            return cruncher.Convert();
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return String.Empty;
        }
        
        class ItalianNumberCruncher
        {
            public ItalianNumberCruncher(int number)
            {
                _number = number;
                _threeDigitChops = ChopEveryThreeDigits(_number);
                _nextSet = ThreeDigitSets.Units;
            }
            
            public string Convert()
            {
                string word = String.Empty;
                
                foreach(int chop in _threeDigitChops)
                {
                    Func<int, string> chopToString = GetNextChopConverter();
                    
                    word = chopToString(chop) + word;
                }
                
                return word;
            }
            
            protected readonly int _number;
            protected readonly List<int> _threeDigitChops;
            
            protected ThreeDigitSets _nextSet;
            
            protected static List<int> ChopEveryThreeDigits(int number)
            {
                List<int> chops = new List<int>();
                int rest = number;
                
                while (rest > 0)
                {
                    int threeDigit = rest % 1000;
                    
                    chops.Add(threeDigit);
                    
                    rest = (int)(rest / 1000);
                }
                
                return chops;
            }
            
            public Func<int, string> GetNextChopConverter()
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
                        converter = null;
                        break;
                        
                    case ThreeDigitSets.Billions:
                        converter = null;
                        break;
                        
                    case ThreeDigitSets.More:
                        converter = null;
                        break;
                        
                    default:
                        throw new ArgumentOutOfRangeException("Unknow ThreeDigitSet: " + _nextSet);
                }
                
                return converter;
            }
            
            protected static string UnitsConverter(int number)
            {
                int tensAndUnits = number % 100;
                int hundreds = (int)(number / 100);
                
                int units = tensAndUnits % 10;
                int tens = (int)(tensAndUnits / 10);
                
                string words = String.Empty;
                
                words += _numberToHundred[hundreds];
                
                words += _numberToTen[tens];
                
                if (tensAndUnits <= 9)
                {
                    words += _numberToUnit[tensAndUnits];
                }
                else if (tensAndUnits <= 19)
                {
                    words += _numberToTeens[tensAndUnits - 10];
                }
                else
                {
                    if (units == 1 || units == 8)
                    {
                        words = words.Remove(words.Length - 1);
                    }
                    
                    words += _numberToUnit[units];
                }
                
                return words;
            }
            
            protected static string ThousandsConverter(int number)
            {
                return UnitsConverter(number) + "mila";
            }
            
            protected static string[] _numberToUnit = new string[]
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
            
            protected static string[] _numberToTen = new string[]
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
            
            protected static string[] _numberToTeens = new string[]
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
            
            protected static string[] _numberToHundred = new string[]
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
            
            protected enum ThreeDigitSets
            {
                Units,
                Thousands,
                Millions,
                Billions,
                More
            }
        }
    }
}