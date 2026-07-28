namespace Humanizer;

/// <summary>
/// Renders decimal scale systems where the count before a scale uses a bound stem and the scale
/// suffix can change when a lower-order remainder follows.
/// </summary>
/// <remarks>
/// Sinhala uses this pattern for forms such as "දෙසීය"/"දෙසිය එක" and
/// "දෙදාහ"/"දෙදහස් එක". The profile keeps those stems and suffixes in generated locale data so
/// the runtime algorithm remains reusable.
/// </remarks>
class StemmedScaleNumberToWordsConverter(StemmedScaleNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly StemmedScaleNumberToWordsProfile profile = profile;

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
            return profile.NegativeWord + profile.NegativeJoiner + ConvertPositive(magnitude);
        }

        return ConvertPositive((ulong)number);
    }

    /// <inheritdoc />
    public override string ConvertToOrdinal(int number)
    {
        if (profile.OrdinalExceptions.TryGetValue(number, out var exactOrdinal))
        {
            return exactOrdinal;
        }

        if (number < 0)
        {
            var magnitude = number == int.MinValue
                ? (ulong)int.MaxValue + 1UL
                : (ulong)-number;
            return profile.NegativeWord + profile.NegativeJoiner + ConvertPositiveToOrdinal(magnitude);
        }

        return ConvertPositiveToOrdinal((ulong)number);
    }

    string ConvertPositiveToOrdinal(ulong number)
    {
        if (number <= int.MaxValue && profile.OrdinalExceptions.TryGetValue((int)number, out var exactOrdinal))
        {
            return exactOrdinal;
        }

        return ConvertPositive(number) + profile.OrdinalSuffix;
    }

    string ConvertPositive(ulong number)
    {
        if (number < (ulong)profile.Words.Length)
        {
            return profile.Words[(int)number];
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
            parts.Add(FormatScale(count, scale, remainder > 0));
        }

        if (remainder > 0)
        {
            parts.Add(ConvertPositive(remainder));
        }

        if (parts.Count == 0)
        {
            throw new InvalidOperationException("Stemmed scale profiles require a scale value no greater than the dense word table range.");
        }

        return string.Join(profile.Joiner, parts);
    }

    string FormatScale(ulong count, StemmedScale scale, bool hasRemainder)
    {
        if (count == 1)
        {
            return hasRemainder ? scale.OneWithRemainder : scale.One;
        }

        var suffix = hasRemainder ? scale.SuffixWithRemainder : scale.Suffix;
        if (count < (ulong)profile.CountStems.Length && !string.IsNullOrEmpty(profile.CountStems[(int)count]))
        {
            return profile.CountStems[(int)count] + scale.StemJoiner + suffix;
        }

        var fallbackName = hasRemainder ? scale.FallbackNameWithRemainder : scale.FallbackName;
        return ConvertPositive(count) + scale.FallbackJoiner + fallbackName;
    }
}

/// <summary>
/// Generated data for <see cref="StemmedScaleNumberToWordsConverter"/>.
/// </summary>
sealed class StemmedScaleNumberToWordsProfile(
    string zeroWord,
    string negativeWord,
    string negativeJoiner,
    string joiner,
    string ordinalSuffix,
    string[] words,
    string[] countStems,
    StemmedScale[] scales,
    FrozenDictionary<int, string>? ordinalExceptions = null)
{
    /// <summary>Gets the zero word.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the negative prefix word.</summary>
    public string NegativeWord { get; } = negativeWord;
    /// <summary>Gets the joiner between the negative word and the positive phrase.</summary>
    public string NegativeJoiner { get; } = negativeJoiner;
    /// <summary>Gets the joiner between scale parts.</summary>
    public string Joiner { get; } = joiner;
    /// <summary>Gets the suffix appended to non-exception word ordinals.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets exact cardinal words covered by the dense table.</summary>
    public string[] Words { get; } = ValidateWords(words);
    /// <summary>Gets bound count stems used before scale suffixes.</summary>
    public string[] CountStems { get; } = countStems;
    /// <summary>Gets descending scale rows.</summary>
    public StemmedScale[] Scales { get; } = ValidateScales(scales, words.Length);
    /// <summary>Gets exact word ordinal exceptions.</summary>
    public FrozenDictionary<int, string> OrdinalExceptions { get; } = ordinalExceptions ?? FrozenDictionary<int, string>.Empty;

    static string[] ValidateWords(string[] value)
    {
        if (value.Length == 0)
        {
            throw new InvalidOperationException("Stemmed scale profiles require at least one word entry.");
        }

        return value;
    }

    static StemmedScale[] ValidateScales(StemmedScale[] value, int denseWordCount)
    {
        if (value.Length == 0 || value[^1].Value > denseWordCount)
        {
            throw new InvalidOperationException("Stemmed scale profiles require a lowest scale value covered by the dense word table.");
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (value[i].Value <= 0)
            {
                throw new InvalidOperationException("Stemmed scale profiles require positive scale values.");
            }

            if (i > 0 && value[i - 1].Value <= value[i].Value)
            {
                throw new InvalidOperationException("Stemmed scale profiles require scales in descending order.");
            }
        }

        return value;
    }
}

/// <summary>
/// One scale row for stemmed scale number rendering.
/// </summary>
readonly record struct StemmedScale(
    long Value,
    string One,
    string OneWithRemainder,
    string Suffix,
    string SuffixWithRemainder,
    string StemJoiner,
    string FallbackName,
    string FallbackNameWithRemainder,
    string FallbackJoiner);