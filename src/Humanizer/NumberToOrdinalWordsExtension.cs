using System;
using System.Collections.Generic;

namespace Humanizer
{
    /// <summary>
    /// Transforms a number into ordinal words; e.g. 1 => first
    /// </summary>
    public static class NumberToOrdinalWordsExtension
    {
        /// <summary>
        /// 1.ToOrdinalWords() -> "first"
        /// </summary>
        /// <param name="number">Number to be turned to ordinal words</param>
        /// <returns></returns>
        public static string ToOrdinalWords(this int number)
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
                    towords = normalPart.ToWords();
                    return towords + " " + exceptionPart;
                }
            }

            return NormalNumberToWords(number);
        }

        private static string NormalNumberToWords(int number)
        {
            string towords = number.ToWords().Replace('-', ' ');

            // one hundred => hundredth
            if (towords.IndexOf("one", StringComparison.Ordinal) == 0)
                towords = towords.Remove(0, 4);
            // twenty => twentieth
            if (towords.EndsWith("y"))
                towords = towords.TrimEnd('y') + "ie";

            return towords + "th";
        }

        static bool ExceptionNumbersToWords(int number, out string words)
        {
            var exceptions = new Dictionary<int, string>
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

            return exceptions.TryGetValue(number, out words);
        }
    }
}
