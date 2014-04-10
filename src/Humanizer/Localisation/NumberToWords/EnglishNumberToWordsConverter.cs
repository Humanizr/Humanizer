using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    public class EnglishNumberToWordsConverter : INumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private static readonly string[] TensMap = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {1, "first"},
            {2, "second"},
            {3, "third"},
            {4, "forth"},
            {5, "fifth"},
            {8, "eighth"},
            {9, "ninth"},
            {12, "twelfth"},
        };

        public string Convert(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return string.Format("minus {0}", Convert(-number));

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} billion", Convert(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0} million", Convert(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0} thousand", Convert(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0} hundred", Convert(number / 100)));
                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                    parts.Add("and");

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

        public string ConvertToOrdinal(int number)
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
            if (towords.EndsWith("y"))
                towords = towords.TrimEnd('y') + "ie";

            return towords + "th";
        }

        private static string RemoveOnePrefix(string towords)
        {
            // one hundred => hundredth
            if (towords.IndexOf("one", StringComparison.Ordinal) == 0)
                towords = towords.Remove(0, 4);

            return towords;
        }

        private static bool ExceptionNumbersToWords(int number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}