namespace Humanizer;

/// <summary>
/// Parses decimal number words whose scale nouns precede their count words.
/// </summary>
internal class ScaleLeadingCompoundWordsToNumberConverter(ScaleLeadingCompoundWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly ScaleLeadingCompoundWordsToNumberProfile profile = profile;

    /// <inheritdoc />
    public override long Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    /// <inheritdoc />
    public override bool TryConvert(string words, out long parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    /// <inheritdoc />
    public override bool TryConvert(string words, out long parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        var normalized = Normalize(words);
        var negative = false;
        if (normalized.StartsWith(profile.MinusWord + " ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized[(profile.MinusWord.Length + 1)..];
        }

        if (profile.OrdinalMap.TryGetValue(normalized, out var ordinalValue))
        {
            parsedValue = negative ? -ordinalValue : ordinalValue;
            unrecognizedWord = null;
            return true;
        }

        if (profile.OrdinalPrefix.Length > 0 && normalized.StartsWith(profile.OrdinalPrefix, StringComparison.Ordinal))
        {
            normalized = normalized[profile.OrdinalPrefix.Length..].TrimStart();
        }

        if (profile.OrdinalSuffix.Length > 0 && normalized.EndsWith(profile.OrdinalSuffix, StringComparison.Ordinal))
        {
            normalized = normalized[..^profile.OrdinalSuffix.Length].TrimEnd();
        }

        var tokens = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0)
        {
            parsedValue = default;
            unrecognizedWord = words;
            return false;
        }

        try
        {
            var maxValue = negative ? (ulong)long.MaxValue + 1UL : long.MaxValue;
            if (TryParseValue(tokens, 0, tokens.Length, maxValue, out var value, out var next) &&
                next == tokens.Length &&
                TryCoerceParsedValue(value, negative, out parsedValue))
            {
                unrecognizedWord = null;
                return true;
            }
        }
        catch (OverflowException)
        {
        }

        parsedValue = default;
        unrecognizedWord = tokens[0];
        return false;
    }

    bool TryParseValue(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        value = 0;
        next = start;
        var consumed = false;

        for (var scaleIndex = 0; scaleIndex < profile.Scales.Length; scaleIndex++)
        {
            var scale = profile.Scales[scaleIndex];
            var scaleValue = (ulong)scale.Value;
            if (scaleValue > maxValue || next >= end || tokens[next] != scale.Name)
            {
                continue;
            }

            var maxCount = Math.Min(GetMaximumCountForScale(scaleIndex), maxValue / scaleValue);
            if (maxCount <= 0 || !TryParseCount(tokens, next + 1, end, maxCount, out var count, out var afterCount) || count <= 0)
            {
                return false;
            }

            value = checked(value + checked(count * scaleValue));
            next = afterCount;
            consumed = true;
        }

        if (next < end)
        {
            var remainderStart = next;
            if (consumed && tokens[remainderStart] == profile.ConjunctionWord)
            {
                remainderStart++;
            }

            var remainingMax = maxValue == ulong.MaxValue ? ulong.MaxValue : maxValue - value;
            if (TryParseUnderOneHundred(tokens, remainderStart, end, Math.Min(remainingMax, 99), out var remainder, out var afterRemainder))
            {
                value = checked(value + remainder);
                next = afterRemainder;
                consumed = true;
            }
        }

        return consumed;
    }

    bool TryParseCount(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        if (maxValue <= 9)
        {
            return TryParseUnit(tokens, start, end, maxValue, out value, out next);
        }

        if (maxValue < 100)
        {
            return TryParseUnderOneHundred(tokens, start, end, maxValue, out value, out next);
        }

        return TryParseValue(tokens, start, end, maxValue, out value, out next);
    }

    bool TryParseUnderOneHundred(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        if (TryParseUnit(tokens, start, end, maxValue, out value, out next))
        {
            return true;
        }

        if (start >= end || !profile.Tens.TryGetValue(tokens[start], out var rawTensValue) || rawTensValue < 0 || (ulong)rawTensValue > maxValue)
        {
            value = default;
            next = start;
            return false;
        }

        value = (ulong)rawTensValue;
        next = start + 1;
        if (next + 1 < end && tokens[next] == profile.ConjunctionWord && profile.Units.TryGetValue(tokens[next + 1], out var rawUnitValue) && rawUnitValue is >= 1 and <= 9 && value + (ulong)rawUnitValue <= maxValue)
        {
            value += (ulong)rawUnitValue;
            next += 2;
        }

        return true;
    }

    bool TryParseUnit(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        if (start < end &&
            profile.Units.TryGetValue(tokens[start], out var rawValue) &&
            rawValue >= 0 &&
            (ulong)rawValue <= maxValue)
        {
            value = (ulong)rawValue;
            next = start + 1;
            return true;
        }

        value = default;
        next = start;
        return false;
    }

    static bool TryCoerceParsedValue(ulong value, bool negative, out long parsedValue)
    {
        if (negative)
        {
            if (value == (ulong)long.MaxValue + 1UL)
            {
                parsedValue = long.MinValue;
                return true;
            }

            if (value <= long.MaxValue)
            {
                parsedValue = -(long)value;
                return true;
            }
        }
        else if (value <= long.MaxValue)
        {
            parsedValue = (long)value;
            return true;
        }

        parsedValue = default;
        return false;
    }

    ulong GetMaximumCountForScale(int scaleIndex)
    {
        if (scaleIndex == 0)
        {
            return ulong.MaxValue / (ulong)profile.Scales[scaleIndex].Value;
        }

        return (ulong)(profile.Scales[scaleIndex - 1].Value / profile.Scales[scaleIndex].Value - 1);
    }

    static string Normalize(string value) =>
        string.Join(" ", value.Trim().ToLowerInvariant().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}

/// <summary>
/// Immutable generated profile for <see cref="ScaleLeadingCompoundWordsToNumberConverter"/>.
/// </summary>
internal sealed class ScaleLeadingCompoundWordsToNumberProfile(
    FrozenDictionary<string, long> units,
    FrozenDictionary<string, long> tens,
    ScaleLeadingCompoundScale[] scales,
    string conjunctionWord,
    string minusWord,
    string ordinalPrefix,
    string ordinalSuffix,
    FrozenDictionary<string, long>? ordinalMap = null)
{
    /// <summary>Gets unit and teen tokens.</summary>
    public FrozenDictionary<string, long> Units { get; } = units;
    /// <summary>Gets decade tokens.</summary>
    public FrozenDictionary<string, long> Tens { get; } = tens;
    /// <summary>Gets descending scale rows.</summary>
    public ScaleLeadingCompoundScale[] Scales { get; } = scales;
    /// <summary>Gets the conjunction token.</summary>
    public string ConjunctionWord { get; } = conjunctionWord;
    /// <summary>Gets the negative prefix token.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the ordinal prefix token.</summary>
    public string OrdinalPrefix { get; } = ordinalPrefix;
    /// <summary>Gets the ordinal suffix token.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets exact ordinal tokens.</summary>
    public FrozenDictionary<string, long> OrdinalMap { get; } = ordinalMap ?? FrozenDictionary<string, long>.Empty;
}