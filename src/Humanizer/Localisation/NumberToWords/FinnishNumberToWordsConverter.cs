using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FinnishNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "nolla", "yksi", "kaksi", "kolme", "neljä", "viisi", "kuusi", "seitsemän", "kahdeksan", "yhdeksän", "kymmenen" };
        private static readonly string[] OrdinalUnitsMap = { "nollas", "ensimmäinen", "toinen", "kolmas", "neljäs", "viides", "kuudes", "seitsemäs", "kahdeksas", "yhdeksäs", "kymmenes" };

        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {1, "yhdes" },
            {2, "kahdes" }
        };

        public override string Convert(long input)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var number = (int)input;

            if (number < 0)
            {
                return string.Format("miinus {0}", Convert(-number));
            }

            if (number == 0)
            {
                return UnitsMap[0];
            }

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(number / 1000000000 == 1
                    ? "miljardi "
                    : string.Format("{0}miljardia ", Convert(number / 1000000000)));

                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(number / 1000000 == 1
                    ? "miljoona "
                    : string.Format("{0}miljoonaa ", Convert(number / 1000000)));

                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(number / 1000 == 1
                    ? "tuhat "
                    : string.Format("{0}tuhatta ", Convert(number / 1000)));

                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(number / 100 == 1
                    ? "sata"
                    : string.Format("{0}sataa", Convert(number / 100)));

                number %= 100;
            }

            if (number >= 20 && (number / 10) > 0)
            {
                parts.Add(string.Format("{0}kymmentä", Convert(number / 10)));
                number %= 10;
            }
            else if (number > 10 && number < 20)
            {
                parts.Add(string.Format("{0}toista", UnitsMap[number % 10]));
            }

            if (number > 0 && number <= 10)
            {
                parts.Add(UnitsMap[number]);
            }

            return string.Join("", parts).Trim();
        }

        private string GetOrdinalUnit(int number, bool useExceptions)
        {
            if (useExceptions && OrdinalExceptions.ContainsKey(number))
            {
                return OrdinalExceptions[number];
            }

            return OrdinalUnitsMap[number];
        }

        private string ToOrdinal(int number, bool useExceptions)
        {
            if (number == 0)
            {
                return OrdinalUnitsMap[0];
            }

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0}miljardis", (number / 1000000000) == 1 ? "" : ToOrdinal(number / 1000000000, true)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0}miljoonas", (number / 1000000) == 1 ? "" : ToOrdinal(number / 1000000, true)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0}tuhannes", (number / 1000) == 1 ? "" : ToOrdinal(number / 1000, true)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0}sadas", (number / 100) == 1 ? "" : ToOrdinal(number / 100, true)));
                number %= 100;
            }

            if (number >= 20 && (number / 10) > 0)
            {
                parts.Add(string.Format("{0}kymmenes", ToOrdinal(number / 10, true)));
                number %= 10;
            }
            else if (number > 10 && number < 20)
            {
                parts.Add(string.Format("{0}toista", GetOrdinalUnit(number % 10, true)));
            }

            if (number > 0 && number <= 10)
            {
                parts.Add(GetOrdinalUnit(number, useExceptions));
            }

            return string.Join("", parts);
        }

        public override string ConvertToOrdinal(int number)
        {
            return ToOrdinal(number, false);
        }
    }
}
