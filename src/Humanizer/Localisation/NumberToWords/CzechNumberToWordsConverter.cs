using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Humanizer.Localisation.NumberToWords
{
    internal class CzechNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] BillionsMap = { "miliarda", "miliardy", "miliard" };
        private static readonly string[] MillionsMap = { "milion", "miliony", "milionů" };
        private static readonly string[] ThousandsMap = { "tisíc", "tisíce", "tisíc" };
        private static readonly string[] HundredsMap = { "nula", "sto", "dvě stě", "tři sta", "čtyři sta", "pět set", "šest set", "sedm set", "osm set", "devět set" };
        private static readonly string[] TensMap = { "nula", "deset", "dvacet", "třicet", "čtyřicet", "padesát", "šedesát", "sedmdesát", "osmdesát", "devadesát" };
        private static readonly string[] UnitsMap = { "nula", "jeden", "dva", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět", "deset", "jedenáct", "dvanáct", "třináct", "čtrnáct", "patnáct", "šestnáct", "sedmnáct", "osmnáct", "devatenáct" };

        private static readonly string[] UnitsMasculineOverrideMap = { "jeden", "dva" };
        private static readonly string[] UnitsFeminineOverrideMap = { "jedna", "dvě" };
        private static readonly string[] UnitsNeuterOverride = { "jedno", "dvě" };
        private static readonly string[] UnitsIntraOverride = { "jedna", "dva" };

        private readonly CultureInfo _culture;

        public CzechNumberToWordsConverter(CultureInfo culture)
        {
            _culture = culture;
        }

        public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        {
            if (number == 0)
            {
                return UnitByGender(number, gender);
            }

            var parts = new List<string>();
            if (number < 0)
            {
                parts.Add("mínus");
                number = -number;
            }

            CollectThousandAndAbove(parts, ref number, 1_000_000_000, GrammaticalGender.Feminine, BillionsMap);
            CollectThousandAndAbove(parts, ref number, 1_000_000, GrammaticalGender.Masculine, MillionsMap);
            CollectThousandAndAbove(parts, ref number, 1_000, GrammaticalGender.Masculine, ThousandsMap);

            CollectLessThanThousand(parts, number, gender);

            return string.Join(" ", parts);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return number.ToString(_culture);
        }

        private string UnitByGender(long number, GrammaticalGender? gender)
        {
            if (number != 1 && number != 2)
            {
                return UnitsMap[number];
            }

            return gender switch
            {
                GrammaticalGender.Masculine => UnitsMasculineOverrideMap[number - 1],
                GrammaticalGender.Feminine => UnitsFeminineOverrideMap[number - 1],
                GrammaticalGender.Neuter => UnitsNeuterOverride[number - 1],
                null => UnitsIntraOverride[number - 1],
                _ => throw new ArgumentOutOfRangeException(nameof(gender)),
            };
        }

        private void CollectLessThanThousand(List<string> parts, long number, GrammaticalGender? gender)
        {
            if (number >= 100)
            {
                parts.Add(HundredsMap[number / 100]);
                number %= 100;
            }

            if (number >= 20)
            {
                parts.Add(TensMap[number / 10]);
                number %= 10;
            }

            if (number > 0)
            {
                parts.Add(UnitByGender(number, gender));
            }
        }

        private void CollectThousandAndAbove(List<string> parts, ref long number, long divisor, GrammaticalGender gender, string[] map)
        {
            var n = number / divisor;

            if (n <= 0)
            {
                return;
            }

            CollectLessThanThousand(parts, n, n < 19 ? gender : (GrammaticalGender?)null);

            var units = n % 1000;

            if (units == 1)
            {
                parts.Add(map[0]);
            }
            else if (units > 1 && units < 5)
            {
                parts.Add(map[1]);
            }
            else
            {
                parts.Add(map[2]);
            }

            number %= divisor;
        }
    }
}
