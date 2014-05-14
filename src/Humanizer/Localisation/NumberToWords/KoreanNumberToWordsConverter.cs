using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class KoreanNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "��", "�ϳ�", "��", "��", "��", "�ټ�", "����", "�ϰ�", "����", "��ȩ"/*, "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"*/ };
        private static readonly string[] TensMap = { "��", "��", "����", "����", "����", "��", "����", "����", "����", "����" };

        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {1, "ù ��°"},
            {2, "�� ��°"},
            {3, "�� ��°"},
            {4, "�� ��°"},
            {11, "���� ��°"},
            {12, "���� ��°"},
            {13, "���� ��°"},
            {14, "���� ��°"},
            {20, "���� ��°"},
            {21, "������ ��°"},
            {22, "������ ��°"},
            {23, "������ ��°"},
            {24, "������ ��°"},
            {31, "������ ��°"},
            {32, "������ ��°"},
            {33, "������ ��°"},
            {34, "������ ��°"},
            {41, "������ ��°"},
            {42, "����� ��°"},
            {43, "���缼 ��°"},
            {44, "����� ��°"},
            {51, "���� ��°"},
            {52, "���� ��°"},
            {53, "���� ��°"},
            {54, "���� ��°"},
            {61, "������ ��°"},
            {62, "������ ��°"},
            {63, "������ ��°"},
            {64, "������ ��°"},
            {71, "������ ��°"},
            {72, "����� ��°"},
            {73, "���缼 ��°"},
            {74, "����� ��°"},
            {81, "������ ��°"},
            {82, "����� ��°"},
            {83, "���缼 ��°"},
            {84, "����� ��°"},
            {91, "������ ��°"},
            {92, "����� ��°"},
            {93, "���缼 ��°"},
            {94, "����� ��°"}
        };

        public override string Convert(int number)
        {
            if (number == 0)
                return "��";

            if (number < 0)
                return string.Format("-{0}", Convert(-number));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0}�� ��", Convert(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0}�� ��", Convert(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0}õ", Convert(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0}��", Convert(number / 100)));
                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                    parts.Add("��");

                if (number < 20)
                    parts.Add(UnitsMap[number]);
                else
                {
                    var lastPart = TensMap[number / 10];
                    if ((number % 10) > 0)
                        lastPart += string.Format("-{0}", UnitsMap[number % 10]);

                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts.ToArray());
        }

        public override string ConvertToOrdinal(int number)
        {
            string towords;
            // 9 => ninth
            if (ExceptionNumbersToWords(number, out towords))
                return towords;

            // 21 => twenty first
            if (number > 20)
            {
                string exceptionPart;
                if (ExceptionNumbersToWords(number%10, out exceptionPart))
                {
                    var normalPart = number - number%10;
                    towords = RemoveOnePrefix(Convert(normalPart));
                    return towords + " " + exceptionPart;
                }
            }

            return NormalNumberToWords(number);
        }

        private string NormalNumberToWords(int number)
        {
            string towords = Convert(number).Replace('-', ' ');

            towords = RemoveOnePrefix(towords);
            // twenty => twentieth
            //if (towords.EndsWith("y"))
            //    towords = towords.TrimEnd('y') + "ie";

            return towords + "��°";
        }

        private static string RemoveOnePrefix(string towords)
        {
            // one hundred => hundredth
            if (towords.IndexOf("��", StringComparison.Ordinal) == 0)
                towords = towords.Remove(0, 4);

            return towords;
        }

        private static bool ExceptionNumbersToWords(int number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}