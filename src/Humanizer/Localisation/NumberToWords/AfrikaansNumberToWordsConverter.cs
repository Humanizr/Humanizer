namespace Humanizer
{
    class AfrikaansNumberToWordsConverter :
        GenderlessNumberToWordsConverter
    {
        static readonly string[] UnitsMap = ["nul", "een", "twee", "drie", "vier", "vyf", "ses", "sewe", "agt", "nege", "tien", "elf", "twaalf", "dertien", "veertien", "vyftien", "sestien", "sewentien", "agtien", "negentien"];
        static readonly string[] TensMap = ["nul", "tien", "twintig", "dertig", "veertig", "vyftig", "sestig", "sewentig", "tagtig", "negentig"];

        static readonly Dictionary<int, string> OrdinalExceptions = new()
        {
            {0, "nulste"},
            {1, "eerste"},
            {3, "derde"},
            {7, "sewende"},
            {8, "agste"},
            {9, "negende"},
            {10, "tiende"},
            {14, "veertiende"},
            {17, "sewentiende"},
            {19, "negentiende"}
        };

        public override string Convert(long number)
        {
            if (number is > int.MaxValue or < int.MinValue)
            {
                throw new NotImplementedException();
            }
            return Convert((int)number, false);
        }

        public override string ConvertToOrdinal(int number) =>
            Convert(number, true);

        string Convert(int number, bool isOrdinal)
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

            if (number / 1000000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000000)} miljard");
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                parts.Add($"{Convert(number / 1000000)} miljoen");
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                parts.Add($"{Convert(number / 1000)} duisend");
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                parts.Add($"{Convert(number / 100)} honderd");
                number %= 100;
            }

            if (number > 0)
            {
                //if (parts.Count != 0)
                //    parts.Add("en");

                if (number < 20)
                {
                    if (parts.Count > 0)
                    {
                        parts.Add("en");
                    }

                    parts.Add(GetUnitValue(number, isOrdinal));
                }
                else
                {
                    var lastPartValue = number / 10 * 10;
                    var lastPart = TensMap[number / 10];
                    if (number % 10 > 0)
                    {
                        lastPart = $"{GetUnitValue(number % 10, false)} en {(isOrdinal ? GetUnitValue(lastPartValue, true) : lastPart)}";
                    }
                    else if (number % 10 == 0)
                    {
                        lastPart = $"{(parts.Count > 0 ? "en " : "")}{lastPart}{(isOrdinal ? "ste" : "")}";
                    }
                    else if (isOrdinal)
                    {
                        lastPart = lastPart.TrimEnd('~') + "ste";
                    }

                    parts.Add(lastPart);
                }
            }
            else if (isOrdinal)
            {
                parts[parts.Count - 1] += "ste";
            }

            var toWords = string.Join(" ", parts);

            if (isOrdinal)
            {
                toWords = RemoveOnePrefix(toWords);
            }

            return toWords;
        }

        static string GetUnitValue(int number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                if (ExceptionNumbersToWords(number, out var exceptionString))
                {
                    return exceptionString;
                }
                else if (number > 19)
                {
                    return TensMap[number / 10] + "ste";
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

        static string RemoveOnePrefix(string toWords)
        {
            // one hundred => hundredth
            if (toWords.StartsWith("een", StringComparison.Ordinal))
            {
                if (toWords.IndexOf("een en", StringComparison.Ordinal) != 0)
                {
                    toWords = toWords.Remove(0, 4);
                }
            }

            return toWords;
        }

        static bool ExceptionNumbersToWords(int number, out string words) =>
            OrdinalExceptions.TryGetValue(number, out words);
    }
}
