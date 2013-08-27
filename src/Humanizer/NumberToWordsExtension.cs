using System;
using System.Collections.Generic;

namespace Humanizer
{
    public static class NumberToWordsExtension
    {
        // http://stackoverflow.com/questions/2729752/converting-numbers-in-to-words-c-sharp
        /// <summary>
        /// 3501.ToWords() -> "three thousand five hundred and one"
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToWords(this int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return string.Format("minus {0}", ToWords(Math.Abs(number)));

            var parts = new List<string>();

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0} million", ToWords(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0} thousand", ToWords(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0} hundred", ToWords(number / 100)));
                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                    parts.Add("and");

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    parts.Add(unitsMap[number]);
                else
                {
                    var lastPart = tensMap[number / 10];
                    if ((number % 10) > 0)
                        lastPart += string.Format("-{0}", unitsMap[number % 10]);
                    parts.Add(lastPart);
                }
            }

            return string.Join(" ", parts);
        }
    }
}
