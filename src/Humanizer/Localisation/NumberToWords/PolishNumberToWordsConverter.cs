using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Localisation.NumberToWords
{
    internal class PolishNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] HundredsMap = { "zero", "sto", "dwieście", "trzysta", "czterysta", "pięćset", "sześćset", "siedemset", "osiemset", "dziewięćset" };
        private static readonly string[] TensMap = { "zero", "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "sześćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
        private static readonly string[] UnitsMap = { "zero", "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć", "dziesięć", "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };

        private readonly CultureInfo _culture;

        public PolishNumberToWordsConverter(CultureInfo culture)
        {
            _culture = culture;
        }

        private static void CollectPartsUnderThousand(ICollection<string> parts, int number)
        {
            var hundreds = number/100;
            if (hundreds > 0)
            {
                parts.Add(HundredsMap[hundreds]);
                number = number%100;
            }

            var tens = number/10;
            if (tens > 1)
            {
                parts.Add(TensMap[tens]);
                number = number%10;
            }

            if (number > 0)
                parts.Add(UnitsMap[number]);
        }

        private static int GetMappingIndex(int number)
        {
            if (number == 1)
                return 0;

            if (number > 1 && number < 5)
                return 1; //denominator

            var tens = number/10;
            if (tens > 1)
            {
                var unity = number%10;
                if (unity > 1 && unity < 5)
                    return 1; //denominator
            }

            return 2; //genitive
        }

        public override string Convert(int number)
        {
            if (number == 0)
                return "zero";

            var parts = new List<string>();

            if (number < 0)
            {
                parts.Add("minus");
                number = -number;
            }

            var milliard = number/1000000000;
            if (milliard > 0)
            {
                if (milliard > 1)
                    CollectPartsUnderThousand(parts, milliard);

                var map = new[] { "miliard", "miliardy", "miliardów" }; //one, denominator, genitive
                parts.Add(map[GetMappingIndex(milliard)]);
                number %= 1000000000;
            }

            var million = number/1000000;
            if (million > 0)
            {
                if (million > 1)
                    CollectPartsUnderThousand(parts, million);

                var map = new[] { "milion", "miliony", "milionów" }; //one, denominator, genitive
                parts.Add(map[GetMappingIndex(million)]);
                number %= 1000000;
            }

            var thouthand = number/1000;
            if (thouthand > 0)
            {
                if (thouthand > 1)
                    CollectPartsUnderThousand(parts, thouthand);

                var thousand = new[] { "tysiąc", "tysiące", "tysięcy" }; //one, denominator, genitive
                parts.Add(thousand[GetMappingIndex(thouthand)]);
                number %= 1000;
            }

            var units = number/1;
            if (units > 0)
                CollectPartsUnderThousand(parts, units);

            return string.Join(" ", parts);
        }

        public override string ConvertToOrdinal(int number)
        {
            return number.ToString(_culture);
        }
    }
}
