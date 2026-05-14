namespace Humanizer;

/// <summary>
/// Parses number words produced by stemmed scale profiles.
/// </summary>
internal class StemmedScaleWordsToNumberConverter(StemmedScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly StemmedScaleWordsToNumberProfile profile = profile;
    readonly FrozenDictionary<string, long> tokenValues = BuildTokenValues(profile);
    readonly TokenizedStemmedScaleWord[] tokenizedValues = Tokenize(BuildTokenValues(profile));

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

        long total = 0;
        var sourceTokens = SplitWords(source);
        for (var index = 0; index < sourceTokens.Length;)
        {
            if (!TryMatchToken(sourceTokens, index, out var value, out var consumed))
            {
                parsedValue = default;
                unrecognizedWord = sourceTokens[index];
                return false;
            }

            try
            {
                total = checked(total + value);
            }
            catch (OverflowException)
            {
                parsedValue = default;
                unrecognizedWord = source;
                return false;
            }

            index += consumed;
        }

        parsedValue = negative ? -total : total;
        unrecognizedWord = null;
        return true;
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

    static TokenizedStemmedScaleWord[] Tokenize(FrozenDictionary<string, long> values) =>
        values
            .Select(static pair => new TokenizedStemmedScaleWord(
                SplitWords(pair.Key),
                pair.Value))
            .OrderByDescending(static pair => pair.Tokens.Length)
            .ThenByDescending(static pair => pair.Tokens.Sum(static token => token.Length))
            .ToArray();

    bool TryMatchToken(string[] sourceTokens, int index, out long value, out int consumed)
    {
        foreach (var candidate in tokenizedValues)
        {
            if (index + candidate.Tokens.Length > sourceTokens.Length)
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

readonly record struct TokenizedStemmedScaleWord(string[] Tokens, long Value);

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