using System;
using System.Collections.Generic;

namespace Humanizer.Localisation
{
    internal class EnglishNumberToWordsConverter : INumberToWordsConverter
    {
        public string Convert(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return string.Format("minus {0}", Math.Abs(number).ToWords());

            var parts = new List<string>();

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} billion", (number / 1000000000).ToWords()));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0} million", (number / 1000000).ToWords()));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                parts.Add(string.Format("{0} thousand", NumberToWordsExtension.ToWords(number / 1000)));
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                parts.Add(string.Format("{0} hundred", NumberToWordsExtension.ToWords(number / 100)));
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

            return string.Join(" ", parts.ToArray());
        }
    }
}