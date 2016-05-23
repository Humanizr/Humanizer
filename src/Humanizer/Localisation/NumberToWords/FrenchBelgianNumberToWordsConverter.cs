using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FrenchBelgianNumberToWordsConverter : FrenchAbstractNumberToWordsConverter
    {
        private static readonly string[] TensMap = new string[] { "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "septante", "quatre-vingt", "nonante" };
        private static readonly Dictionary<int, string> NumberExceptions = new Dictionary<int, string> { {80, "quatre-vingts"}, {81, "quatre-vingt-un"} };

        public FrenchBelgianNumberToWordsConverter() : base()
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
                string lastPart = TensMap[number / 10];

                if ((number % 10) > 0)
                {
                    if ((number - 1) % 10 == 0)
                        lastPart += " et un";
                    else
                        lastPart += string.Format("-{0}", UnitsMap[number % 10]);
                }

                parts.Add(lastPart);
            }
        }
    }
}