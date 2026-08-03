namespace Humanizer;

/// <summary>
/// Parses languages that use linking affixes inside compounds, such as embedded teen stems or
/// joined suffixes on cardinal tokens.
/// </summary>
internal class LinkingAffixWordsToNumberConverter(LinkingAffixWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly LinkingAffixWordsToNumberProfile profile = profile;

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

        var normalized = TokenMapWordsToNumberNormalizer.Normalize(
            words.Replace("'", string.Empty).Replace("’", string.Empty),
            TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces);
        var negative = false;

        foreach (var negativePrefix in profile.NegativePrefixes)
        {
            if (!normalized.StartsWith(negativePrefix, StringComparison.Ordinal))
            {
                continue;
            }

            negative = true;
            normalized = normalized[negativePrefix.Length..].Trim();
            break;
        }

        if (TryParseCardinal(normalized, out var value))
        {
            parsedValue = value;
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = WordsToNumberTokenizer.GetLastTokenOrSelf(normalized);
        parsedValue = default;
        return false;
    }

    /// <summary>
    /// Parses a normalized cardinal phrase with linked suffixes and teen prefixes.
    /// </summary>
    /// <param name="words">A normalized phrase ready for token-by-token parsing.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string words, out long value)
    {
        if (profile.CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        long total = 0;
        long current = 0;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();
            if (ShouldIgnore(token))
            {
                continue;
            }

            if (TryParseTeenToken(token, out var teenValue))
            {
                current = checked(current + teenValue);
                continue;
            }

            if (TryParseLinkedToken(token, out var linkedValue))
            {
                current = checked(current + linkedValue);
                continue;
            }

            if (!profile.CardinalMap.TryGetValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (tokenValue >= profile.ScaleThreshold)
            {
                total = checked(total + checked((current == 0 ? 1 : current) * tokenValue));
                current = 0;
            }
            else if (tokenValue == profile.HundredValue)
            {
                current = checked((current == 0 ? 1 : current) * tokenValue);
            }
            else
            {
                current = checked(current + tokenValue);
            }
        }

        value = checked(total + current);
        return true;
    }

    /// <summary>
    /// Consumes consecutive teen prefixes without growing the call stack, then resolves the
    /// remaining cardinal or linked token.
    /// </summary>
    bool TryParseTeenToken(string token, out long value)
    {
        var remainder = token.AsSpan();
        var prefixCount = 0L;
        var hasMappedTerminal = false;
        var terminalValue = default(long);

        while (remainder.StartsWith(profile.TeenPrefix, StringComparison.Ordinal) &&
               remainder.Length > profile.TeenPrefix.Length)
        {
            prefixCount++;
            remainder = remainder[profile.TeenPrefix.Length..];

            foreach (var cardinal in profile.CardinalMap)
            {
                if (remainder.Length != cardinal.Key.Length || !remainder.SequenceEqual(cardinal.Key.AsSpan()))
                {
                    continue;
                }

                terminalValue = cardinal.Value;
                hasMappedTerminal = true;
                break;
            }

            if (hasMappedTerminal)
            {
                break;
            }
        }

        if (prefixCount == 0)
        {
            value = default;
            return false;
        }

        if (!hasMappedTerminal && !TryParseCardinal(remainder.ToString(), out terminalValue))
        {
            value = default;
            return false;
        }

        value = terminalValue;
        for (var index = 0L; index < prefixCount; index++)
        {
            value = checked(profile.TeenBaseValue + value);
        }

        return true;
    }

    /// <summary>
    /// Returns <c>true</c> when a token is configured to be ignored during parsing.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <returns><c>true</c> if the token should be skipped; otherwise, <c>false</c>.</returns>
    bool ShouldIgnore(string token) =>
        Array.IndexOf(profile.IgnoredTokens, token) >= 0;

    /// <summary>
    /// Tries to strip a linked suffix from a token and resolve the base token as a cardinal value.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the token ends with a known linked suffix; otherwise, <c>false</c>.</returns>
    bool TryParseLinkedToken(string token, out long value)
    {
        foreach (var suffix in profile.LinkedSuffixes)
        {
            if (!token.EndsWith(suffix, StringComparison.Ordinal) || token.Length == suffix.Length)
            {
                continue;
            }

            if (profile.CardinalMap.TryGetValue(token[..^suffix.Length], out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }
}

/// <summary>
/// Immutable locale data used by <see cref="LinkingAffixWordsToNumberConverter"/>.
/// </summary>
sealed class LinkingAffixWordsToNumberProfile(
    FrozenDictionary<string, long> cardinalMap,
    string teenPrefix,
    long teenBaseValue,
    string[] linkedSuffixes,
    string[] ignoredTokens,
    string[] negativePrefixes,
    long hundredValue = 100,
    long scaleThreshold = 1000)
{
    /// <summary>
    /// Gets the token-to-value map used by the parser.
    /// </summary>
    public FrozenDictionary<string, long> CardinalMap { get; } = cardinalMap;
    /// <summary>
    /// Gets the prefix that marks a teen stem.
    /// </summary>
    public string TeenPrefix { get; } = string.IsNullOrEmpty(teenPrefix)
        ? throw new ArgumentException("Teen prefix cannot be empty.", nameof(teenPrefix))
        : teenPrefix;
    /// <summary>
    /// Gets the base value added when a teen prefix is matched.
    /// </summary>
    public long TeenBaseValue { get; } = teenBaseValue;
    /// <summary>
    /// Gets the suffixes that may be attached to a linked cardinal token.
    /// </summary>
    public string[] LinkedSuffixes { get; } = linkedSuffixes;
    /// <summary>
    /// Gets the tokens that should be skipped during parsing.
    /// </summary>
    public string[] IgnoredTokens { get; } = ignoredTokens;
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
    /// <summary>
    /// Gets the value that represents a hundred token in the locale.
    /// </summary>
    public long HundredValue { get; } = hundredValue;
    /// <summary>
    /// Gets the value at or above which tokens are treated as large scales.
    /// </summary>
    public long ScaleThreshold { get; } = scaleThreshold;
}