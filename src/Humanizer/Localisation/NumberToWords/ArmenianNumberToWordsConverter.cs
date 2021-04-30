using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class ArmenianNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "զրո", "մեկ", "երկու", "երեք", "չորս", "հինգ", "վեց", "յոթ", "ութ", "ինը", "տաս", "տասնմեկ", "տասներկու", "տասներեք", "տասնչորս", "տասնհինգ", "տասնվեց", "տասնյոթ", "տասնութ", "տասնինը" };
        private static readonly string[] TensMap = { "զրո", "տաս", "քսան", "երեսուն", "քառասուն", "հիսուն", "վաթսուն", "յոթանասուն", "ութսուն", "իննսուն" };

        private static readonly Dictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
        {
            {0, "զրոյական"},
            {1, "առաջին"},
            {2, "երկրորդ"},
            {3, "երրորդ"},
            {4, "չորրորդ"}
        };

        public override string Convert(long number)
        {
            return ConvertImpl(number, false);
        }

        public override string ConvertToOrdinal(int number)
        {
            if (ExceptionNumbersToWords(number, out var exceptionString))
            {
                return exceptionString;
            }

            return ConvertImpl(number, true);
        }

        private string ConvertImpl(long number, bool isOrdinal)
        {
            if (number == 0)
            {
                return GetUnitValue(0, isOrdinal);
            }

            if (number == long.MinValue)
            {
                return  "մինուս ինը քվինտիլիոն " +
                        "երկու հարյուր քսաներեք կվադրիլիոն " +
                        "երեք հարյուր յոթանասուներկու տրիլիոն " +
                        "երեսունվեց միլիարդ " +
                        "ութ հարյուր հիսունչորս միլիոն " +
                        "յոթ հարյուր յոթանասունհինգ հազար " +
                        "ութ հարյուր ութ";
            }

            if (number < 0)
            {
                return string.Format("մինուս {0}", ConvertImpl(-number, isOrdinal));
            }

            var parts = new List<string>();

            if ((number / 1000000000000000000) > 0)
            {
                parts.Add(string.Format("{0} քվինտիլիոն", Convert(number / 1000000000000000000)));
                number %= 1000000000000000000;
            }

            if ((number / 1000000000000000) > 0)
            {
                parts.Add(string.Format("{0} կվադրիլիոն", Convert(number / 1000000000000000)));
                number %= 1000000000000000;
            }

            if ((number / 1000000000000) > 0)

            {
                parts.Add(string.Format("{0} տրիլիոն", Convert(number / 1000000000000)));
                number %= 1000000000000;
            }

            if ((number / 1000000000) > 0)
            {
                parts.Add(string.Format("{0} միլիարդ", Convert(number / 1000000000)));
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                parts.Add(string.Format("{0} միլիոն", Convert(number / 1000000)));
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                if ((number / 1000) == 1)
                {
                    parts.Add("հազար");
                }
                else
                {
                    parts.Add(string.Format("{0} հազար", Convert(number / 1000)));
                }

                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                if ((number / 100) == 1)
                {
                    parts.Add("հարյուր");
                }
                else
                {
                    parts.Add(string.Format("{0} հարյուր", Convert(number / 100)));
                }

                number %= 100;
            }

            if (number > 0)
            {
                if (number < 20)
                {
                    parts.Add(GetUnitValue(number, isOrdinal));
                }
                else
                {
                    var lastPart = TensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        lastPart += string.Format("{0}", GetUnitValue(number % 10, isOrdinal));
                    }
                    else if (isOrdinal)
                    {
                        lastPart += "երորդ";
                    }

                    parts.Add(lastPart);
                }
            }
            else if (isOrdinal)
            {
                parts[parts.Count - 1] += "երորդ";
            }

            var toWords = string.Join(" ", parts.ToArray());

            //if (isOrdinal)
            //{
            //    toWords = RemoveOnePrefix(toWords);
            //}

            return toWords;
        }

        private static string GetUnitValue(long number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                return UnitsMap[number] + "երորդ";
            }
            else
            {
                return UnitsMap[number];
            }
        }

        private static string RemoveOnePrefix(string toWords)
        {
            // one hundred => hundredth
            if (toWords.StartsWith("մեկ", StringComparison.Ordinal))
            {
                toWords = toWords.Remove(0, 4);
            }

            return toWords;
        }

        private static bool ExceptionNumbersToWords(long number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }
    }
}
