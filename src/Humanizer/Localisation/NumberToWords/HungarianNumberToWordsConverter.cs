namespace Humanizer;

class HungarianNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    // Units
    private static readonly string[] UnitsMap = ["", "egy", "kettő", "három", "négy", "öt", "hat", "hét", "nyolc", "kilenc"];
    private static readonly string[] OrdinalUnitsMap = ["", "első", "második", "harmadik", "negyedik", "ötödik", "hatodik", "hetedik", "nyolcadik", "kilencedik"];

    // Tens
    private static readonly string[] TensMap = ["", "tizen", "huszon", "harminc", "negyven", "ötven", "hatvan", "hetven", "nyolcvan", "kilencven"];
    private static readonly string[] OrdinalTensMap = ["", "tizedik", "huszadik", "harmincadik", "negyvenedik", "ötvenedik", "hatvanadik", "hetvenedik", "nyolcvanadik", "kilencvenedik"];

    // Hundreds
    private static readonly string[] HundredsMap = ["", "száz", "kétszáz", "háromszáz", "négyszáz", "ötszáz", "hatszáz", "hétszáz", "nyolcszáz", "kilencszáz"];

    // Exceptional single numbers when used as ordinal numbers and the whole number is greater than 10
    private static readonly Dictionary<long, string> OrdinalUnitsExceptions = new()
    {
        {
            1, "egyedik"
        },
        {
            2, "kettedik"
        }
    };

    // Exceptional ten numbers when the number divided by 10 gives no remainder
    private static readonly Dictionary<long, string> WholeTensExceptions = new()
    {
        {
            10, "tíz"
        },
        {
            20, "húsz"
        }
    };


    public override string Convert(long number) => ConvertInternal(number, false);

    public override string ConvertToOrdinal(int number) => ConvertInternal(number, true);

    private static string ConvertInternal(long number, bool isOrdinal)
    {
        // Handle zero and negative numbers
        switch (number)
        {
            case 0:
                return isOrdinal ? "nulladik" : "nulla";
            case < 0:
                return $"mínusz {ConvertInternal(-number, isOrdinal)}";
        }

        var isLessThanTen = number < 10;
        var parts = new List<string>(10);

        CollectParts(parts, ref number, isOrdinal, isLessThanTen, 1_000_000_000_000_000_000, "trillió", "trilliomodik");
        CollectParts(parts, ref number, isOrdinal, isLessThanTen, 1_000_000_000_000_000, "billiárd", "billiárdodik");
        CollectParts(parts, ref number, isOrdinal, isLessThanTen, 1_000_000_000_000, "billió", "billiomodik");
        CollectParts(parts, ref number, isOrdinal, isLessThanTen, 1_000_000_000, "milliárd", "milliárdodik");
        CollectParts(parts, ref number, isOrdinal, isLessThanTen, 1_000_000, "millió", "milliomodik");

        // All numbers above 2000 should be separated by dashes per thousands
        if (2_000 <= number)
        {
            CollectParts(parts, ref number, isOrdinal, isLessThanTen, 1_000, "ezer", "ezredik");
            var underAThousandPart = GetUnderAThousandPart(number, isOrdinal, false, isLessThanTen);
            if (underAThousandPart != string.Empty)
            {
                parts.Add(underAThousandPart);
            }
        }
        else
        {
            // In hungarian there is no separator between one thousand and the rest of the numbers
            var lastPart = 1_000 <= number ? GetOneThousandPart(ref number, isOrdinal) : "";
            lastPart += GetUnderAThousandPart(number, isOrdinal, false, isLessThanTen);

            if (lastPart != string.Empty)
            {
                parts.Add(lastPart);
            }
        }

        return string.Join("-", parts);
    }

    // Thousands part for numbers between 1000 and 1999
    static string GetOneThousandPart(ref long number, bool isOrdinal)
    {
        const int divisor = 1_000;

        var oneThousandPart = isOrdinal && number == divisor ? "ezredik" : "ezer";

        number %= divisor;
        return oneThousandPart;
    }

    private static void CollectParts(List<string> parts, ref long number, bool isOrdinal, bool isLessThanTen,
        long divisor, string word,
        string ordinal)
    {
        var result = number / divisor;
        if (result == 0)
        {
            return;
        }

        var prefixNumber = GetUnderAThousandPart(result, isOrdinal, true, isLessThanTen);

        number %= divisor;
        parts.Add(number == 0 && isOrdinal ? prefixNumber + ordinal : prefixNumber + word);
    }

    private static string GetUnderAThousandPart(long number, bool isOrdinal, bool isPrefix, bool originalLessThanTen)
    {
        var numberString = "";
        if (100 <= number)
        {
            // Return hundred + "adik" if the number is exactly one of hundreds e.g.: századik, hétszázadik
            if (isOrdinal && number % 100 == 0)
            {
                return HundredsMap[number / 100] + "adik";
            }

            numberString += HundredsMap[number / 100];
            number %= 100;
        }

        if (10 <= number)
        {
            // Return an ordinal ten if the number is exactly one of tens
            if (isOrdinal && number % 10 == 0)
            {
                return numberString + OrdinalTensMap[number / 10];
            }

            numberString += WholeTensExceptions.TryGetValue(number, out var value) ? value : TensMap[number / 10];
            number %= 10;
        }

        if (isOrdinal && !isPrefix)
        {
            numberString += GetOrdinalOnes(number, originalLessThanTen);
        }
        else
        {
            numberString += isPrefix && number == 2 ? "két" : UnitsMap[number];
        }

        return numberString;
    }

    private static string GetOrdinalOnes(long number, bool lessThanTen)
    {
        if (lessThanTen)
        {
            return OrdinalUnitsMap[number];
        }

        return OrdinalUnitsExceptions.TryGetValue(number, out var value) ? value : OrdinalUnitsMap[number];
    }

    public override string ConvertToTuple(int number) =>
        number switch
        {
            1 => "szimpla",
            2 => "dupla",
            3 => "tripla",
            4 => "kvadrupla",
            5 => "pentapla",
            6 => "hexapla",
            7 => "heptapla",
            8 => "octapla",
            9 => "nonapla",
            10 => "dekapla",
            100 => "hektapla",
            1000 => "kiliapla",
            _ => $"{number}"
        };
}