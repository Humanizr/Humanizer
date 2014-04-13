using System.Collections.Generic;
using Humanizer.Localisation.GrammaticalNumber;

namespace Humanizer.Localisation.NumberToWords
{
    internal class RussianNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] HunderdsMap = { "ноль", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
        private static readonly string[] TensMap = { "ноль", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
        private static readonly string[] UnitsMap = { "ноль", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };

        private static void CollectPartsUnderThousand(ICollection<string> parts, int number, GrammaticalGender gender)
        {
            var hunderds = number / 100;
            if (hunderds > 0)
            {
                parts.Add(HunderdsMap[hunderds]);
                number %= 100;
            }

            var tens = number / 10;
            if (tens > 1)
            {
                parts.Add(TensMap[tens]);
                number %= 10;
            }

            if (number > 0)
            {
                if (number == 1 && gender == GrammaticalGender.Feminine)
                    parts.Add("одна");
                else if (number == 1 && gender == GrammaticalGender.Neuter)
                    parts.Add("одно");
                else if (number == 2 && gender == GrammaticalGender.Feminine)
                    parts.Add("две");
                else if (number < 20)
                    parts.Add(UnitsMap[number]);
            }
        }

        public override string Convert(int number)
        {
            return Convert(number, GrammaticalGender.Masculine);
        }

        public override string Convert(int number, GrammaticalGender gender)
        {
            if (number == 0)
                return "ноль";

            var parts = new List<string>();

            if (number < 0)
            {
                parts.Add("минус");
                number = -number;
            }

            var milliards = number / 1000000000;
            if (milliards > 0)
            {
                CollectPartsUnderThousand(parts, milliards, GrammaticalGender.Masculine);
                var map = new[] { "миллиард", "миллиарда", "миллиардов" };
                var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(milliards);
                parts.Add(map[GetIndex(grammaticalNumber)]);
                number %= 1000000000;
            }

            var millions = number / 1000000;
            if (millions > 0)
            {
                CollectPartsUnderThousand(parts, millions, GrammaticalGender.Masculine);
                var map = new[] { "миллион", "миллиона", "миллионов" };
                var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(millions);
                parts.Add(map[GetIndex(grammaticalNumber)]);
                number %= 1000000;
            }

            var thousands = number / 1000;
            if (thousands > 0)
            {
                CollectPartsUnderThousand(parts, thousands, GrammaticalGender.Feminine);
                var map = new[] { "тысяча", "тысячи", "тысячь" };
                var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(thousands);
                parts.Add(map[GetIndex(grammaticalNumber)]);
                number %= 1000;
            }

            if (number > 0)
                CollectPartsUnderThousand(parts, number, gender);

            return string.Join(" ", parts);
        }

        private static int GetIndex(RussianGrammaticalNumber grammaticalNumber)
        {
            if (grammaticalNumber == RussianGrammaticalNumber.Singular)
                return 0;

            if (grammaticalNumber == RussianGrammaticalNumber.Paucal)
                return 1;

            return 2;
        }
    }
}