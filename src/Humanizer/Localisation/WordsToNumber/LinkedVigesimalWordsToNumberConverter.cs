namespace Humanizer;

/// <summary>
/// Parses linked-vigesimal words with scale nouns before their count words.
/// </summary>
internal class LinkedVigesimalWordsToNumberConverter(LinkedVigesimalWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly LinkedVigesimalWordsToNumberProfile profile = profile;

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
            parsedValue = default;
            unrecognizedWord = words ?? string.Empty;
            return false;
        }

        var normalized = Normalize(words);
        var negative = false;
        foreach (var negativePrefix in profile.NegativePrefixes)
        {
            if (normalized == negativePrefix)
            {
                parsedValue = default;
                unrecognizedWord = words;
                return false;
            }

            if (normalized.StartsWith(negativePrefix + " ", StringComparison.Ordinal))
            {
                negative = true;
                normalized = normalized[(negativePrefix.Length + 1)..];
                break;
            }
        }

        if (TryParseOrdinalCandidate(normalized, out var ordinalValue))
        {
            parsedValue = negative ? -ordinalValue : ordinalValue;
            unrecognizedWord = null;
            return true;
        }

        foreach (var ordinalSuffix in profile.OrdinalSuffixes)
        {
            if (ordinalSuffix.Length > 0 && normalized.EndsWith(ordinalSuffix, StringComparison.Ordinal))
            {
                normalized = normalized[..^ordinalSuffix.Length].TrimEnd();
                break;
            }
        }

        var tokens = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0)
        {
            parsedValue = default;
            unrecognizedWord = words;
            return false;
        }

        var maxValue = negative ? (ulong)long.MaxValue + 1UL : long.MaxValue;
        if (TryParseValue(tokens, 0, tokens.Length, maxValue, out var parsedMagnitude, out var next) &&
            next == tokens.Length &&
            TryCoerceParsedValue(parsedMagnitude, negative, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        unrecognizedWord = GetUnrecognizedWord(tokens, next, words);
        return false;
    }

    bool TryParseValue(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        value = 0;
        next = start;
        var consumed = false;

        while (next < end)
        {
            if (TryMatchAnyTokenPhrase(tokens, next, end, profile.TerminalRemainderJoinerTokenPhrases, out var afterJoiner))
            {
                next = afterJoiner;
                continue;
            }

            if (TryParseScale(tokens, next, end, maxValue - value, out var scaleValue, out var afterScale))
            {
                value += scaleValue;
                next = afterScale;
                consumed = true;
                continue;
            }

            if (TryParseExact(tokens, next, end, maxValue - value, out var exactValue, out var afterExact))
            {
                value += exactValue;
                next = afterExact;
                consumed = true;
                continue;
            }

            break;
        }

        return consumed;
    }

    bool TryParseScale(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        for (var scaleIndex = 0; scaleIndex < profile.TokenizedScales.Length; scaleIndex++)
        {
            var scale = profile.TokenizedScales[scaleIndex];
            var scaleValue = (ulong)scale.Value;
            if (scaleValue > maxValue)
            {
                continue;
            }

            var maxCount = Math.Min(GetMaximumCountForScale(scaleIndex), maxValue / scaleValue);

            if (TryMatchTokenPhrase(tokens, start, end, scale.OneTokens, out next) ||
                TryMatchTokenPhrase(tokens, start, end, scale.OneWithRemainderTokens, out next))
            {
                value = scaleValue;
                return true;
            }

            if ((TryMatchTokenPhrase(tokens, start, end, scale.NameTokens, out var afterName) ||
                    TryMatchTokenPhrase(tokens, start, end, scale.NameWithRemainderTokens, out afterName)) &&
                TryParseScaleCount(tokens, afterName, end, maxCount, scaleValue, out var count, out next) &&
                count > 0)
            {
                value = count * scaleValue;
                return true;
            }
        }

        value = default;
        next = start;
        return false;
    }

    bool TryParseScaleCount(string[] tokens, int start, int end, ulong maxValue, ulong scaleValue, out ulong value, out int next)
    {
        if (!TryParseExact(tokens, start, end, maxValue, out var firstValue, out var afterFirst) || firstValue == 0)
        {
            value = default;
            next = start;
            return false;
        }

        var countEnd = FindScaleCountEnd(tokens, afterFirst, end, scaleValue);
        if (firstValue >= (ulong)profile.ScaleCountCompositeThreshold && countEnd > afterFirst)
        {
            for (var candidateEnd = countEnd; candidateEnd > afterFirst; candidateEnd--)
            {
                if (TryParseValue(tokens, start, candidateEnd, maxValue, out var compositeValue, out var afterComposite) &&
                    afterComposite == candidateEnd)
                {
                    value = compositeValue;
                    next = afterComposite;
                    return true;
                }
            }
        }

        value = firstValue;
        next = afterFirst;
        return true;
    }

    ulong GetMaximumCountForScale(int scaleIndex)
    {
        if (scaleIndex == 0)
        {
            return ulong.MaxValue / (ulong)profile.TokenizedScales[scaleIndex].Value;
        }

        var previous = (ulong)profile.TokenizedScales[scaleIndex - 1].Value;
        var current = (ulong)profile.TokenizedScales[scaleIndex].Value;
        if (previous <= current || previous % current != 0)
        {
            throw new InvalidOperationException("Linked-vigesimal parser profiles require descending scales where each larger scale is divisible by the next smaller scale.");
        }

        return previous / current - 1UL;
    }

    int FindScaleCountEnd(string[] tokens, int start, int end, ulong parentScaleValue)
    {
        for (var index = start; index < end; index++)
        {
            foreach (var scale in profile.TokenizedScales.Where(scale => (ulong)scale.Value < parentScaleValue))
            {
                if (TryMatchTokenPhrase(tokens, index, end, scale.NameTokens, out _) ||
                    TryMatchTokenPhrase(tokens, index, end, scale.NameWithRemainderTokens, out _))
                {
                    return index;
                }
            }
        }

        return end;
    }

    bool TryParseExact(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        var lastCandidateEnd = Math.Min(end, start + profile.MaximumExactTokenCount);
        foreach (var candidate in profile.MultiTokenExactWords.Where(candidate => start + candidate.Tokens.Length <= lastCandidateEnd))
        {
            if (candidate.Value >= 0 &&
                (ulong)candidate.Value <= maxValue &&
                TryMatchTokenPhrase(tokens, start, end, candidate.Tokens, out next))
            {
                value = (ulong)candidate.Value;
                return true;
            }
        }

        if (start < end &&
            profile.ExactWords.TryGetValue(tokens[start], out var rawValue) &&
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

    static bool TryMatchAnyTokenPhrase(string[] tokens, int start, int end, string[][] phraseTokenSets, out int next)
    {
        for (var index = 0; index < phraseTokenSets.Length; index++)
        {
            var phraseTokens = phraseTokenSets[index];
            if (phraseTokens.Length > 0 && TryMatchTokenPhrase(tokens, start, end, phraseTokens, out next))
            {
                return true;
            }
        }

        next = start;
        return false;
    }

    static bool TryMatchTokenPhrase(string[] tokens, int start, int end, string[] phraseTokens, out int next)
    {
        if (phraseTokens.Length == 0)
        {
            next = start;
            return false;
        }

        if (start + phraseTokens.Length > end)
        {
            next = start;
            return false;
        }

        for (var i = 0; i < phraseTokens.Length; i++)
        {
            if (tokens[start + i] != phraseTokens[i])
            {
                next = start;
                return false;
            }
        }

        next = start + phraseTokens.Length;
        return true;
    }

    bool TryParseOrdinalCandidate(string candidate, out long parsedValue)
    {
        if (profile.OrdinalMap.TryGetValue(candidate, out parsedValue))
        {
            return true;
        }

        parsedValue = default;
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

    string GetUnrecognizedWord(string[] tokens, int next, string words)
    {
        if (next > 0 && next < tokens.Length)
        {
            return tokens[next];
        }

        foreach (var token in tokens)
        {
            if (!profile.ExactWords.ContainsKey(token) &&
                !profile.OrdinalMap.ContainsKey(token) &&
                !profile.NegativePrefixes.Contains(token) &&
                !profile.TerminalRemainderJoinerTokens.Contains(token))
            {
                return token;
            }
        }

        return words;
    }

    static string Normalize(string value) =>
        string.Join(" ", value.Trim().ToLowerInvariant().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}

/// <summary>
/// Immutable generated profile for <see cref="LinkedVigesimalWordsToNumberConverter"/>.
/// </summary>
internal sealed class LinkedVigesimalWordsToNumberProfile
{
    /// <summary>Initializes a new generated linked-vigesimal parser profile.</summary>
    public LinkedVigesimalWordsToNumberProfile(
        string[] words,
        LinkedVigesimalScale[] scales,
        string terminalRemainderJoiner,
        string[]? negativePrefixes = null,
        string[]? ordinalSuffixes = null,
        FrozenDictionary<string, long>? ordinalMap = null,
        FrozenDictionary<string, long>? additionalCardinals = null,
        string terminalRemainderAlternateJoiner = "",
        long scaleCountCompositeThreshold = 100)
    {
        ExactWords = BuildExactWords(words, scales, additionalCardinals);
        MultiTokenExactWords = TokenizeNumberWords(ExactWords);
        MaximumExactTokenCount = GetMaximumTokenCount(ExactWords.Keys);
        Scales = ValidateScales(scales);
        TokenizedScales = TokenizeScales(Scales);
        TerminalRemainderJoinerTokenPhrases = TokenizeTerminalRemainderJoiners(terminalRemainderJoiner, terminalRemainderAlternateJoiner);
        TerminalRemainderJoinerTokens = TerminalRemainderJoinerTokenPhrases
            .SelectMany(static tokens => tokens)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        NegativePrefixes = negativePrefixes ?? [];
        OrdinalSuffixes = ordinalSuffixes ?? [];
        OrdinalMap = ordinalMap ?? FrozenDictionary<string, long>.Empty;
        ScaleCountCompositeThreshold = scaleCountCompositeThreshold;
    }

    /// <summary>Gets exact cardinal words.</summary>
    public FrozenDictionary<string, long> ExactWords { get; }
    /// <summary>Gets multi-token exact cardinal words, longest phrases first.</summary>
    public TokenizedNumberWord[] MultiTokenExactWords { get; }
    /// <summary>Gets maximum token count for exact words.</summary>
    public int MaximumExactTokenCount { get; }
    /// <summary>Gets descending scale rows.</summary>
    public LinkedVigesimalScale[] Scales { get; }
    /// <summary>Gets tokenized descending scale rows.</summary>
    public TokenizedLinkedVigesimalScale[] TokenizedScales { get; }
    /// <summary>Gets pre-tokenized terminal remainder linker phrases.</summary>
    public string[][] TerminalRemainderJoinerTokenPhrases { get; }
    /// <summary>Gets distinct tokens from terminal remainder linkers.</summary>
    public string[] TerminalRemainderJoinerTokens { get; }
    /// <summary>Gets negative prefixes.</summary>
    public string[] NegativePrefixes { get; }
    /// <summary>Gets ordinal suffixes.</summary>
    public string[] OrdinalSuffixes { get; }
    /// <summary>Gets exact ordinal tokens.</summary>
    public FrozenDictionary<string, long> OrdinalMap { get; }
    /// <summary>Gets the minimum exact count value that may be followed by a linked low remainder inside a scale count.</summary>
    public long ScaleCountCompositeThreshold { get; }

    static FrozenDictionary<string, long> BuildExactWords(string[] words, LinkedVigesimalScale[] scales, FrozenDictionary<string, long>? additionalCardinals)
    {
        var values = new Dictionary<string, long>();
        for (var i = 0; i < words.Length; i++)
        {
            values[Normalize(words[i])] = i;
        }

        foreach (var scale in scales)
        {
            values[Normalize(scale.One)] = scale.Value;
            values[Normalize(scale.OneWithRemainder)] = scale.Value;
            foreach (var pair in scale.CountOverrides.Concat(scale.CountOverridesWithRemainder))
            {
                values[Normalize(pair.Value)] = pair.Key * scale.Value;
            }
        }

        if (additionalCardinals is not null)
        {
            foreach (var pair in additionalCardinals)
            {
                values[Normalize(pair.Key)] = pair.Value;
            }
        }

        return values.ToFrozenDictionary();
    }

    static TokenizedNumberWord[] TokenizeNumberWords(FrozenDictionary<string, long> values) =>
        values
            .Select(pair => new TokenizedNumberWord(Tokenize(pair.Key), pair.Value))
            .Where(word => word.Tokens.Length > 1)
            .OrderByDescending(word => word.Tokens.Length)
            .ToArray();

    static TokenizedLinkedVigesimalScale[] TokenizeScales(LinkedVigesimalScale[] value) =>
        value.Select(scale => new TokenizedLinkedVigesimalScale(
            scale.Value,
            Tokenize(scale.One),
            Tokenize(scale.OneWithRemainder),
            Tokenize(scale.Name),
            Tokenize(scale.NameWithRemainder))).ToArray();

    static int GetMaximumTokenCount(IEnumerable<string> values)
    {
        var maximum = 1;
        foreach (var value in values)
        {
            maximum = Math.Max(maximum, Tokenize(value).Length);
        }

        return maximum;
    }

    static string[][] TokenizeTerminalRemainderJoiners(string terminalRemainderJoiner, string terminalRemainderAlternateJoiner) =>
        new[] { terminalRemainderJoiner, terminalRemainderAlternateJoiner }
            .Select(Tokenize)
            .Where(static tokens => tokens.Length > 0)
            .OrderByDescending(static tokens => tokens.Length)
            .ToArray();

    static string[] Tokenize(string value) =>
        value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    static string Normalize(string value) =>
        string.Join(" ", value.Trim().ToLowerInvariant().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    static LinkedVigesimalScale[] ValidateScales(LinkedVigesimalScale[] value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i].Value <= 0)
            {
                throw new InvalidOperationException("Linked-vigesimal parser profiles require positive scale values.");
            }

            if (i > 0 && value[i - 1].Value <= value[i].Value)
            {
                throw new InvalidOperationException("Linked-vigesimal parser profiles require descending scales.");
            }
        }

        return value;
    }
}

/// <summary>Pre-tokenized linked-vigesimal scale phrase.</summary>
readonly record struct TokenizedLinkedVigesimalScale(long Value, string[] OneTokens, string[] OneWithRemainderTokens, string[] NameTokens, string[] NameWithRemainderTokens);