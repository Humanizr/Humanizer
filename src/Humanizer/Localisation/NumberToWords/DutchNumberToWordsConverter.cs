using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    /// <summary>
    /// Dutch spelling of numbers is not really officially regulated.
    /// There are a few different rules that can be applied.
    /// Used the rules as stated here.
    /// http://www.beterspellen.nl/website/?pag=110
    /// </summary>
    internal class DutchNumberToWordsConverter : INumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "nul", "een", "twee", "drie", "vier", "vijf", "zes", "zeven", "acht", "negen", "tien", "elf", "twaalf", "dertien", "veertien", "vijftien", "zestien", "zeventien", "achttien", "negentien" };
        private static readonly string[] TensMap = { "nul", "tien", "twintig", "dertig", "veertig", "vijftig", "zestig", "zeventig", "tachtig", "negentig" };

        class Fact
        {
            public int Value { get; set; }
            public string Name { get; set; }
            public string Prefix { get; set; }
            public string Postfix { get; set; }
            public bool DisplayOneUnit { get; set; }
        }

        private static readonly Fact[] Hunderds =
	    {
		    new Fact {Value = 1000000000, Name = "miljard", Prefix = " ", Postfix = " ", DisplayOneUnit = true},
		    new Fact {Value = 1000000,    Name = "miljoen", Prefix = " ", Postfix = " ", DisplayOneUnit = true},
		    new Fact {Value = 1000,       Name = "duizend", Prefix = "",  Postfix = " ", DisplayOneUnit = false},
		    new Fact {Value = 100,        Name = "honderd", Prefix = "",  Postfix = "",  DisplayOneUnit = false},
	    };

        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {1, "eerste"},
            {3, "derde"},
            {8, "achtste"},
        };

        public string Convert(int number)
        {
            if (number == 0)
                return UnitsMap[0];

            if (number < 0)
                return string.Format("min {0}", Convert(-number));

            var word = "";

            foreach (var m in Hunderds)
            {
                var divided = number / m.Value;

                if (divided <= 0) continue;

                if (divided == 1 && !m.DisplayOneUnit)
                {
                    word += m.Name;
                }
                else
                {
                    word += Convert(divided) + m.Prefix + m.Name;
                }

                number %= m.Value;
                if (number > 0)
                {
                    word += m.Postfix;
                }
            }

            if (number > 0)
            {
                if (number < 20)
                {
                    word += UnitsMap[number];
                }
                else
                {
                    var tens = TensMap[number / 10];
                    var unit = number % 10;
                    if (unit > 0)
                    {
                        var units = UnitsMap[unit];
                        var trema = units.EndsWith("e");
                        word += units + (trema ? "ën" : "en") + tens;
                    }
                    else
                    {
                        word += tens;
                    }
                }
            }

            return word;
        }

        public string ConvertToOrdinal(int number)
        {
            var exceptionalTens = number % 100;

            // 112 => 12 => honderdtwaalfde
            if (exceptionalTens < 20 && number > 20 && exceptionalTens > 0)
            {
                // replace normal word with ordinal version
                return ReplaceNormalWordWithOrdinalWord(number, exceptionalTens);
            }

            var exceptionalUnit = number % 10;

            string exceptionalWord;
            if (exceptionalTens < 10 && ExceptionNumbersToWords(exceptionalUnit, out exceptionalWord))
            {
                // replace normal word with ordinal version
                return ReplaceNormalWordWithOrdinalWord(number, exceptionalUnit, exceptionalWord);
            }

            return number < 20 ? Convert(number) + "de" : Convert(number) + "ste";
        }

        private string ReplaceNormalWordWithOrdinalWord(int number, int exceptionalNumber)
        {
            return ReplaceNormalWordWithOrdinalWord(number, exceptionalNumber, Convert(exceptionalNumber));
        }

        private string ReplaceNormalWordWithOrdinalWord(int number, int exceptionalNumber, string ordinalWord)
        {
            var word = Convert(number);
            var normalWord = Convert(exceptionalNumber);
            var pos = word.LastIndexOf(normalWord, StringComparison.CurrentCulture);
            return word.Substring(0, pos) + ordinalWord;
        }

        private static bool ExceptionNumbersToWords(int number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}