using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class FrenchNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
        private static readonly string[] TensMap = { "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix"};

        private static readonly Dictionary<int,string> NumberExceptions = new Dictionary<int, string>
        {
            {71, "soixante et onze"},
            {81, "quatre-vingt-un"},
            {91, "quatre-vingt-onze"}
        };

        public override string Convert(int number, GrammaticalGender gender)
        {
            if (number == 0)
                return UnitsMap[0];

            if (number < 0)
                return string.Format("moins {0}", Convert(Math.Abs(number)));

            var parts = new List<string>();
           
            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} milliard{1}",
                    Convert(number/1000000000),
                    number/1000000000 == 1 ? "" : "s"));

                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                 parts.Add(string.Format("{0} million{1}",
                    Convert(number/1000000),
                    number/1000000 == 1 ? "" : "s"));

                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(number/1000 == 1
                    ? string.Format("mille")
                    : string.Format("{0} mille", Convert(number/1000)));

                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(number < 200 ? "cent" : string.Format("{0} cent", Convert(number/100)));
                number %= 100;
            }

            if (number > 0)
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
                        int baseNumber = number < 80 ? 60 : 80;
                        lastPart = string.Format("{0}-{1}", TensMap[baseNumber/10], Convert(number - baseNumber));
                    }
                    else
                    {
                        lastPart = TensMap[number/10];
                        if ((number%10) > 0)
                        {
                            if ((number - 1) % 10 == 0)
                                lastPart += " et un";
                            else
                                lastPart += string.Format("-{0}", UnitsMap[number%10]);
                        }
                    }
                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            if (number == 1)
                return gender == GrammaticalGender.Feminine ? "première" : "premier";

            var convertedNumber = Convert(number);

            if (convertedNumber.EndsWith("s") && !convertedNumber.EndsWith("trois"))
                convertedNumber = convertedNumber.TrimEnd('s');
            else if (convertedNumber.EndsWith("cinq"))
                convertedNumber += "u";
            else if (convertedNumber.EndsWith("neuf"))
                convertedNumber = convertedNumber.TrimEnd('f') + "v";

            if (convertedNumber.StartsWith("un "))
                convertedNumber = convertedNumber.Remove(0, 3);

            convertedNumber = convertedNumber.TrimEnd('e');
            convertedNumber += "ième";
            return convertedNumber;
        }
    }
}
