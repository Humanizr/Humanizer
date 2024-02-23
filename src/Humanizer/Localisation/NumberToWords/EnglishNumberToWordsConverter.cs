namespace Humanizer
{
    class EnglishNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        static readonly string[] UnitsMap = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];
        static readonly string[] TensMap = ["zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

        static readonly Dictionary<long, string> OrdinalExceptions = new()
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

        public override string Convert(long number) =>
            Convert(number, false);

        public override string Convert(long number, bool addAnd = true) =>
            Convert(number, false, addAnd);

        public override string ConvertToOrdinal(int number) =>
            Convert(number, true);

        string Convert(long number, bool isOrdinal, bool addAnd = true)
        {
            if (number == 0)
            {
                return GetUnitValue(0, isOrdinal);
            }

            if (number < 0)
            {
                return $"minus {Convert(-number)}";
            }

            var parts = new List<string>();

            if (number / 1000000000000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000000000000)} quintillion");
                number %= 1000000000000000000;
            }

            if (number / 1000000000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000000000)} quadrillion");
                number %= 1000000000000000;
            }

            if (number / 1000000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000000)} trillion");
                number %= 1000000000000;
            }

            if (number / 1000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000)} billion");
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000)} million");
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                parts.Add($"{Convert(number / 1000)} thousand");
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                parts.Add($"{Convert(number / 100)} hundred");
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
                    if (number % 10 > 0)
                    {
                        lastPart += $"-{GetUnitValue(number % 10, isOrdinal)}";
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
                parts[^1] += "th";
            }

            var toWords = string.Join(" ", parts);

            if (isOrdinal)
            {
                toWords = RemoveOnePrefix(toWords);
            }

            return toWords;
        }

        static string GetUnitValue(long number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                if (ExceptionNumbersToWords(number, out var exceptionString))
                {
                    return exceptionString;
                }

                return UnitsMap[number] + "th";
            }

            return UnitsMap[number];
        }

        static string RemoveOnePrefix(string toWords)
        {
            // one hundred => hundredth
            if (toWords.StartsWith("one", StringComparison.Ordinal))
            {
                toWords = toWords.Remove(0, 4);
            }

            return toWords;
        }

        static bool ExceptionNumbersToWords(long number, [NotNullWhen(true)] out string? words) =>
            OrdinalExceptions.TryGetValue(number, out words);

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
