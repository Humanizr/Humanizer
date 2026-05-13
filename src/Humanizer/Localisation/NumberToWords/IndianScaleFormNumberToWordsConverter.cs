namespace Humanizer;

/// <summary>
/// Renders Indian-grouped number systems whose scale nouns have singular, plural, and continuing
/// forms, and whose one-count scales can be expressed without an explicit one word.
/// </summary>
class IndianScaleFormNumberToWordsConverter(IndianScaleFormNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly IndianScaleFormNumberToWordsProfile profile = profile;

    /// <inheritdoc />
    public override string Convert(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            var magnitude = number == long.MinValue
                ? (ulong)long.MaxValue + 1UL
                : (ulong)-number;
            return profile.NegativeWord + " " + ConvertPositive(magnitude);
        }

        return ConvertPositive((ulong)number);
    }

    /// <inheritdoc />
    public override string ConvertToOrdinal(int number)
    {
        if (number < 0)
        {
            var magnitude = number == int.MinValue
                ? (ulong)int.MaxValue + 1UL
                : (ulong)-number;
            return profile.NegativeWord + " " + ConvertPositiveToOrdinal(magnitude);
        }

        return ConvertPositiveToOrdinal((ulong)number);
    }

    string ConvertPositiveToOrdinal(ulong number)
    {
        if (number <= int.MaxValue && profile.OrdinalMap.TryGetValue((int)number, out var exactOrdinal))
        {
            return exactOrdinal;
        }

        var cardinal = ConvertPositive(number);
        foreach (var replacement in profile.OrdinalTerminalReplacements)
        {
            if (cardinal.EndsWith(replacement.Key, StringComparison.Ordinal) &&
                (cardinal.Length == replacement.Key.Length || cardinal[cardinal.Length - replacement.Key.Length - 1] == ' '))
            {
                return cardinal[..^replacement.Key.Length] + replacement.Value;
            }
        }

        return cardinal + profile.OrdinalSuffix;
    }

    string ConvertPositive(ulong number)
    {
        if (number < (ulong)profile.DenseUnitsMap.Length)
        {
            return profile.DenseUnitsMap[(int)number];
        }

        var parts = new List<string>();
        var remainder = number;

        foreach (var scale in profile.Scales)
        {
            var scaleValue = (ulong)scale.Value;
            var count = remainder / scaleValue;
            if (count == 0)
            {
                continue;
            }

            remainder %= scaleValue;
            parts.Add(FormatScale(scale, count, remainder > 0));
        }

        if (remainder > 0)
        {
            parts.Add(ConvertPositive(remainder));
        }

        if (parts.Count == 0)
        {
            throw new InvalidOperationException("Indian scale-form number profiles require a scale value no greater than the dense unit map range.");
        }

        return string.Join(" ", parts);
    }

    string FormatScale(IndianScaleForm scale, ulong count, bool hasRemainder)
    {
        var scaleWord = count == 1
            ? hasRemainder ? scale.SingularWithRemainder : scale.Singular
            : hasRemainder ? scale.PluralWithRemainder : scale.Plural;

        return count == 1 && scale.OmitOne
            ? scaleWord
            : ConvertPositive(count) + " " + scaleWord;
    }
}

/// <summary>
/// Immutable generated profile for <see cref="IndianScaleFormNumberToWordsConverter"/>.
/// </summary>
sealed class IndianScaleFormNumberToWordsProfile(
    string zeroWord,
    string negativeWord,
    string ordinalSuffix,
    string[] denseUnitsMap,
    IndianScaleForm[] scales,
    FrozenDictionary<int, string>? ordinalMap = null,
    FrozenDictionary<string, string>? ordinalTerminalReplacements = null)
{
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative numbers.</summary>
    public string NegativeWord { get; } = negativeWord;
    /// <summary>Gets the suffix appended when no terminal replacement applies.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets dense cardinal words keyed by value.</summary>
    public string[] DenseUnitsMap { get; } = ValidateDenseUnitsMap(denseUnitsMap);
    /// <summary>Gets descending Indian grouping scale rows.</summary>
    public IndianScaleForm[] Scales { get; } = ValidateScales(scales, denseUnitsMap.Length);
    /// <summary>Gets exact ordinal words keyed by value.</summary>
    public FrozenDictionary<int, string> OrdinalMap { get; } = ordinalMap ?? FrozenDictionary<int, string>.Empty;
    /// <summary>Gets terminal cardinal-token replacements used to form ordinals.</summary>
    public FrozenDictionary<string, string> OrdinalTerminalReplacements { get; } = ordinalTerminalReplacements ?? FrozenDictionary<string, string>.Empty;

    static string[] ValidateDenseUnitsMap(string[] value)
    {
        if (value.Length < 100)
        {
            throw new InvalidOperationException("Indian scale-form number profiles require dense words for 0 through 99.");
        }

        return value;
    }

    static IndianScaleForm[] ValidateScales(IndianScaleForm[] value, int denseUnitCount)
    {
        if (value.Length == 0 || value[^1].Value > denseUnitCount)
        {
            throw new InvalidOperationException("Indian scale-form number profiles require a scale value covered by the dense unit map range.");
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (value[i].Value <= 0)
            {
                throw new InvalidOperationException("Indian scale-form number profiles require positive scale values.");
            }

            if (i > 0 && value[i - 1].Value <= value[i].Value)
            {
                throw new InvalidOperationException("Indian scale-form number profiles require scales in descending order.");
            }
        }

        return value;
    }
}

/// <summary>
/// Describes one scale row with exact and continuing forms.
/// </summary>
/// <param name="Value">The numeric scale value.</param>
/// <param name="Singular">The singular scale form used when the phrase ends at this scale.</param>
/// <param name="SingularWithRemainder">The singular scale form used before a remainder.</param>
/// <param name="Plural">The plural scale form used when the phrase ends at this scale.</param>
/// <param name="PluralWithRemainder">The plural scale form used before a remainder.</param>
/// <param name="OmitOne">Whether a count of one is omitted before the singular scale form.</param>
readonly record struct IndianScaleForm(
    long Value,
    string Singular,
    string SingularWithRemainder,
    string Plural,
    string PluralWithRemainder,
    bool OmitOne);