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
            parsedValue = default;
            unrecognizedWord = words ?? string.Empty;
            return false;
        }

        var normalized = Normalize(words);
        var negative = false;
        if (normalized.StartsWith(profile.MinusWord + " ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized[(profile.MinusWord.Length + 1)..];
        }

        var ordinalCandidate = normalized;

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

        var maxValue = negative ? (ulong)long.MaxValue + 1UL : long.MaxValue;
        if (TryParseValue(tokens, 0, tokens.Length, maxValue, allowTerminalRemainderConjunction: true, out var value, out var next) &&
            next == tokens.Length)
        {
            if (TryCoerceParsedValue(value, negative, out parsedValue))
            {
                unrecognizedWord = null;
                return true;
            }

            parsedValue = default;
            unrecognizedWord = words;
            return false;
        }

        if (TryParseOrdinalCandidate(ordinalCandidate, normalized, negative, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        unrecognizedWord = GetUnrecognizedWord(tokens, next, words);
        return false;
    }

    bool TryParseValue(string[] tokens, int start, int end, ulong maxValue, bool allowTerminalRemainderConjunction, out ulong value, out int next)
    {
        value = 0;
        next = start;
        var consumed = false;

        for (var scaleIndex = 0; scaleIndex < profile.TokenizedScales.Length; scaleIndex++)
        {
            var scale = profile.TokenizedScales[scaleIndex];
            var scaleValue = (ulong)scale.Value;
            if (scaleValue > maxValue || next >= end || !TryMatchTokenPhrase(tokens, next, end, scale.Tokens, out var afterScale))
            {
                continue;
            }

            var maxCount = Math.Min(GetMaximumCountForScale(scaleIndex), maxValue / scaleValue);
            if (maxCount <= 0 || !TryParseCount(tokens, afterScale, end, maxCount, allowTerminalRemainderConjunction: false, out var count, out var afterCount) || count <= 0)
            {
                return false;
            }

            if (count > (maxValue - value) / scaleValue)
            {
                next = afterCount;
                return false;
            }

            value += count * scaleValue;
            next = afterCount;
            consumed = true;
        }

        if (next < end)
        {
            var remainderStart = next;
            if (allowTerminalRemainderConjunction && consumed && TryMatchTokenPhrase(tokens, remainderStart, end, profile.TerminalRemainderConjunctionTokens, out var afterTerminalConjunction))
            {
                remainderStart = afterTerminalConjunction;
            }
            else if (consumed && tokens[remainderStart] == profile.ConjunctionWord)
            {
                remainderStart++;
            }

            var remainingMax = maxValue == ulong.MaxValue ? ulong.MaxValue : maxValue - value;
            if (TryParseUnderOneHundred(tokens, remainderStart, end, Math.Min(remainingMax, 99), out var remainder, out var afterRemainder))
            {
                if (remainder > maxValue - value)
                {
                    next = afterRemainder;
                    return false;
                }

                value += remainder;
                next = afterRemainder;
                consumed = true;
            }
        }

        return consumed;
    }

    static bool TryMatchTokenPhrase(string[] tokens, int start, int end, string[] phraseTokens, out int next)
    {
        if (phraseTokens.Length == 1)
        {
            if (start < end && tokens[start] == phraseTokens[0])
            {
                next = start + 1;
                return true;
            }

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

    bool TryParseCount(string[] tokens, int start, int end, ulong maxValue, bool allowTerminalRemainderConjunction, out ulong value, out int next)
    {
        if (maxValue <= 9)
        {
            return TryParseUnit(tokens, start, end, maxValue, out value, out next);
        }

        if (maxValue < 100)
        {
            return TryParseUnderOneHundred(tokens, start, end, maxValue, out value, out next);
        }

        return TryParseValue(tokens, start, end, maxValue, allowTerminalRemainderConjunction, out value, out next);
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
        if (next < end &&
            tokens[next] == profile.ConjunctionWord &&
            TryParseUnit(tokens, next + 1, end, Math.Min(9UL, maxValue - value), out var unitValue, out var afterUnit) &&
            unitValue is >= 1UL and <= 9UL)
        {
            value += unitValue;
            next = afterUnit;
        }

        return true;
    }

    bool TryParseUnit(string[] tokens, int start, int end, ulong maxValue, out ulong value, out int next)
    {
        var lastCandidateEnd = Math.Min(end, start + profile.MaximumUnitTokenCount);
        foreach (var unit in profile.MultiTokenUnits)
        {
            if (start + unit.Tokens.Length > lastCandidateEnd)
            {
                continue;
            }

            if (unit.Value >= 0 &&
                (ulong)unit.Value <= maxValue &&
                TryMatchTokenPhrase(tokens, start, end, unit.Tokens, out next))
            {
                value = (ulong)unit.Value;
                return true;
            }
        }

        if (start < end &&
            profile.Units.TryGetValue(tokens[start], out var singleTokenRawValue) &&
            singleTokenRawValue >= 0 &&
            (ulong)singleTokenRawValue <= maxValue)
        {
            value = (ulong)singleTokenRawValue;
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

        var previous = (ulong)profile.Scales[scaleIndex - 1].Value;
        var current = (ulong)profile.Scales[scaleIndex].Value;
        if (previous <= current || previous % current != 0)
        {
            throw new InvalidOperationException("Scale-leading compound parser profiles require descending scales where each larger scale is divisible by the next smaller scale.");
        }

        return previous / current - 1UL;
    }

    bool TryParseOrdinalCandidate(string originalCandidate, string strippedCandidate, bool negative, out long parsedValue)
    {
        if (TryParseOrdinalCandidate(originalCandidate, negative, out parsedValue))
        {
            return true;
        }

        return !string.Equals(originalCandidate, strippedCandidate, StringComparison.Ordinal) &&
            TryParseOrdinalCandidate(strippedCandidate, negative, out parsedValue);
    }

    bool TryParseOrdinalCandidate(string candidate, bool negative, out long parsedValue)
    {
        if (profile.OrdinalMap.TryGetValue(candidate, out var ordinalValue))
        {
            parsedValue = negative ? -ordinalValue : ordinalValue;
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
            if (token != profile.ConjunctionWord &&
                token != profile.MinusWord &&
                !profile.Units.ContainsKey(token) &&
                !profile.Tens.ContainsKey(token) &&
                !profile.OrdinalMap.ContainsKey(token) &&
                !profile.Scales.Any(scale => scale.Name == token))
            {
                return token;
            }
        }

        return words;
    }

    static string Normalize(string value) =>
        string.Join(" ", value.Trim().ToLowerInvariant().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}


/// <summary>Pre-tokenized number word phrase for allocation-conscious matching.</summary>
/// <param name="Tokens">The normalized tokens in the phrase.</param>
/// <param name="Value">The numeric value represented by the phrase.</param>
readonly record struct TokenizedNumberWord(string[] Tokens, long Value);

/// <summary>Pre-tokenized scale phrase for allocation-conscious matching.</summary>
/// <param name="Value">The numeric scale value.</param>
/// <param name="Tokens">The normalized tokens in the scale name.</param>
readonly record struct TokenizedScaleLeadingCompoundScale(long Value, string[] Tokens);

/// <summary>
/// Immutable generated profile for <see cref="ScaleLeadingCompoundWordsToNumberConverter"/>.
/// </summary>
internal sealed class ScaleLeadingCompoundWordsToNumberProfile(
    FrozenDictionary<string, long> units,
    FrozenDictionary<string, long> tens,
    ScaleLeadingCompoundScale[] scales,
    string conjunctionWord,
    string? terminalRemainderConjunctionWord,
    string minusWord,
    string ordinalPrefix,
    string ordinalSuffix,
    FrozenDictionary<string, long>? ordinalMap = null)
{
    /// <summary>Gets unit and teen tokens.</summary>
    public FrozenDictionary<string, long> Units { get; } = units;
    /// <summary>Gets multi-token unit and teen entries, longest phrases first.</summary>
    public TokenizedNumberWord[] MultiTokenUnits { get; } = TokenizeNumberWords(units);
    /// <summary>Gets the maximum normalized token count in any unit phrase.</summary>
    public int MaximumUnitTokenCount { get; } = GetMaximumTokenCount(units.Keys);
    /// <summary>Gets decade tokens.</summary>
    public FrozenDictionary<string, long> Tens { get; } = tens;
    /// <summary>Gets descending scale rows.</summary>
    public ScaleLeadingCompoundScale[] Scales { get; } = ValidateScales(scales);
    /// <summary>Gets descending scale rows with pre-tokenized scale names.</summary>
    public TokenizedScaleLeadingCompoundScale[] TokenizedScales { get; } = TokenizeScales(ValidateScales(scales));
    /// <summary>Gets the conjunction token.</summary>
    public string ConjunctionWord { get; } = conjunctionWord;
    /// <summary>Gets the conjunction token used before potentially ambiguous terminal remainders.</summary>
    public string TerminalRemainderConjunctionWord { get; } = string.IsNullOrWhiteSpace(terminalRemainderConjunctionWord) ? conjunctionWord : terminalRemainderConjunctionWord!;
    /// <summary>Gets the pre-tokenized terminal remainder conjunction phrase.</summary>
    public string[] TerminalRemainderConjunctionTokens { get; } = Tokenize(string.IsNullOrWhiteSpace(terminalRemainderConjunctionWord) ? conjunctionWord : terminalRemainderConjunctionWord!);
    /// <summary>Gets the negative prefix token.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the ordinal prefix token.</summary>
    public string OrdinalPrefix { get; } = ordinalPrefix;
    /// <summary>Gets the ordinal suffix token.</summary>
    public string OrdinalSuffix { get; } = ordinalSuffix;
    /// <summary>Gets exact ordinal tokens.</summary>
    public FrozenDictionary<string, long> OrdinalMap { get; } = ordinalMap ?? FrozenDictionary<string, long>.Empty;

    static TokenizedNumberWord[] TokenizeNumberWords(FrozenDictionary<string, long> values) =>
        values
            .Select(pair => new TokenizedNumberWord(Tokenize(pair.Key), pair.Value))
            .Where(word => word.Tokens.Length > 1)
            .OrderByDescending(word => word.Tokens.Length)
            .ToArray();

    static TokenizedScaleLeadingCompoundScale[] TokenizeScales(ScaleLeadingCompoundScale[] value) =>
        value.Select(scale => new TokenizedScaleLeadingCompoundScale(scale.Value, Tokenize(scale.Name))).ToArray();

    static int GetMaximumTokenCount(IEnumerable<string> values)
    {
        var maximum = 1;
        foreach (var value in values)
        {
            maximum = Math.Max(maximum, Tokenize(value).Length);
        }

        return maximum;
    }

    static string[] Tokenize(string value) =>
        value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    static ScaleLeadingCompoundScale[] ValidateScales(ScaleLeadingCompoundScale[] value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i].Value <= 0)
            {
                throw new InvalidOperationException("Scale-leading compound parser profiles require positive scale values.");
            }

            if (i > 0)
            {
                var previous = (ulong)value[i - 1].Value;
                var current = (ulong)value[i].Value;
                if (previous <= current || previous % current != 0)
                {
                    throw new InvalidOperationException("Scale-leading compound parser profiles require descending scales where each larger scale is divisible by the next smaller scale.");
                }
            }
        }

        return value;
    }
}