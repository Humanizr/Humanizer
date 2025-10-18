namespace Humanizer;

class ArmenianNumberToWordsConverter :
    GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["զրո", "մեկ", "երկու", "երեք", "չորս", "հինգ", "վեց", "յոթ", "ութ", "ինը", "տաս", "տասնմեկ", "տասներկու", "տասներեք", "տասնչորս", "տասնհինգ", "տասնվեց", "տասնյոթ", "տասնութ", "տասնինը"];
    static readonly string[] TensMap = ["զրո", "տաս", "քսան", "երեսուն", "քառասուն", "հիսուն", "վաթսուն", "յոթանասուն", "ութսուն", "իննսուն"];

    static readonly FrozenDictionary<long, string> OrdinalExceptions = new Dictionary<long, string>
    {
        {
            0, "զրոյական"
        },
        {
            1, "առաջին"
        },
        {
            2, "երկրորդ"
        },
        {
            3, "երրորդ"
        },
        {
            4, "չորրորդ"
        }
    }.ToFrozenDictionary();

    public override string Convert(long number) =>
        ConvertImpl(number, false);

    public override string ConvertToOrdinal(int number)
    {
        if (ExceptionNumbersToWords(number, out var exceptionString))
        {
            return exceptionString;
        }

        return ConvertImpl(number, true);
    }

    string ConvertImpl(long number, bool isOrdinal)
    {
        if (number == 0)
        {
            return GetUnitValue(0, isOrdinal);
        }

        if (number == long.MinValue)
        {
            return "մինուս ինը քվինտիլիոն " +
                   "երկու հարյուր քսաներեք կվադրիլիոն " +
                   "երեք հարյուր յոթանասուներկու տրիլիոն " +
                   "երեսունվեց միլիարդ " +
                   "ութ հարյուր հիսունչորս միլիոն " +
                   "յոթ հարյուր յոթանասունհինգ հազար " +
                   "ութ հարյուր ութ";
        }

        if (number < 0)
        {
            return $"մինուս {ConvertImpl(-number, isOrdinal)}";
        }

        var parts = new List<string>();

        if (number / 1000000000000000000 > 0)
        {
            parts.Add($"{Convert(number / 1000000000000000000)} քվինտիլիոն");
            number %= 1000000000000000000;
        }

        if (number / 1000000000000000 > 0)
        {
            parts.Add($"{Convert(number / 1000000000000000)} կվադրիլիոն");
            number %= 1000000000000000;
        }

        if (number / 1000000000000 > 0)

        {
            parts.Add($"{Convert(number / 1000000000000)} տրիլիոն");
            number %= 1000000000000;
        }

        if (number / 1000000000 > 0)
        {
            parts.Add($"{Convert(number / 1000000000)} միլիարդ");
            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            parts.Add($"{Convert(number / 1000000)} միլիոն");
            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            if (number / 1000 == 1)
            {
                parts.Add("հազար");
            }
            else
            {
                parts.Add($"{Convert(number / 1000)} հազար");
            }

            number %= 1000;
        }

        if (number / 100 > 0)
        {
            if (number / 100 == 1)
            {
                parts.Add("հարյուր");
            }
            else
            {
                parts.Add($"{Convert(number / 100)} հարյուր");
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
                if (number % 10 > 0)
                {
                    lastPart += $"{GetUnitValue(number % 10, isOrdinal)}";
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
            parts[^1] += "երորդ";
        }

        var toWords = string.Join(" ", parts);

        //if (isOrdinal)
        //{
        //    toWords = RemoveOnePrefix(toWords);
        //}

        return toWords;
    }

    static string GetUnitValue(long number, bool isOrdinal)
    {
        if (isOrdinal)
        {
            return UnitsMap[number] + "երորդ";
        }

        return UnitsMap[number];
    }

    static bool ExceptionNumbersToWords(long number, [NotNullWhen(true)] out string? words) =>
        OrdinalExceptions.TryGetValue(number, out words);
}