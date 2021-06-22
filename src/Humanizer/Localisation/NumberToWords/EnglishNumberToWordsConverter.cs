using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class EnglishNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private static readonly string[] TensMap = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        private static readonly Dictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
        {
            {1, "first"},
            {2, "second"},
            {3, "third"},
            {4, "fourth"},
            {5, "fifth"},
            {8, "eighth"},
            {9, "ninth"},
            {12, "twelfth"},
        };

        public override string Convert(long number)
        {
            return Convert(number, false);
        }

        public override string Convert(long number, bool addAnd = true)
        {
            return Convert(number, false, addAnd);
        }

        public override string ConvertToOrdinal(int number)
        {
            return Convert(number, true);
        }

        private string Convert(long number, bool isOrdinal, bool addAnd = true)
        {
            if (number == 0)
            {
                return GetUnitValue(0, isOrdinal);
            }

            if (number < 0)
            {
                return string.Format("minus {0}", Convert(-number));
            }

            var parts = new List<string>();

            if ((number / 1000000000000000000) > 0)
            {
                parts.Add(string.Format("{0} quintillion", Convert(number / 1000000000000000000)));
                number %= 1000000000000000000;
            }

            if ((number / 1000000000000000) > 0)
            {
                parts.Add(string.Format("{0} quadrillion", Convert(number / 1000000000000000)));
                number %= 1000000000000000;
            }

            if ((number / 1000000000000) > 0)
            {
                parts.Add(string.Format("{0} trillion", Convert(number / 1000000000000)));
                number %= 1000000000000;
            }

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
                if (parts.Count != 0 && addAnd)
                {
                    parts.Add("and");
                }

                if (number < 20)
                {
                    parts.Add(GetUnitValue(number, isOrdinal));
                }
                else
                {
                    var lastPart = TensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        lastPart += string.Format("-{0}", GetUnitValue(number % 10, isOrdinal));
                    }
                    else if (isOrdinal)
                    {
                        lastPart = lastPart.TrimEnd('y') + "ieth";
                    }

                    parts.Add(lastPart);
                }
            }
            else if (isOrdinal)
            {
                parts[parts.Count - 1] += "th";
            }

            var toWords = string.Join(" ", parts.ToArray());

            if (isOrdinal)
            {
                toWords = RemoveOnePrefix(toWords);
            }

            return toWords;
        }

        private static string GetUnitValue(long number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                if (ExceptionNumbersToWords(number, out var exceptionString))
                {
                    return exceptionString;
                }
                else
                {
                    return UnitsMap[number] + "th";
                }
            }
            else
            {
                return UnitsMap[number];
            }
        }

        private static string RemoveOnePrefix(string toWords)
        {
            // one hundred => hundredth
            if (toWords.StartsWith("one", StringComparison.Ordinal))
            {
                toWords = toWords.Remove(0, 4);
            }

            return toWords;
        }

        private static bool ExceptionNumbersToWords(long number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }

        public override string ConvertToTuple(int number)
        {
            switch (number)
            {
                case 1:
                    return "single";
                case 2:
                    return "double";
                case 3:
                    return "triple";
                case 4:
                    return "quadruple";
                case 5:
                    return "quintuple";
                case 6:
                    return "sextuple";
                case 7:
                    return "septuple";
                case 8:
                    return "octuple";
                case 9:
                    return "nonuple";
                case 10:
                    return "decuple";
                case 100:
                    return "centuple";
                case 1000:
                    return "milluple";
                default:
                    return $"{number}-tuple";
            }
        }
    }
}
