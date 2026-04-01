namespace Humanizer;

class HyphenatedScaleNumberToWordsConverter(HyphenatedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly HyphenatedScaleNumberToWordsProfile profile = profile;

    public override string Convert(long number) => ConvertInternal(number, false);

    public override string ConvertToOrdinal(int number) => ConvertInternal(number, true);

    string ConvertInternal(long number, bool isOrdinal)
    {
        // Handle zero and negative numbers
        switch (number)
        {
            case 0:
                return isOrdinal ? profile.ZeroOrdinalWord : profile.ZeroWord;
            case < 0:
                return $"{profile.MinusWord} {ConvertInternal(-number, isOrdinal)}";
        }

        var isLessThanTen = number < 10;
        var parts = new List<string>(10);

        foreach (var scale in profile.Scales)
        {
            CollectParts(parts, ref number, isOrdinal, isLessThanTen, scale);
        }

        // All numbers above 2000 should be separated by dashes per thousands
        if (2_000 <= number)
        {
            CollectParts(parts, ref number, isOrdinal, isLessThanTen, profile.ThousandScale);
            var underAThousandPart = GetUnderAThousandPart(number, isOrdinal, false, isLessThanTen);
            if (underAThousandPart != string.Empty)
            {
                parts.Add(underAThousandPart);
            }
        }
        else
        {
            // Some hyphenated-scale locales inline the one-thousand segment directly into the
            // under-a-thousand remainder instead of emitting an extra dash boundary.
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
    string GetOneThousandPart(ref long number, bool isOrdinal)
    {
        const int divisor = 1_000;

        var oneThousandPart = isOrdinal && number == divisor
            ? profile.ThousandScale.Ordinal
            : profile.ThousandScale.Cardinal;

        number %= divisor;
        return oneThousandPart;
    }

    void CollectParts(List<string> parts, ref long number, bool isOrdinal, bool isLessThanTen, HyphenatedScale scale)
    {
        var result = number / scale.Divisor;
        if (result == 0)
        {
            return;
        }

        var prefixNumber = GetUnderAThousandPart(result, isOrdinal, true, isLessThanTen);

        number %= scale.Divisor;
        parts.Add(number == 0 && isOrdinal ? prefixNumber + scale.Ordinal : prefixNumber + scale.Cardinal);
    }

    string GetUnderAThousandPart(long number, bool isOrdinal, bool isPrefix, bool originalLessThanTen)
    {
        var numberString = "";
        if (100 <= number)
        {
            // Return hundred + "adik" if the number is exactly one of hundreds e.g.: századik, hétszázadik
            if (isOrdinal && number % 100 == 0)
            {
                return profile.HundredsMap[number / 100] + "adik";
            }

            numberString += profile.HundredsMap[number / 100];
            number %= 100;
        }

        if (10 <= number)
        {
            // Return an ordinal ten if the number is exactly one of tens
            if (isOrdinal && number % 10 == 0)
            {
                return numberString + profile.OrdinalTensMap[number / 10];
            }

            numberString += profile.WholeTensExceptions.TryGetValue((int)number, out var value)
                ? value
                : profile.TensMap[number / 10];
            number %= 10;
        }

        if (isOrdinal && !isPrefix)
        {
            numberString += GetOrdinalOnes(number, originalLessThanTen);
        }
        else
        {
            numberString += isPrefix && number == 2 ? profile.TwoPrefixWord : profile.UnitsMap[number];
        }

        return numberString;
    }

    string GetOrdinalOnes(long number, bool lessThanTen)
    {
        if (lessThanTen)
        {
            return profile.OrdinalUnitsMap[number];
        }

        return profile.OrdinalUnitsExceptions.TryGetValue((int)number, out var value)
            ? value
            : profile.OrdinalUnitsMap[number];
    }

    public override string ConvertToTuple(int number) =>
        profile.TupleMap.TryGetValue(number, out var tuple)
            ? tuple
            : $"{number}";

}

sealed class HyphenatedScaleNumberToWordsProfile(
    string zeroWord,
    string zeroOrdinalWord,
    string minusWord,
    string twoPrefixWord,
    string[] unitsMap,
    string[] ordinalUnitsMap,
    string[] tensMap,
    string[] ordinalTensMap,
    string[] hundredsMap,
    FrozenDictionary<int, string> ordinalUnitsExceptions,
    FrozenDictionary<int, string> wholeTensExceptions,
    HyphenatedScale[] scales,
    HyphenatedScale thousandScale,
    FrozenDictionary<int, string> tupleMap)
{
    public string ZeroWord { get; } = zeroWord;
    public string ZeroOrdinalWord { get; } = zeroOrdinalWord;
    public string MinusWord { get; } = minusWord;
    public string TwoPrefixWord { get; } = twoPrefixWord;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    public string[] HundredsMap { get; } = hundredsMap;
    public FrozenDictionary<int, string> OrdinalUnitsExceptions { get; } = ordinalUnitsExceptions;
    public FrozenDictionary<int, string> WholeTensExceptions { get; } = wholeTensExceptions;
    public HyphenatedScale[] Scales { get; } = scales;
    public HyphenatedScale ThousandScale { get; } = thousandScale;
    public FrozenDictionary<int, string> TupleMap { get; } = tupleMap;
}

readonly record struct HyphenatedScale(long Divisor, string Cardinal, string Ordinal);
