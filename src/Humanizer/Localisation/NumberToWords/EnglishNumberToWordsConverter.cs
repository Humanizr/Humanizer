namespace Humanizer;

class EnglishNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];
    static readonly string[] UnitsMapTh = ["zeroth", "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth", "eleventh", "twelfth", "thirteenth", "fourteenth", "fifteenth", "sixteenth", "seventeenth", "eighteenth", "nineteenth"];
    static readonly string[] TensMap = ["zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

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

        var parts = new List<string>(20);

        CollectParts(parts, ref number, isOrdinal, 1_000_000_000_000_000_000, "quintillion", "quintillionth");
        CollectParts(parts, ref number, isOrdinal, 1_000_000_000_000_000, "quadrillion", "quadrillionth");
        CollectParts(parts, ref number, isOrdinal, 1_000_000_000_000, "trillion", "trillionth");
        CollectParts(parts, ref number, isOrdinal, 1_000_000_000, "billion", "billionth");
        CollectParts(parts, ref number, isOrdinal, 1_000_000, "million", "millionth");
        CollectParts(parts, ref number, isOrdinal, 1_000, "thousand", "thousandth");

        CollectPartsUnderAThousand(parts, number, isOrdinal, addAnd);

        if (isOrdinal && parts[0] == "one")
        {
            // one hundred => hundredth
            parts.RemoveAt(0);
        }

        return string.Join(" ", parts);
    }

    static void CollectParts(List<string> parts, ref long number, bool isOrdinal, long divisor, string word, string ordinal)
    {
        var result = number / divisor;
        if (result == 0)
        {
            return;
        }

        CollectPartsUnderAThousand(parts, result);

        number %= divisor;
        parts.Add(number == 0 && isOrdinal ? ordinal : word);
    }

    static void CollectPartsUnderAThousand(List<string> parts, long number, bool isOrdinal = false, bool addAnd = true)
    {
        if (number >= 100)
        {
            parts.Add(GetUnitValue(number / 100, false));
            number %= 100;
            parts.Add(number == 0 && isOrdinal ? "hundredth" : "hundred");
        }

        if (number == 0)
        {
            return;
        }

        if (parts.Count > 0 && addAnd)
        {
            parts.Add("and");
        }

        if (number >= 20)
        {
            var tens = TensMap[number / 10];
            var units = number % 10;
            if (units == 0)
            {
                parts.Add(isOrdinal ? $"{tens.TrimEnd('y')}ieth" : tens);
            }
            else
            {
                parts.Add($"{tens}-{GetUnitValue(units, isOrdinal)}");
            }
        }
        else
        {
            parts.Add(GetUnitValue(number, isOrdinal));
        }
    }

    static string GetUnitValue(long number, bool isOrdinal) =>
        isOrdinal ? UnitsMapTh[number] : UnitsMap[number];

    public override string ConvertToTuple(int number) =>
        number switch
        {
            1 => "single",
            2 => "double",
            3 => "triple",
            4 => "quadruple",
            5 => "quintuple",
            6 => "sextuple",
            7 => "septuple",
            8 => "octuple",
            9 => "nonuple",
            10 => "decuple",
            100 => "centuple",
            1000 => "milluple",
            _ => $"{number}-tuple"
        };
}