using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.NumberToWords
{
    internal abstract class FrenchAbstractNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        protected static readonly string[] UnitsMap = new string[] { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };

        protected FrenchAbstractNumberToWordsConverter()
        {

        }

        public override string Convert(int number, GrammaticalGender gender)
        {
            if (number == 0)
                return UnitsMap[0];

            if (number < 0)
                return string.Format("moins {0}", Convert(Math.Abs(number)));

            var parts = new List<string>();

            if ((number/1000000000) > 0)
            {
                parts.Add(string.Format("{0} milliard{1}",
                    Convert(number/1000000000),
                    number/1000000000 == 1 ? string.Empty : "s"));

                number %= 1000000000;
            }

            if ((number/1000000) > 0)
            {
                parts.Add(string.Format("{0} million{1}",
                   Convert(number/1000000),
                   number/1000000 == 1 ? string.Empty : "s"));

                number %= 1000000;
            }

            if ((number/1000) > 0)
            {
                parts.Add(string.Format("{0}mille",
                number/1000 != 1 ? (" " + Convert(number/1000)) : string.Empty));

                number %= 1000;
            }

            if ((number/100) > 0)
            {
                if (number < 200)
                    parts.Add("cent");
                else
                    parts.Add(string.Format("{0} cent{1}",
                    Convert(number/100),
                    number%100 == 0 ? "s" : string.Empty));

                number %= 100;
            }

            if (number > 0)
            {
                LastPart(ref parts, ref number);

                // only 'un' agrees with gender, and its feminine form is 'une'
                if (gender == GrammaticalGender.Feminine && parts.Last().EndsWith("un"))
                    parts[parts.Count - 1] += "e";
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

            if (number == 0)
                convertedNumber += "t";

            convertedNumber = convertedNumber.TrimEnd('e');
            convertedNumber += "ième";
            return convertedNumber;
        }

        protected abstract void LastPart(ref List<string> parts, ref int number);
    }
}
