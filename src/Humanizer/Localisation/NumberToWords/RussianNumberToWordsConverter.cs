using System.Collections.Generic;
using Humanizer.Localisation.GrammaticalNumber;

namespace Humanizer.Localisation.NumberToWords
{
    internal class RussianNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static string ToWordsUnderThousand(int number, GrammaticalGender gender)
        {
            var parts = new List<string>();

            var hunderdsMap = new[] { "ноль", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
            var hunderds = number / 100;
            if (hunderds > 0)
            {
                parts.Add(hunderdsMap[hunderds]);
                number %= 100;
            }

            var tens = number / 10;
            if (tens > 1)
            {
                var tensMap = new[] { "ноль", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
                parts.Add(tensMap[tens]);
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
                else
                {
                    var unitsMap = new[] { "ноль", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
                    if (number < 20)
                        parts.Add(unitsMap[number]);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        public override string Convert(int number)
        {
            if (number == 0)
                return "ноль";

            if (number < 0)
                return string.Format("минус {0}", Convert(-number));

            var parts = new List<string>();

            var milliards = number / 1000000000;
            if (milliards > 0)
            {
                var map = new[] { "миллиард", "миллиарда", "миллиардов" };
                var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(milliards);
                parts.Add(string.Format("{0} {1}", ToWordsUnderThousand(milliards, GrammaticalGender.Masculine), map[(int)grammaticalNumber]));
                number %= 1000000000;
            }

            var millions = number / 1000000;
            if (millions > 0)
            {
                var map = new[] { "миллион", "миллиона", "миллионов" };
                var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(millions);
                parts.Add(string.Format("{0} {1}", ToWordsUnderThousand(millions, GrammaticalGender.Masculine), map[(int)grammaticalNumber]));
                number %= 1000000;
            }

            var thousands = number / 1000;
            if (thousands > 0)
            {
                var map = new[] { "тысяча", "тысячи", "тысячь" };
                var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(thousands);
                parts.Add(string.Format("{0} {1}", ToWordsUnderThousand(thousands, GrammaticalGender.Feminine), map[(int)grammaticalNumber]));
                number %= 1000;
            }

            if (number > 0)
            {
                parts.Add(ToWordsUnderThousand(number, GrammaticalGender.Masculine));
            }

            return string.Join(" ", parts.ToArray());
        }
    }
}