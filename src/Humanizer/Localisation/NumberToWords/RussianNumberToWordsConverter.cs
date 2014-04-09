using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class RussianNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private enum GrammaticalGender
        {
            Masculine,
            Feminine,
            Neuter
        }

        private enum GrammaticalNumber
        {
            Singular,
            Paucal,
            Plural
        }

        private static GrammaticalNumber ToRussianGrammaticalNumber(int number)
        {
            var mod100 = number % 100;
            if (mod100 / 10 != 1)
            {
                var mod10 = number % 10;

                if (mod10 == 1) // 1, 21, 31, 41 ... 91, 101, 121 ..
                    return GrammaticalNumber.Singular;

                if (mod10 > 1 && mod10 < 5) // 2, 3, 4, 22, 23, 24 ...
                    return GrammaticalNumber.Paucal;
            }

            return GrammaticalNumber.Plural;
        }

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
                var grammaticalNumber = ToRussianGrammaticalNumber(milliards);
                parts.Add(string.Format("{0} {1}", ToWordsUnderThousand(milliards, GrammaticalGender.Masculine), map[(int)grammaticalNumber]));
                number %= 1000000000;
            }

            var millions = number / 1000000;
            if (millions > 0)
            {
                var map = new[] { "миллион", "миллиона", "миллионов" };
                var grammaticalNumber = ToRussianGrammaticalNumber(millions);
                parts.Add(string.Format("{0} {1}", ToWordsUnderThousand(millions, GrammaticalGender.Masculine), map[(int)grammaticalNumber]));
                number %= 1000000;
            }

            var thousands = number / 1000;
            if (thousands > 0)
            {
                var map = new[] { "тысяча", "тысячи", "тысячь" };
                var grammaticalNumber = ToRussianGrammaticalNumber(thousands);
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
