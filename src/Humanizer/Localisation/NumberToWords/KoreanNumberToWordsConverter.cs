using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class KoreanNumberToWordsConverter : DefaultNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "영", "하나", "둘", "셋", "넷", "다섯", "여섯", "일곱", "여덟", "아홉"/*, "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"*/ };
        private static readonly string[] TensMap = { "영", "열", "스물", "서른", "마흔", "쉰", "예순", "일흔", "여든", "아흔" };

        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {1, "첫 번째"},
            {2, "두 번째"},
            {3, "세 번째"},
            {4, "네 번째"},
            {11, "열한 번째"},
            {12, "열두 번째"},
            {13, "열세 번째"},
            {14, "열네 번째"},
            {20, "스무 번째"},
            {21, "스물한 번째"},
            {22, "스물두 번째"},
            {23, "스물세 번째"},
            {24, "스물네 번째"},
            {31, "서른한 번째"},
            {32, "서른두 번째"},
            {33, "서른세 번째"},
            {34, "서른네 번째"},
            {41, "마흔한 번째"},
            {42, "마흔두 번째"},
            {43, "마흔세 번째"},
            {44, "마흔네 번째"},
            {51, "쉰한 번째"},
            {52, "쉰두 번째"},
            {53, "쉰세 번째"},
            {54, "쉰네 번째"},
            {61, "예순한 번째"},
            {62, "예순두 번째"},
            {63, "예순세 번째"},
            {64, "예순네 번째"},
            {71, "일흔한 번째"},
            {72, "일흔두 번째"},
            {73, "일흔세 번째"},
            {74, "일흔네 번째"},
            {81, "여든한 번째"},
            {82, "여든두 번째"},
            {83, "여든세 번째"},
            {84, "여든네 번째"},
            {91, "아흔한 번째"},
            {92, "아흔두 번째"},
            {93, "아흔세 번째"},
            {94, "아흔네 번째"}
        };

        public override string Convert(int number)
        {
            if (number == 0)
                return "영";

            if (number < 0)
                return string.Format("-{0}", Convert(-number));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0}십 억", Convert(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0}백 만", Convert(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0}천", Convert(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0}백", Convert(number / 100)));
                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                    parts.Add("와");

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

            return towords + "번째";
        }

        private static string RemoveOnePrefix(string towords)
        {
            // one hundred => hundredth
            if (towords.IndexOf("일", StringComparison.Ordinal) == 0)
                towords = towords.Remove(0, 4);

            return towords;
        }

        private static bool ExceptionNumbersToWords(int number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}