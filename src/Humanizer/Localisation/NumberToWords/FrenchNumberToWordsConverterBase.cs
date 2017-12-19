﻿using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    abstract class FrenchNumberToWordsConverterBase : GenderedNumberToWordsConverter
    {
        static readonly string[] UnitsMap = { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"};
        static readonly string[] TensMap = { "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "septante", "octante", "nonante" };

        public override string Convert(long input, GrammaticalGender gender)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }
            var number = (int)input;

            if (number == 0)
                return UnitsMap[0];
            var parts = new List<string>();

            if (number < 0)
            {
                parts.Add("moins");
                number = -number;
            }

            CollectParts(parts, ref number, 1000000000, "milliard");
            CollectParts(parts, ref number, 1000000, "million");
            CollectThousands(parts, ref number, 1000, "mille");

            CollectPartsUnderAThousand(parts, number, gender, true);

            return string.Join(" ", parts);
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

        protected static string GetUnits(int number, GrammaticalGender gender)
        {
            if (number == 1 && gender == GrammaticalGender.Feminine)
            {
                return "une";
            }

            return UnitsMap[number];
        }

        static void CollectHundreds(ICollection<string> parts, ref int number, int d, string form, bool pluralize)
        {
            if (number < d) return;

            var result = number/d;
            if (result == 1)
            {
                parts.Add(form);
            }
            else
            {
                parts.Add(GetUnits(result, GrammaticalGender.Masculine));
                if (number%d == 0 && pluralize)
                {
                    parts.Add(form + "s");
                }
                else
                {
                    parts.Add(form);
                }
            }

            number %= d;
        }

        void CollectParts(ICollection<string> parts, ref int number, int d, string form)
        {
            if (number < d) return;

            var result = number/d;

            CollectPartsUnderAThousand(parts, result, GrammaticalGender.Masculine, true);

            if (result == 1)
            {
                parts.Add(form);
            }
            else
            {
                parts.Add(form + "s");
            }

            number %= d;
        }

        void CollectPartsUnderAThousand(ICollection<string> parts, int number, GrammaticalGender gender, bool pluralize)
        {
            CollectHundreds(parts, ref number, 100, "cent", pluralize);

            if (number > 0)
            {
                CollectPartsUnderAHundred(parts, ref number, gender, pluralize);
            }
        }

        void CollectThousands(ICollection<string> parts, ref int number, int d, string form)
        {
            if (number < d) return;

            var result = number/d;
            if (result > 1)
            {
                CollectPartsUnderAThousand(parts, result, GrammaticalGender.Masculine, false);
            }

            parts.Add(form);

            number %= d;
        }

        protected virtual void CollectPartsUnderAHundred(ICollection<string> parts, ref int number, GrammaticalGender gender, bool pluralize)
        {
            if (number < 20)
            {
                parts.Add(GetUnits(number, gender));
            }
            else
            {
                var units = number%10;
                var tens = GetTens(number/10);
                if (units == 0)
                {
                    parts.Add(tens);
                }
                else if (units == 1)
                {
                    parts.Add(tens);
                    parts.Add("et");
                    parts.Add(GetUnits(1, gender));
                }
                else
                {
                    parts.Add($"{tens}-{GetUnits(units, gender)}");
                }
            }
        }

        protected virtual string GetTens(int tens)
        {
            return TensMap[tens];
        }
    }
}