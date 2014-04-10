using System;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.NumberToWords
{
    internal class PolishNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private enum Numeral
        {
            One = 1,
            Thousand = 1000,
            Million = 1000000,//10^6
            Miliard = 1000000000,//10^9
        }

        private static readonly string Negative = "minus";
        private static readonly string Zero = "zero";

        private static string ConvertNumberUnderThousand(Numeral numeral, int number)
        {
            if (numeral != Numeral.One && number == 1)
                return string.Empty;

            var result = new StringBuilder();

            var hundreds = number / 100;
            if (hundreds > 0)
            {
                var map = new[] { "", "sto", "dwieście", "trzysta", "czterysta", "pięćset", "sześćset", "siedemset", "osiemset", "dziewięćset" };
                result.AppendFormat(@"{0} ", map[hundreds]);
                number = number % 100;
            }

            var tens = number / 10;
            if (tens > 1)
            {
                var map = new[] { "", "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "sześćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
                result.AppendFormat(@"{0} ", map[tens]);
                number = number % 10;
            }

            if (number > 0)
            {
                var map = new[] { "zero", "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć", "dziesięć", "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };
                result.AppendFormat(@"{0} ", map[number]);
            }

            return result.ToString();
        }
        private static int GetMappingIndex(int number)
        {
            if (number == 1)
                return 0;

            if (number > 1 && number < 5)
                return 1;//denominator

            var tens = number / 10;
            if (tens > 1)
            {
                var unity = number % 10;
                if (unity > 1 && unity < 5)
                    return 1;//denominator
            }

            return 2;//genitive
        }
        private static string GetSuffix(Numeral numeral, int num)
        {
            switch (numeral)
            {
                case Numeral.Miliard:
                    var miliard = new[] { "miliard", "miliardy", "miliardów" }; //one, denominator, genitive
                    return miliard[GetMappingIndex(num)];
                case Numeral.Million:
                    var million = new[] { "milion", "miliony", "milionów" }; //one, denominator, genitive
                    return million[GetMappingIndex(num)];
                case Numeral.Thousand:
                    var thousand = new[] { "tysiąc", "tysiące", "tysięcy" }; //one, denominator, genitive
                    return thousand[GetMappingIndex(num)];
                default:
                    return string.Empty;
            }
        }

        public override string Convert(int number)
        {
            if (number == 0)
                return Zero;

            var result = new StringBuilder();

            if (number < 0)
            {
                result.AppendFormat(@"{0} ", Negative);
                number = Math.Abs(number);
            }

            var numerals = ((Numeral[])Enum.GetValues(typeof(Numeral))).Reverse();
            foreach (var numeral in numerals)
            {
                var num = number / (int)numeral;
                if (num > 0)
                {
                    result.AppendFormat(@"{0}{1} ", ConvertNumberUnderThousand(numeral, num), GetSuffix(numeral, num));
                    number %= (int)numeral;
                }
            }

            return result.ToString().Trim();
        }

    }
}
