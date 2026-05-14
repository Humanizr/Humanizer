namespace Humanizer;

/// <summary>
/// Parses number words produced by stemmed scale profiles.
/// </summary>
internal class StemmedScaleWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    readonly StemmedScaleWordsToNumberProfile profile;
    readonly FrozenDictionary<string, long> tokenValues;
    readonly TokenizedStemmedScaleWord[] tokenizedValues;
    readonly TokenizedStemmedScaleWord[] fallbackScales;

    public StemmedScaleWordsToNumberConverter(StemmedScaleWordsToNumberProfile profile)
    {
        this.profile = profile;
        tokenValues = BuildTokenValues(profile);
        tokenizedValues = Tokenize(tokenValues);
        fallbackScales = Tokenize(BuildFallbackScaleValues(profile));
    }

    /// <inheritdoc />
    public override long Convert(string words)
    {
        if (!TryConvert(words, out var result, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return result;
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

        var source = words.Trim();
        var negative = false;
        foreach (var prefix in profile.NegativePrefixes)
        {
            if (!source.StartsWith(prefix, StringComparison.Ordinal))
            {
                continue;
            }

            negative = true;
            source = source[prefix.Length..].Trim();
            break;
        }

        if (TryParseOrdinal(source, out var ordinalValue))
        {
            parsedValue = negative ? -ordinalValue : ordinalValue;
            unrecognizedWord = null;
            return true;
        }

        if (tokenValues.TryGetValue(source, out var exactValue))
        {
            parsedValue = negative ? -exactValue : exactValue;
            unrecognizedWord = null;
            return true;
        }

        var sourceTokens = SplitWords(source);
        if (!TryParseTokenSequence(sourceTokens, out var magnitude, out unrecognizedWord))
        {
            parsedValue = default;
            return false;
        }

        return TryConvertMagnitude(magnitude, negative, source, out parsedValue, out unrecognizedWord);
    }

    bool TryParseOrdinal(string source, out long value)
    {
        if (profile.OrdinalMap.TryGetValue(source, out value))
        {
            return true;
        }

        foreach (var suffix in profile.OrdinalSuffixes)
        {
            if (!source.EndsWith(suffix, StringComparison.Ordinal))
            {
                continue;
            }

            var cardinal = source[..^suffix.Length].Trim();
            if (cardinal.Length != 0 && TryConvert(cardinal, out value, out _))
            {
                return true;
            }
        }

        value = default;
        return false;
    }

    static FrozenDictionary<string, long> BuildTokenValues(StemmedScaleWordsToNumberProfile profile)
    {
        var values = new Dictionary<string, long>(StringComparer.Ordinal);

        for (var i = 0; i < profile.Words.Length; i++)
        {
            Add(values, profile.Words[i], i);
        }

        foreach (var scale in profile.Scales)
        {
            Add(values, scale.One, scale.Value);
            Add(values, scale.OneWithRemainder, scale.Value);

            for (var count = 2; count < profile.CountStems.Length; count++)
            {
                var stem = profile.CountStems[count];
                if (string.IsNullOrEmpty(stem))
                {
                    continue;
                }

                Add(values, stem + scale.StemJoiner + scale.Suffix, checked(count * scale.Value));
                Add(values, stem + scale.StemJoiner + scale.SuffixWithRemainder, checked(count * scale.Value));
            }
        }

        foreach (var entry in profile.AdditionalCardinals)
        {
            Add(values, entry.Key, entry.Value);
        }

        return values.ToFrozenDictionary(StringComparer.Ordinal);
    }

    static FrozenDictionary<string, long> BuildFallbackScaleValues(StemmedScaleWordsToNumberProfile profile)
    {
        var values = new Dictionary<string, long>(StringComparer.Ordinal);

        foreach (var scale in profile.Scales)
        {
            Add(values, scale.FallbackName, scale.Value);
            Add(values, scale.FallbackNameWithRemainder, scale.Value);
        }

        return values.ToFrozenDictionary(StringComparer.Ordinal);
    }

    static TokenizedStemmedScaleWord[] Tokenize(FrozenDictionary<string, long> values) =>
        values
            .Select(static pair => new TokenizedStemmedScaleWord(
                SplitWords(pair.Key),
                checked((ulong)pair.Value)))
            .OrderByDescending(static pair => pair.Tokens.Length)
            .ThenByDescending(static pair => pair.Tokens.Sum(static token => token.Length))
            .ToArray();

    static bool TryConvertMagnitude(
        ulong magnitude,
        bool negative,
        string source,
        out long parsedValue,
        out string? unrecognizedWord)
    {
        if (negative)
        {
            var longMinMagnitude = (ulong)long.MaxValue + 1UL;
            if (magnitude == longMinMagnitude)
            {
                parsedValue = long.MinValue;
                unrecognizedWord = null;
                return true;
            }

            if (magnitude <= (ulong)long.MaxValue)
            {
                parsedValue = -(long)magnitude;
                unrecognizedWord = null;
                return true;
            }
        }
        else if (magnitude <= (ulong)long.MaxValue)
        {
            parsedValue = (long)magnitude;
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        unrecognizedWord = source;
        return false;
    }

    bool TryParseTokenSequence(string[] sourceTokens, out ulong value, out string? unrecognizedWord) =>
        TryParseTokenSequence(sourceTokens, 0, sourceTokens.Length, out value, out unrecognizedWord);

    bool TryParseTokenSequence(
        string[] sourceTokens,
        int startIndex,
        int endIndex,
        out ulong value,
        out string? unrecognizedWord)
    {
        for (var index = endIndex - 1; index >= startIndex; index--)
        {
            if (!TryMatchFallbackScale(sourceTokens, index, endIndex, out var scaleValue, out var scaleTokens))
            {
                continue;
            }

            var count = 1UL;
            if (index != startIndex &&
                !TryParseTokenSequence(sourceTokens, startIndex, index, out count, out _))
            {
                continue;
            }

            var remainder = 0UL;
            var remainderStart = index + scaleTokens;
            if (remainderStart < endIndex &&
                (!TryParseTokenSequence(sourceTokens, remainderStart, endIndex, out remainder, out _) ||
                 remainder >= scaleValue))
            {
                continue;
            }

            try
            {
                value = checked(checked(count * scaleValue) + remainder);
            }
            catch (OverflowException)
            {
                value = default;
                unrecognizedWord = string.Join(" ", sourceTokens.Skip(startIndex).Take(endIndex - startIndex));
                return false;
            }

            unrecognizedWord = null;
            return true;
        }

        return TryParseAdditiveRange(sourceTokens, startIndex, endIndex, out value, out unrecognizedWord);
    }

    bool TryParseAdditiveRange(
        string[] sourceTokens,
        int startIndex,
        int endIndex,
        out ulong value,
        out string? unrecognizedWord)
    {
        var total = 0UL;

        for (var index = startIndex; index < endIndex;)
        {
            if (!TryMatchToken(sourceTokens, index, endIndex, out var tokenValue, out var consumed))
            {
                value = default;
                unrecognizedWord = sourceTokens[index];
                return false;
            }

            try
            {
                total = checked(total + tokenValue);
            }
            catch (OverflowException)
            {
                value = default;
                unrecognizedWord = string.Join(" ", sourceTokens);
                return false;
            }

            index += consumed;
        }

        value = total;
        unrecognizedWord = null;
        return true;
    }

    bool TryMatchFallbackScale(string[] sourceTokens, int index, int endIndex, out ulong value, out int consumed) =>
        TryMatchToken(fallbackScales, sourceTokens, index, endIndex, out value, out consumed);

    bool TryMatchToken(string[] sourceTokens, int index, int endIndex, out ulong value, out int consumed) =>
        TryMatchToken(tokenizedValues, sourceTokens, index, endIndex, out value, out consumed);

    static bool TryMatchToken(
        TokenizedStemmedScaleWord[] candidates,
        string[] sourceTokens,
        int index,
        int endIndex,
        out ulong value,
        out int consumed)
    {
        foreach (var candidate in candidates)
        {
            if (index + candidate.Tokens.Length > endIndex)
            {
                continue;
            }

            var matched = true;
            for (var offset = 0; offset < candidate.Tokens.Length; offset++)
            {
                if (!string.Equals(sourceTokens[index + offset], candidate.Tokens[offset], StringComparison.Ordinal))
                {
                    matched = false;
                    break;
                }
            }

            if (!matched)
            {
                continue;
            }

            value = candidate.Value;
            consumed = candidate.Tokens.Length;
            return true;
        }

        value = default;
        consumed = default;
        return false;
    }

    static void Add(Dictionary<string, long> values, string key, long value)
    {
        if (!string.IsNullOrWhiteSpace(key))
        {
            values[key] = value;
        }
    }

    static string[] SplitWords(string value) =>
        value
            .Split([' '], StringSplitOptions.RemoveEmptyEntries)
            .Select(static token => token.Trim())
            .Where(static token => token.Length != 0)
            .ToArray();
}

readonly record struct TokenizedStemmedScaleWord(string[] Tokens, ulong Value);

/// <summary>
/// Generated data for <see cref="StemmedScaleWordsToNumberConverter"/>.
/// </summary>
internal sealed class StemmedScaleWordsToNumberProfile(
    string[] words,
    string[] countStems,
    StemmedScale[] scales,
    string[] negativePrefixes,
    string[] ordinalSuffixes,
    FrozenDictionary<string, long>? ordinalMap = null,
    FrozenDictionary<string, long>? additionalCardinals = null)
{
    /// <summary>Gets exact low-number words.</summary>
    public string[] Words { get; } = words;
    /// <summary>Gets bound count stems used before scale suffixes.</summary>
    public string[] CountStems { get; } = countStems;
    /// <summary>Gets the scale rows shared with the writer profile.</summary>
    public StemmedScale[] Scales { get; } = scales;
    /// <summary>Gets negative prefixes.</summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
    /// <summary>Gets suffixes that mark word ordinals.</summary>
    public string[] OrdinalSuffixes { get; } = ordinalSuffixes;
    /// <summary>Gets exact ordinal words.</summary>
    public FrozenDictionary<string, long> OrdinalMap { get; } = ordinalMap ?? FrozenDictionary<string, long>.Empty;
    /// <summary>Gets additional exact cardinal tokens not generated from the stemmed scale rows.</summary>
    public FrozenDictionary<string, long> AdditionalCardinals { get; } = additionalCardinals ?? FrozenDictionary<string, long>.Empty;
}