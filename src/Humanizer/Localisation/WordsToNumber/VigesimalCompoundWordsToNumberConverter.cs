namespace Humanizer;

/// <summary>
/// Parses languages that combine base-20 constructions with ordinary scale words.
/// </summary>
internal class VigesimalCompoundWordsToNumberConverter(VigesimalCompoundWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly VigesimalCompoundWordsToNumberProfile profile = profile;

    /// <inheritdoc />
    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    /// <inheritdoc />
    public override bool TryConvert(string words, out int parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    /// <inheritdoc />
    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        var normalized = TokenMapWordsToNumberNormalizer.Normalize(words, TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces);
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

        if (profile.OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        return false;
    }

    /// <summary>
    /// Parses a normalized cardinal phrase that may contain vigesimal and teen compounds.
    /// </summary>
    /// <param name="words">A normalized phrase ready for tokenization.</param>
    /// <param name="value">When this method returns, the parsed integer value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token that was not recognized.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var current = 0;
        unrecognizedWord = null;
        var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
        string? pendingToken = null;

        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var token))
        {
            if (ShouldIgnore(token))
            {
                continue;
            }

            // The vigesimal and teen lookaheads must run before ordinary token lookup because they
            // reinterpret the current token as a structural marker rather than a literal number.
            // When the follower does not qualify, the peeked token is pushed back so the normal
            // token path can still consume it on the next loop iteration.
            if (token == profile.VigesimalLeadingToken &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var vigesimalFollower))
            {
                if (IsVigesimalFollower(vigesimalFollower))
                {
                    current += profile.VigesimalValue;
                    continue;
                }

                pendingToken = vigesimalFollower;
            }

            if (token == profile.TeenLeaderToken &&
                profile.TeenLeaderBases.Contains(current) &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var teenToken))
            {
                if (profile.CardinalMap.TryGetValue(teenToken, out var teenPart) && teenPart is >= 1 and <= 9)
                {
                    current += profile.TeenBaseValue + teenPart;
                    continue;
                }

                pendingToken = teenToken;
            }

            if (!profile.CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (numeric == profile.HundredValue)
            {
                current = (current == 0 ? 1 : current) * numeric;
            }
            else if (numeric >= profile.ScaleThreshold)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
            }
            else
            {
                current += numeric;
            }
        }

        value = total + current;
        return true;
    }

    /// <summary>
    /// Returns <c>true</c> when a token is configured to be ignored during parsing.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <returns><c>true</c> if the token should be skipped; otherwise, <c>false</c>.</returns>
    bool ShouldIgnore(string token)
    {
        foreach (var ignoredToken in profile.IgnoredTokens)
        {
            if (token == ignoredToken)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns <c>true</c> when the next token is a supported follower for the vigesimal leader.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <returns><c>true</c> if the token is a known follower; otherwise, <c>false</c>.</returns>
    bool IsVigesimalFollower(string token)
    {
        foreach (var follower in profile.VigesimalFollowerTokens)
        {
            if (token == follower)
            {
                return true;
            }
        }

        return false;
    }
}

/// <summary>
/// Immutable locale data used by <see cref="VigesimalCompoundWordsToNumberConverter"/>.
/// </summary>
sealed class VigesimalCompoundWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    FrozenDictionary<string, int> ordinalMap,
    string[] negativePrefixes,
    string[] ignoredTokens,
    string vigesimalLeadingToken,
    string[] vigesimalFollowerTokens,
    int vigesimalValue,
    string teenLeaderToken,
    FrozenSet<int> teenLeaderBases,
    int teenBaseValue = 10,
    int hundredValue = 100,
    int scaleThreshold = 1000)
{
    /// <summary>
    /// Gets the token-to-value map used by the parser.
    /// </summary>
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    /// <summary>
    /// Gets the exact ordinal-token map accepted by the parser.
    /// </summary>
    public FrozenDictionary<string, int> OrdinalMap { get; } = ordinalMap;
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
    /// <summary>
    /// Gets the tokens that should be skipped during parsing.
    /// </summary>
    public string[] IgnoredTokens { get; } = ignoredTokens;
    /// <summary>
    /// Gets the token that introduces a vigesimal compound.
    /// </summary>
    public string VigesimalLeadingToken { get; } = vigesimalLeadingToken;
    /// <summary>
    /// Gets the tokens that may follow the vigesimal leader.
    /// </summary>
    public string[] VigesimalFollowerTokens { get; } = vigesimalFollowerTokens;
    /// <summary>
    /// Gets the value contributed by the vigesimal leader.
    /// </summary>
    public int VigesimalValue { get; } = vigesimalValue;
    /// <summary>
    /// Gets the token that introduces a teen compound after a base value.
    /// </summary>
    public string TeenLeaderToken { get; } = teenLeaderToken;
    /// <summary>
    /// Gets the base values that can legally precede the teen leader token.
    /// </summary>
    public FrozenSet<int> TeenLeaderBases { get; } = teenLeaderBases;
    /// <summary>
    /// Gets the base value used for teen compounds.
    /// </summary>
    public int TeenBaseValue { get; } = teenBaseValue;
    /// <summary>
    /// Gets the value that represents a hundred token in the locale.
    /// </summary>
    public int HundredValue { get; } = hundredValue;
    /// <summary>
    /// Gets the value at or above which tokens are treated as large scales.
    /// </summary>
    public int ScaleThreshold { get; } = scaleThreshold;
}
