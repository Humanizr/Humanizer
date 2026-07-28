namespace Humanizer;

/// <summary>
/// Renders numbers for locales that hyphenate scale phrases and rely on generated tables for
/// hundreds, tens, units, and ordinal exceptions.
/// </summary>
/// <remarks>
/// The converter walks the configured scale rows from largest to smallest, then stitches the
/// remaining under-thousand fragment with the locale's hyphenation and ordinal rules.
/// </remarks>
class HyphenatedScaleNumberToWordsConverter(HyphenatedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly HyphenatedScaleNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number) => ConvertInternal(number, false);

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number) => ConvertInternal(number, true);

    /// <summary>
    /// Shared cardinal and ordinal rendering path for the hyphenated-scale family.
    /// </summary>
    string ConvertInternal(long number, bool isOrdinal)
    {
        // Zero and negative values short-circuit before the scale walk because this family only
        // models the positive hyphenation grammar.
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

        // Above 2,000 the thousand row becomes a hard hyphen boundary; folding it into the
        // remaining under-thousand render would produce the wrong surface form.
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
            // Some locales fuse 1,000 into the remainder instead of emitting a separate thousand
            // dash, so the under-thousand fragment has to be rendered with that boundary context.
            var lastPart = 1_000 <= number ? GetOneThousandPart(ref number, isOrdinal) : "";
            lastPart += GetUnderAThousandPart(number, isOrdinal, false, isLessThanTen);

            if (lastPart != string.Empty)
            {
                parts.Add(lastPart);
            }
        }

        return string.Join("-", parts);
    }

    /// <summary>
    /// Returns the one-thousand segment used by locales that spell 1,000-1,999 as a direct prefix.
    /// </summary>
    string GetOneThousandPart(ref long number, bool isOrdinal)
    {
        const int divisor = 1_000;

        var oneThousandPart = isOrdinal && number == divisor
            ? profile.ThousandScale.Ordinal
            : profile.ThousandScale.Cardinal;

        number %= divisor;
        return oneThousandPart;
    }

    /// <summary>
    /// Decomposes the current scale row and appends the localized result to <paramref name="parts"/>.
    /// </summary>
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

    /// <summary>
    /// Renders the fragment below one thousand, optionally using ordinal forms for the terminal digit.
    /// </summary>
    string GetUnderAThousandPart(long number, bool isOrdinal, bool isPrefix, bool originalLessThanTen)
    {
        var numberString = "";
        if (100 <= number)
        {
            // Exact hundreds use a dedicated ordinal stem, so the cardinal hundred word must not
            // be reused with a generic suffix.
            if (isOrdinal && number % 100 == 0)
            {
                return profile.HundredsMap[number / 100] + "adik";
            }

            numberString += profile.HundredsMap[number / 100];
            number %= 100;
        }

        if (10 <= number)
        {
            // Exact tens follow the same rule: the locale already encodes the correct ordinal stem
            // in the tens table, so suffixing the cardinal form would be wrong.
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

    /// <summary>
    /// Returns the ordinal unit word, including locale-specific exceptions when the value is not a direct suffix case.
    /// </summary>
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

    /// <inheritdoc/>
    public override string ConvertToTuple(int number) =>
        profile.TupleMap.TryGetValue(number, out var tuple)
            ? tuple
            : $"{number}";
}

/// <summary>
/// Immutable generated data for <see cref="HyphenatedScaleNumberToWordsConverter"/>.
/// </summary>
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
    /// <summary>Gets the cardinal zero word.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the ordinal zero word.</summary>
    public string ZeroOrdinalWord { get; } = zeroOrdinalWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the prefix used when the locale spells the unit digit as a leading stem.</summary>
    public string TwoPrefixWord { get; } = twoPrefixWord;
    /// <summary>Gets the cardinal unit lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the ordinal unit lexicon.</summary>
    public string[] OrdinalUnitsMap { get; } = ordinalUnitsMap;
    /// <summary>Gets the cardinal tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the ordinal tens lexicon.</summary>
    public string[] OrdinalTensMap { get; } = ordinalTensMap;
    /// <summary>Gets the cardinal hundreds lexicon.</summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>Gets ordinal overrides for units that do not take the regular suffix.</summary>
    public FrozenDictionary<int, string> OrdinalUnitsExceptions { get; } = ordinalUnitsExceptions;
    /// <summary>Gets cardinal overrides for whole tens that do not use the regular tens lexicon.</summary>
    public FrozenDictionary<int, string> WholeTensExceptions { get; } = wholeTensExceptions;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public HyphenatedScale[] Scales { get; } = scales;
    /// <summary>Gets the one-thousand row used by locales that special-case 1,000-1,999.</summary>
    public HyphenatedScale ThousandScale { get; } = thousandScale;
    /// <summary>Gets named tuple overrides for small integers.</summary>
    public FrozenDictionary<int, string> TupleMap { get; } = tupleMap;
}

/// <summary>
/// One descending scale row for <see cref="HyphenatedScaleNumberToWordsConverter"/>.
/// </summary>
/// <param name="Divisor">The divisor for the scale row.</param>
/// <param name="Cardinal">The cardinal form of the scale row.</param>
/// <param name="Ordinal">The ordinal form of the scale row.</param>
readonly record struct HyphenatedScale(long Divisor, string Cardinal, string Ordinal);