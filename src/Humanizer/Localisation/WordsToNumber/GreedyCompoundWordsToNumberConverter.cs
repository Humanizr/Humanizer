namespace Humanizer;

/// <summary>
/// Parses languages that prefer greedy token matching for glued compounds and ordinal forms.
/// </summary>
internal class GreedyCompoundWordsToNumberConverter(GreedyCompoundWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly GreedyCompoundWordsToNumberProfile profile = profile;
    // Match the longest candidate first so shorter tokens do not steal the prefix of a glued
    // compound before the parser has a chance to recognize the full token.
    readonly string[] cardinalTokenOrder = profile.CardinalMap.Keys
        .OrderByDescending(static key => key.Length)
        .ToArray();

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

        var trimmed = words.Trim();
        var normalized = Normalize(trimmed);
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

        // Greedy locales deliberately prefer abbreviation and exact-ordinal matches before the
        // cardinal parser; otherwise tokens like "21st" or a locale-specific ordinal spelling can
        // be consumed as an ordinary compound and lose their suffix semantics.
        if (TryParseOrdinalAbbreviation(trimmed, out var value) ||
            profile.OrdinalMap.TryGetValue(normalized, out value) ||
            TryParseCardinal(normalized, out value, out unrecognizedWord))
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
    /// Normalizes a phrase according to the locale-specific character and diacritic rules.
    /// </summary>
    string Normalize(string words)
        => Normalize(
            words,
            profile.CharactersToRemove,
            profile.CharactersToReplaceWithSpace,
            profile.TextReplacements,
            profile.Lowercase,
            profile.RemoveDiacritics);

    /// <summary>
    /// Builds the ordinal lookup table used when a locale exposes ordinal spellings through the
    /// number-to-words renderer.
    /// </summary>
    /// <param name="converter">The locale-specific number-to-words converter.</param>
    /// <returns>A frozen map from normalized ordinal text to the corresponding integer.</returns>
    internal static FrozenDictionary<string, long> BuildOrdinalMap(
        INumberToWordsConverter converter,
        string charactersToRemove,
        string charactersToReplaceWithSpace,
        StringReplacement[] textReplacements,
        bool lowercase = false,
        bool removeDiacritics = false)
    {
        var ordinals = new Dictionary<string, long>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            foreach (var gender in new[] { GrammaticalGender.Masculine, GrammaticalGender.Feminine })
            {
                var ordinal = Normalize(
                    converter.ConvertToOrdinal(number, gender),
                    charactersToRemove,
                    charactersToReplaceWithSpace,
                    textReplacements,
                    lowercase,
                    removeDiacritics);

                if (!ordinals.ContainsKey(ordinal))
                {
                    ordinals[ordinal] = number;
                }
            }
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }

    /// <summary>
    /// Normalizes a phrase using locale-specific character stripping, replacements, casing, and
    /// diacritic handling.
    /// </summary>
    /// <param name="words">The text to normalize.</param>
    /// <param name="charactersToRemove">Characters that should be dropped entirely.</param>
    /// <param name="charactersToReplaceWithSpace">Characters that should be collapsed into spaces.</param>
    /// <param name="textReplacements">Literal replacements that should run before character filtering.</param>
    /// <param name="lowercase">Whether the output should be lowercased.</param>
    /// <param name="removeDiacritics">Whether combining marks should be stripped.</param>
    /// <returns>The normalized text.</returns>
    static string Normalize(
        string words,
        string charactersToRemove,
        string charactersToReplaceWithSpace,
        StringReplacement[] textReplacements,
        bool lowercase,
        bool removeDiacritics)
    {
        var normalized = removeDiacritics
            ? TokenMapWordsToNumberNormalizer.RemoveDiacritics(words)
            : words;

        foreach (var replacement in textReplacements)
        {
            normalized = normalized.Replace(replacement.OldValue, replacement.NewValue);
        }

        var source = normalized.AsSpan().Trim();
        var builder = new StringBuilder(source.Length);
        var previousWasSpace = false;

        foreach (var sourceChar in source)
        {
            var current = lowercase
                ? char.ToLowerInvariant(sourceChar)
                : sourceChar;

            if (charactersToRemove.Contains(current))
            {
                continue;
            }

            if (charactersToReplaceWithSpace.Contains(current))
            {
                current = ' ';
            }

            if (char.IsWhiteSpace(current))
            {
                if (previousWasSpace || builder.Length == 0)
                {
                    continue;
                }

                builder.Append(' ');
                previousWasSpace = true;
                continue;
            }

            builder.Append(current);
            previousWasSpace = false;
        }

        if (previousWasSpace)
        {
            builder.Length--;
        }

        return builder.ToString();
    }

    /// <summary>
    /// Parses a normalized phrase by greedily matching the longest known token at each position.
    /// </summary>
    /// <param name="words">A normalized phrase ready for token matching.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token or fragment that was not recognized.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string words, out long value, out string? unrecognizedWord)
    {
        if (profile.CardinalMap.TryGetValue(words, out value))
        {
            unrecognizedWord = null;
            return true;
        }

        value = default;
        unrecognizedWord = null;
        long total = 0;
        long current = 0;
        var position = 0;

        if (string.IsNullOrEmpty(words))
        {
            unrecognizedWord = string.Empty;
            return false;
        }

        while (position < words.Length)
        {
            if (char.IsWhiteSpace(words[position]))
            {
                position++;
                continue;
            }

            if (!TryReadToken(words, ref position, out var token))
            {
                // Preserve the raw fragment so the caller sees the actual unparsed word instead of
                // a synthetic token boundary.
                var start = position;
                while (position < words.Length && !char.IsWhiteSpace(words[position]))
                {
                    position++;
                }

                unrecognizedWord = words[start..position];
                return false;
            }

            if (ShouldIgnore(token))
            {
                continue;
            }

            if (!profile.CardinalMap.TryGetValue(token, out var numeric))
            {
                unrecognizedWord = token;
                return false;
            }

            if (numeric == profile.HundredValue)
            {
                current = checked((current == 0 ? 1 : current) * numeric);
            }
            else if (numeric >= profile.ScaleThreshold)
            {
                total = checked(total + checked((current == 0 ? 1 : current) * numeric));
                current = 0;
            }
            else
            {
                current = checked(current + numeric);
            }
        }

        value = checked(total + current);
        return true;
    }

    /// <summary>
    /// Parses an ordinal abbreviation such as <c>21st</c>.
    /// </summary>
    /// <param name="words">The trimmed input text.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the text is a supported ordinal abbreviation; otherwise, <c>false</c>.</returns>
    bool TryParseOrdinalAbbreviation(string words, out long value)
    {
        if (profile.OrdinalAbbreviationSuffixes.Length == 0)
        {
            value = default;
            return false;
        }

        var span = words.AsSpan().Trim();
        var digitLength = 0;

        while (digitLength < span.Length && span[digitLength] is >= '0' and <= '9')
        {
            digitLength++;
        }

        if (digitLength == 0 || digitLength == span.Length)
        {
            value = default;
            return false;
        }

        var suffix = span[digitLength..];
        foreach (var candidate in profile.OrdinalAbbreviationSuffixes)
        {
            if (suffix.Equals(candidate, StringComparison.Ordinal) &&
                long.TryParse(span[..digitLength], NumberStyles.None, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Reads the next matched token from the input while honoring the greedy token ordering.
    /// </summary>
    /// <param name="words">The normalized phrase.</param>
    /// <param name="position">The current read position within <paramref name="words"/>.</param>
    /// <param name="token">When this method returns, the matched token.</param>
    /// <returns><c>true</c> if a token was matched; otherwise, <c>false</c>.</returns>
    bool TryReadToken(string words, ref int position, out string token)
    {
        foreach (var candidate in cardinalTokenOrder)
        {
            if (position + candidate.Length > words.Length)
            {
                continue;
            }

            if (words.AsSpan(position, candidate.Length).SequenceEqual(candidate.AsSpan()))
            {
                token = candidate;
                position += candidate.Length;
                return true;
            }
        }

        token = string.Empty;
        return false;
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
}

/// <summary>
/// Immutable locale data used by <see cref="GreedyCompoundWordsToNumberConverter"/>.
/// </summary>
sealed class GreedyCompoundWordsToNumberProfile(
    FrozenDictionary<string, long> cardinalMap,
    FrozenDictionary<string, long> ordinalMap,
    string[] negativePrefixes,
    string[] ignoredTokens,
    string[] ordinalAbbreviationSuffixes,
    string charactersToRemove,
    string charactersToReplaceWithSpace,
    StringReplacement[] textReplacements,
    bool lowercase = false,
    bool removeDiacritics = false,
    long hundredValue = 100,
    long scaleThreshold = 1000)
{
    /// <summary>
    /// Gets the token-to-value map used by the parser.
    /// </summary>
    public FrozenDictionary<string, long> CardinalMap { get; } = cardinalMap;
    /// <summary>
    /// Gets the exact ordinal-token map accepted by the parser.
    /// </summary>
    public FrozenDictionary<string, long> OrdinalMap { get; } = ordinalMap;
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
    /// <summary>
    /// Gets the tokens that should be skipped during parsing.
    /// </summary>
    public string[] IgnoredTokens { get; } = ignoredTokens;
    /// <summary>
    /// Gets the suffixes that may appear on ordinal abbreviations such as <c>st</c> or <c>th</c>.
    /// </summary>
    public string[] OrdinalAbbreviationSuffixes { get; } = ordinalAbbreviationSuffixes;
    /// <summary>
    /// Gets the characters removed during normalization.
    /// </summary>
    public string CharactersToRemove { get; } = charactersToRemove;
    /// <summary>
    /// Gets the characters that are converted into spaces during normalization.
    /// </summary>
    public string CharactersToReplaceWithSpace { get; } = charactersToReplaceWithSpace;
    /// <summary>
    /// Gets literal replacements applied before character filtering.
    /// </summary>
    public StringReplacement[] TextReplacements { get; } = textReplacements;
    /// <summary>
    /// Gets a value indicating whether the normalized output should be lowercased.
    /// </summary>
    public bool Lowercase { get; } = lowercase;
    /// <summary>
    /// Gets a value indicating whether combining marks are removed before parsing.
    /// </summary>
    public bool RemoveDiacritics { get; } = removeDiacritics;
    /// <summary>
    /// Gets the value that represents a hundred token in the locale.
    /// </summary>
    public long HundredValue { get; } = hundredValue;
    /// <summary>
    /// Gets the value at or above which tokens are treated as large scales.
    /// </summary>
    public long ScaleThreshold { get; } = scaleThreshold;
}