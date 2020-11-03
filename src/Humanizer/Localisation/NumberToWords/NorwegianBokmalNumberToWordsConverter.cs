using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class NorwegianBokmalNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "null", "en", "to", "tre", "fire", "fem", "seks", "sju", "åtte", "ni", "ti", "elleve", "tolv", "tretten", "fjorten", "femten", "seksten", "sytten", "atten", "nitten" };
        private static readonly string[] TensMap = { "null", "ti", "tjue", "tretti", "førti", "femti", "seksti", "sytti", "åtti", "nitti" };

        private static readonly Dictionary<int, string> OrdinalExceptions = new Dictionary<int, string>
        {
            {0, "nullte"},
            {1, "første"},
            {2, "andre"},
            {3, "tredje"},
            {4, "fjerde"},
            {5, "femte"},
            {6, "sjette"},
            {11, "ellevte"},
            {12, "tolvte"}
        };

        public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        {
            if (number > Int32.MaxValue || number < Int32.MinValue)
            {
                throw new NotImplementedException();
            }

            return Convert((int)number, false, gender);
        }

        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return Convert(number, true, gender);
        }

        private string Convert(int number, bool isOrdinal, GrammaticalGender gender)
        {
            if (number == 0)
            {
                return GetUnitValue(0, isOrdinal);
            }

            if (number < 0)
            {
                return string.Format("minus {0}", Convert(-number, isOrdinal, gender));
            }

            if (number == 1)
            {
                switch (gender)
                {
                    case GrammaticalGender.Feminine:
                        return "ei";
                    case GrammaticalGender.Neuter:
                        return "et";
                }
            }

            var parts = new List<string>();

            var millionOrMore = false;

            const int billion = 1000000000;
            if ((number / billion) > 0)
            {
                millionOrMore = true;
                var isExactOrdinal = isOrdinal && number % billion == 0;
                parts.Add(Part("{0} milliard" + (isExactOrdinal ? "" : "er"), (isExactOrdinal ? "" : "en ") + "milliard", number / billion, !isExactOrdinal));
                number %= billion;
            }

            const int million = 1000000;
            if ((number / million) > 0)
            {
                millionOrMore = true;
                var isExactOrdinal = isOrdinal && number % million == 0;
                parts.Add(Part("{0} million" + (isExactOrdinal ? "" : "er"), (isExactOrdinal ? "" : "en ") + "million", number / million, !isExactOrdinal));
                number %= million;
            }

            var thousand = false;
            if ((number / 1000) > 0)
            {
                thousand = true;
                parts.Add(Part("{0}tusen", number % 1000 < 100 ? "tusen" : "ettusen", number / 1000));
                number %= 1000;
            }

            var hundred = false;
            if ((number / 100) > 0)
            {
                hundred = true;
                parts.Add(Part("{0}hundre", thousand || millionOrMore ? "ethundre" : "hundre", number / 100));
                number %= 100;
            }

            if (number > 0)
            {
                if (parts.Count != 0)
                {
                    if (millionOrMore && !hundred && !thousand)
                    {
                        parts.Add("og ");
                    }
                    else
                    {
                        parts.Add("og");
                    }
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
                        lastPart += string.Format("{0}", GetUnitValue(number % 10, isOrdinal));
                    }
                    else if (isOrdinal)
                    {
                        lastPart = lastPart.TrimEnd('e') + "ende";
                    }

                    parts.Add(lastPart);
                }
            }
            else if (isOrdinal)
            {
                parts[parts.Count - 1] += (number == 0 ? "" : "en") + (millionOrMore ? "te" : "de");
            }

            var toWords = string.Join("", parts.ToArray()).Trim();

            return toWords;
        }

        private static string GetUnitValue(int number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                if (ExceptionNumbersToWords(number, out var exceptionString))
                {
                    return exceptionString;
                }
                else if (number < 13)
                {
                    return UnitsMap[number].TrimEnd('e') + "ende";
                }
                else
                {
                    return UnitsMap[number] + "de";
                }
            }
            else
            {
                return UnitsMap[number];
            }
        }

        private static bool ExceptionNumbersToWords(int number, out string words)
        {
            return OrdinalExceptions.TryGetValue(number, out words);
        }

        private string Part(string pluralFormat, string singular, int number, bool postfixSpace = false)
        {
            var postfix = postfixSpace ? " " : "";
            if (number == 1)
            {
                return singular + postfix;
            }

            return string.Format(pluralFormat, Convert(number)) + postfix;
        }
    }
}
