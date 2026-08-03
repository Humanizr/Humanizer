namespace Humanizer;

/// <summary>
/// Parses locales whose compound tens place the unit before the tens token and may also encode
/// scales, ordinals, and optional ignored glue words in the same phrase.
/// </summary>
/// <remarks>
/// The parser normalizes the input first, removes one configured negative prefix, then resolves
/// either an exact ordinal token or a cardinal phrase composed from compact compounds and scale
/// words. The profile captures all locale-specific vocabulary so the algorithm can stay structural.
/// </remarks>
internal class InvertedTensWordsToNumberConverter(InvertedTensWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    const int MaximumParseDepth = 128;

    readonly InvertedTensWordsToNumberProfile profile = profile;

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

        if (profile.AllowInvariantIntegerInput &&
            long.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            return true;
        }

        if (profile.OrdinalMap.TryGetValue(normalized, out var value) ||
            TryParsePhrase(normalized, out value, out unrecognizedWord))
        {
            parsedValue = value;
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
    /// Parses a tokenized phrase where scales and hundreds may appear as standalone words.
    /// </summary>
    /// <param name="words">The normalized phrase to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the first token that could not be parsed.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParsePhrase(string words, out long value, out string? unrecognizedWord)
    {
        if (!words.Contains(' '))
        {
            return TryParseCompact(words, out value, out unrecognizedWord);
        }

        var remainingWork = (long)words.Length * 32L;
        long total = 0;
        long current = 0;
        unrecognizedWord = null;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (ShouldIgnore(token))
            {
                continue;
            }

            if (!TryParseCompactCore(token.AsSpan(), 0, ref remainingWork, out var tokenValue))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (tokenValue >= 1000)
            {
                total = checked(total + checked((current == 0 ? 1 : current) * tokenValue));
                current = 0;
            }
            else if (tokenValue == 100)
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
    /// Parses either a single token or a collapsed multi-token compound.
    /// </summary>
    /// <param name="word">The normalized token or compact compound to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token or compound that was not recognized.</param>
    /// <returns><c>true</c> if the token was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCompact(string word, out long value, out string? unrecognizedWord)
    {
        var remainingWork = (long)word.Length * 32L;
        var parsed = TryParseCompactCore(word.AsSpan(), 0, ref remainingWork, out value);
        unrecognizedWord = parsed ? null : word;
        return parsed;
    }

    bool TryParseCompactCore(ReadOnlySpan<char> word, int depth, ref long remainingWork, out long value)
    {
        if (depth >= MaximumParseDepth || remainingWork-- <= 0)
        {
            remainingWork = -1;
            value = default;
            return false;
        }

        if (TryGetValue(profile.CardinalMap, word, out value) ||
            TryGetValue(profile.OrdinalMap, word, out value))
        {
            return true;
        }

        if (TryParseOrdinalStem(word, depth, ref remainingWork, out value))
        {
            return true;
        }

        if (remainingWork < 0)
        {
            return false;
        }

        // Direct lexical matches must win before structural splitting. If the scale or compact-ten
        // logic ran first, exact locale words that merely contain a scale token would be
        // reinterpreted as composites and produce the wrong numeric value.
        foreach (var scale in profile.ScaleTokens)
        {
            var index = word.IndexOf(scale.AsSpan(), StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = word[..index];
            var right = StripLeadingIgnoredTokens(TrimSpaces(word[(index + scale.Length)..]));
            long factor = 1;

            if (!left.IsEmpty &&
                !TryParseCompactCore(left, depth + 1, ref remainingWork, out factor))
            {
                if (remainingWork < 0)
                {
                    value = default;
                    return false;
                }

                continue;
            }

            if (!TryParseOptional(right, depth + 1, ref remainingWork, out var remainder))
            {
                if (remainingWork < 0)
                {
                    value = default;
                    return false;
                }

                continue;
            }

            value = checked(checked(factor * profile.CardinalMap[scale]) + remainder);
            return true;
        }

        if (TryParseCompoundTens(word, out value))
        {
            return true;
        }

        // Whitespace-collapsed retries are intentionally last: they are a recovery path for glued
        // compounds, not a primary parse mode, and running them earlier would hide the more useful
        // failure token from the explicit token and scale branches above.
        if (word.Contains(' '))
        {
            var collapsed = word.ToString().Replace(" ", string.Empty);
            if (TryParseCompactCore(collapsed.AsSpan(), depth + 1, ref remainingWork, out value))
            {
                return true;
            }

            if (remainingWork < 0)
            {
                value = default;
                return false;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Removes a configured ordinal suffix and reparses the remaining stem.
    /// </summary>
    /// <param name="word">The normalized token to inspect.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if an ordinal suffix was recognized and the stem parsed; otherwise, <c>false</c>.</returns>
    bool TryParseOrdinalStem(ReadOnlySpan<char> word, int depth, ref long remainingWork, out long value)
    {
        foreach (var suffix in profile.OrdinalSuffixes)
        {
            if (!word.EndsWith(suffix.AsSpan(), StringComparison.Ordinal) || word.Length == suffix.Length)
            {
                continue;
            }

            return TryParseCompactCore(word[..^suffix.Length], depth + 1, ref remainingWork, out value);
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Parses an optional trailing fragment, treating the empty string as zero.
    /// </summary>
    /// <param name="word">The normalized fragment to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the fragment was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseOptional(ReadOnlySpan<char> word, int depth, ref long remainingWork, out long value)
    {
        if (word.IsEmpty)
        {
            value = 0;
            return true;
        }

        return TryParseCompactCore(word, depth, ref remainingWork, out value);
    }

    /// <summary>
    /// Parses a compact unit-plus-tens form that ends with a configured tens token.
    /// </summary>
    /// <param name="word">The normalized compact token to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the token matched a supported unit-plus-tens pattern; otherwise, <c>false</c>.</returns>
    bool TryParseCompoundTens(ReadOnlySpan<char> word, out long value)
    {
        foreach (var tensToken in profile.TensTokens)
        {
            if (!word.EndsWith(tensToken.Word.AsSpan(), StringComparison.Ordinal))
            {
                continue;
            }

            var prefix = word[..^tensToken.Word.Length];
            if (!prefix.EndsWith(profile.TensLinker.AsSpan(), StringComparison.Ordinal))
            {
                continue;
            }

            var unitPart = prefix[..^profile.TensLinker.Length];
            var foundUnit = profile.UnitPartReplacements.Length == 0
                ? TryGetValue(profile.UnitMap, unitPart, out var unitValue)
                : profile.UnitMap.TryGetValue(ApplyUnitPartReplacements(unitPart.ToString()), out unitValue);
            if (foundUnit && unitValue is >= 1 and <= 9)
            {
                value = tensToken.Value + unitValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Removes leading ignored tokens from a scale remainder before reparsing it.
    /// </summary>
    /// <param name="words">The normalized remainder to trim.</param>
    /// <returns>The remainder without any configured leading ignored tokens.</returns>
    ReadOnlySpan<char> StripLeadingIgnoredTokens(ReadOnlySpan<char> words)
    {
        if (words.IsEmpty)
        {
            return words;
        }

        var remaining = words;
        var stripped = true;

        while (stripped)
        {
            stripped = false;

            foreach (var ignoredToken in profile.IgnoredTokens)
            {
                if (string.IsNullOrEmpty(ignoredToken) ||
                    !remaining.StartsWith(ignoredToken.AsSpan(), StringComparison.Ordinal))
                {
                    continue;
                }

                remaining = TrimStartSpaces(remaining[ignoredToken.Length..]);
                stripped = true;
                break;
            }
        }

        return remaining;
    }

    static bool TryGetValue(FrozenDictionary<string, long> values, ReadOnlySpan<char> word, out long value)
    {
        foreach (var candidate in values)
        {
            if (word.Equals(candidate.Key.AsSpan(), StringComparison.Ordinal))
            {
                value = candidate.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    static ReadOnlySpan<char> TrimSpaces(ReadOnlySpan<char> words)
    {
        var trimmed = TrimStartSpaces(words);
        while (!trimmed.IsEmpty && trimmed[^1] == ' ')
        {
            trimmed = trimmed[..^1];
        }

        return trimmed;
    }

    static ReadOnlySpan<char> TrimStartSpaces(ReadOnlySpan<char> words)
    {
        while (!words.IsEmpty && words[0] == ' ')
        {
            words = words[1..];
        }

        return words;
    }

    /// <summary>
    /// Normalizes the unit portion of a compact compound before looking it up in <see cref="InvertedTensWordsToNumberProfile.UnitMap"/>.
    /// </summary>
    /// <param name="word">The unit fragment to normalize.</param>
    /// <returns>The normalized unit fragment.</returns>
    string ApplyUnitPartReplacements(string word)
    {
        var normalized = word;

        foreach (var replacement in profile.UnitPartReplacements)
        {
            normalized = normalized.Replace(replacement.OldValue, replacement.NewValue);
        }

        return normalized;
    }

    /// <summary>
    /// Returns <c>true</c> when a token should be skipped during phrase parsing.
    /// </summary>
    /// <param name="token">The normalized token to inspect.</param>
    /// <returns><c>true</c> if the token is configured as ignorable; otherwise, <c>false</c>.</returns>
    bool ShouldIgnore(string token) =>
        Array.IndexOf(profile.IgnoredTokens, token) >= 0;

    /// <summary>
    /// Normalizes words for dictionary lookup by lowercasing them and removing periods.
    /// </summary>
    /// <param name="words">The raw input phrase.</param>
    /// <returns>The normalized lookup string.</returns>
    static string Normalize(string words) =>
        TokenMapWordsToNumberNormalizer.Normalize(words, TokenMapNormalizationProfile.LowercaseRemovePeriods);

    /// <summary>
    /// Builds an ordinal lookup table by normalizing the locale's ordinal output for the first
    /// two hundred values.
    /// </summary>
    /// <param name="converter">The locale-specific number-to-words converter used to generate ordinal forms.</param>
    /// <returns>A frozen dictionary that maps normalized ordinal text back to its numeric value.</returns>
    internal static FrozenDictionary<string, long> BuildOrdinalMap(INumberToWordsConverter converter)
    {
        var ordinals = new Dictionary<string, long>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

/// <summary>
/// Immutable locale data used by <see cref="InvertedTensWordsToNumberConverter"/>.
/// </summary>
internal sealed class InvertedTensWordsToNumberProfile(
    FrozenDictionary<string, long> cardinalMap,
    FrozenDictionary<string, long> unitMap,
    InvertedTensToken[] tensTokens,
    string tensLinker,
    string[] scaleTokens,
    FrozenDictionary<string, long> ordinalMap,
    string[] negativePrefixes,
    string[] ignoredTokens,
    string[] ordinalSuffixes,
    StringReplacement[] unitPartReplacements,
    bool allowInvariantIntegerInput = false)
{
    /// <summary>
    /// Gets the main token-to-value map used for direct token lookups.
    /// </summary>
    public FrozenDictionary<string, long> CardinalMap { get; } = cardinalMap;
    /// <summary>
    /// Gets the map used when resolving the unit portion of inverted tens compounds.
    /// </summary>
    public FrozenDictionary<string, long> UnitMap { get; } = unitMap;
    /// <summary>
    /// Gets the supported tens tokens and their values.
    /// </summary>
    public InvertedTensToken[] TensTokens { get; } = tensTokens;
    /// <summary>
    /// Gets the linker that separates the unit fragment from the tens token.
    /// </summary>
    public string TensLinker { get; } = tensLinker;
    /// <summary>
    /// Gets the scale tokens that may appear inside compact forms.
    /// </summary>
    public string[] ScaleTokens { get; } = scaleTokens;
    /// <summary>
    /// Gets the exact ordinal token map accepted before falling back to structural parsing.
    /// </summary>
    public FrozenDictionary<string, long> OrdinalMap { get; } = ordinalMap;
    /// <summary>
    /// Gets the prefixes that mark a negative phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
    /// <summary>
    /// Gets the tokens that should be ignored while parsing phrases.
    /// </summary>
    public string[] IgnoredTokens { get; } = ignoredTokens;
    /// <summary>
    /// Gets the suffixes that turn a cardinal stem into an ordinal token.
    /// </summary>
    public string[] OrdinalSuffixes { get; } = ordinalSuffixes;
    /// <summary>
    /// Gets the replacements applied to the unit fragment of compact tens compounds before lookup.
    /// </summary>
    public StringReplacement[] UnitPartReplacements { get; } = unitPartReplacements;
    /// <summary>
    /// Gets a value indicating whether invariant integer text is accepted directly.
    /// </summary>
    public bool AllowInvariantIntegerInput { get; } = allowInvariantIntegerInput;
}

/// <summary>
/// Represents one tens token and the numeric value it contributes to a compact compound.
/// </summary>
internal readonly record struct InvertedTensToken(string Word, long Value);

/// <summary>
/// Represents a literal replacement applied while normalizing compact unit fragments.
/// </summary>
internal readonly record struct StringReplacement(string OldValue, string NewValue);