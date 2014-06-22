using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    /// <summary>
    /// Dutch spelling of numbers is not really officially regulated.
    /// There are a few different rules that can be applied.
    /// Used the rules as stated here.
    /// http://www.beterspellen.nl/website/?pag=110
    /// </summary>
    internal class DutchNumberToWordsConverter : GenderlessNumberToWordsConverter
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
            new Fact {Value = 100,        Name = "honderd", Prefix = "",  Postfix = "",  DisplayOneUnit = false}
        };

        public override string Convert(int number)
        {
            if (number == 0)
                return UnitsMap[0];

            if (number < 0)
                return string.Format("min {0}", Convert(-number));

            var word = "";

            foreach (var m in Hunderds)
            {
                var divided = number / m.Value;

                if (divided <= 0) 
                    continue;

                if (divided == 1 && !m.DisplayOneUnit)
                    word += m.Name;
                else
                    word += Convert(divided) + m.Prefix + m.Name;

                number %= m.Value;
                if (number > 0)
                    word += m.Postfix;
            }

            if (number > 0)
            {
                if (number < 20)
                    word += UnitsMap[number];
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
                        word += tens;
                }
            }

            return word;
        }

        private static readonly Dictionary<string, string> OrdinalExceptions = new Dictionary<string, string>
        {
            {"een", "eerste"},
            {"drie", "derde"},
            {"miljoen", "miljoenste"},
        };

        private static readonly char[] EndingCharForSte = {'t', 'g', 'd'};

        public override string ConvertToOrdinal(int number)
        {
            var word = Convert(number);

            foreach (var kv in OrdinalExceptions.Where(kv => word.EndsWith(kv.Key)))
            {
                // replace word with exception
                return word.Substring(0, word.Length - kv.Key.Length) + kv.Value;
            }

            // achtste
            // twintigste, dertigste, veertigste, ...
            // honderdste, duizendste, ...
            if (word.LastIndexOfAny(EndingCharForSte) == (word.Length - 1))
                return word + "ste";

            return word + "de";
        }
    }
}