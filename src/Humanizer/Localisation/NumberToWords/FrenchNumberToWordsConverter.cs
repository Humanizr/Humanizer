using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FrenchNumberToWordsConverter : FrenchAbstractNumberToWordsConverter
    {
        private static readonly string[] TensMap = new string[] { "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix" };
        private static readonly Dictionary<int, string> NumberExceptions = new Dictionary<int, string> { {71, "soixante et onze"}, {80, "quatre-vingts"}, {81, "quatre-vingt-un"}, {91, "quatre-vingt-onze"} };

        public FrenchNumberToWordsConverter()
        {

        }

        protected override void LastPart(ref List<string> parts, ref int number)
        {
            if (NumberExceptions.ContainsKey(number))
                parts.Add(NumberExceptions[number]);
            else if (number < 20)
                parts.Add(UnitsMap[number]);
            else
            {
                string lastPart;

                if (number >= 70 && (number < 80 || number >= 90))
                {
                    var baseNumber = number < 80 ? 60 : 80;
                    lastPart = string.Format("{0}-{1}", TensMap[baseNumber / 10], Convert(number - baseNumber));
                }
                else
                {
                    lastPart = TensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        if ((number - 1) % 10 == 0)
                            lastPart += " et un";
                        else
                            lastPart += string.Format("-{0}", UnitsMap[number % 10]);
                    }
                }

                parts.Add(lastPart);
            }
        }
    }
}