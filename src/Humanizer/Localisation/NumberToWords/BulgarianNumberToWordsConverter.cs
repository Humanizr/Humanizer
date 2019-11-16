using System;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Localisation.NumberToWords
{
    internal class BolgarianNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "нула", "един", "две", "три", "четири", "пет", "шест", "седем", "осем", "девет", "десет", "единадесет", "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет", "осемнадесет", "деветнадесет" };
        private static readonly string[] TensMap = { "нула", "десет", "двадесет", "тридесет", "четиридесет", "петдесет", "шейсет", "седемдесет", "осемдесет", "деветдесет" };
        private static readonly string[] HundredsMap = { "нула", "сто", "двеста", "триста", "че́тиристотин", "петстотин", "шестстотин", "седемстотин", "осемстотин", "деветстотин" };

        private static readonly string[] TensOrdinal = { string.Empty, "десет", "двадесет", "тридесет", "четиридесет", "петдесет", "шейсет", "седемдесет", "осемдесет", "деветдесет" };
        private static readonly string[] UnitsOrdinal = { string.Empty, "първ", "втор", "трет", "четвърт", "пет", "шест", "седм", "осм", "девят", "десят", "единадесет", "дванадесет", "тринадесет", "четиринадесет", "петнадесет", "шестнадесет", "седемнадесет", "осемнадесет", "деветнадесет" };

        public override string Convert(long input, GrammaticalGender gender)
        {
            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                throw new NotImplementedException();
            }

            if (input == 0)
            {
                return "нула";
            }

            var parts = new List<string>();

            if (input < 0)
            {
                parts.Add("минус");
                input = -input;
            }

            var hundreds = input / 100;

            if (hundreds > 0)
            {
                parts.Add(HundredsMap[input / 100]);
                input %= 100;
            }

            if (input > 0)
            {
                if (input < 20)
                {
                    parts.Add(UnitsMap[input]);
                }
                else
                {

                    parts.Add(TensMap[input / 10]);

                    var units = input % 10;

                    if (units > 0)
                    {
                        parts.Add(string.Format("и {0}", UnitsMap[units]));
                    }
                }
            }

            return string.Join(" ", parts);
        }

        public override string ConvertToOrdinal(int input, GrammaticalGender gender)
        {
            if (input == 0)
            {
                return "нула";
            }

            var parts = new List<string>();

            if (input < 0)
            {
                parts.Add("минус");
                input = -input;
            }

            if (input >= 20)
            {
                var ending = GetEndingForGender(gender, input);
                var tens = input / 10;
                input %= 10;
                if (input == 0)
                {
                    parts.Add(TensOrdinal[tens] + ending);
                }
                else
                {
                    parts.Add(string.Format("{0} и", TensMap[tens]));
                }
            }

            if (input > 0)
            {
                parts.Add(UnitsOrdinal[input] + GetEndingForGender(gender, input));
            }

            return string.Join(" ", parts);
        }

        private static string GetEndingForGender(GrammaticalGender gender, long number)
        {
            switch (gender)
            {
                case GrammaticalGender.Masculine:
                    return "и";
                case GrammaticalGender.Feminine:
                    return "а";
                case GrammaticalGender.Neuter:
                    return "о";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender));
            }
        }

    }
}
