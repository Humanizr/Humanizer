namespace Humanizer;

/// <summary>
/// Shared parser for languages where written numbers are composed from:
/// - direct cardinal tokens
/// - optional tens stems
/// - explicit large-scale words such as thousand or million
/// - optional ordinal forms derived from either explicit YAML or a generated number-to-words bridge
///
/// The parser normalizes the input, strips a configured negative prefix, then resolves either an
/// exact ordinal token or a cardinal phrase assembled from token groups and scale multipliers.
/// The end result should be the integer value the locale phrase denotes, not merely a best-effort
/// token sum.
/// </summary>
internal class CompoundScaleWordsToNumberConverter(CompoundScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly CompoundScaleWordsToNumberProfile profile = profile;

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

        var normalized = Normalize(words);
        var negative = false;

        // Negative handling is deliberately outside the main parser so the cardinal and ordinal
        // rules can stay focused on the positive phrase grammar.
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

        // Ordinal spellings are checked before the cardinal parser so exact forms like "twelfth"
        // are not split into an unrelated cardinal phrase.
        if (profile.OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue))
        {
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
    /// Parses a normalized cardinal phrase made up of direct tokens, optional tens stems, and
    /// large-scale words.
    /// </summary>
    /// <param name="words">A normalized, lowercase phrase with punctuation already removed.</param>
    /// <param name="value">When this method returns, the parsed integer value.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string words, out int value)
    {
        if (profile.CardinalMap.TryGetValue(words, out value))
        {
            return true;
        }

        if (words.Contains(' '))
        {
            var total = 0;
            var current = 0;

            // Multi-token phrases are reduced left to right. Small tokens accumulate into the
            // current group, while scale tokens flush the current group into the total using the
            // locale's scale-multiplier semantics.
            foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
            {
                var token = tokenSpan.ToString();

                if (token == profile.IgnoredToken)
                {
                    continue;
                }

                if (!TryParseCardinal(token, out var tokenValue))
                {
                    value = default;
                    return false;
                }

                if (tokenValue >= 1000)
                {
                    total += (current == 0 ? 1 : current) * tokenValue;
                    current = 0;
                }
                else if (profile.SequenceMultiplierThreshold.HasValue &&
                         tokenValue >= profile.SequenceMultiplierThreshold.Value)
                {
                    current = (current == 0 ? 1 : current) * tokenValue;
                }
                else
                {
                    current += tokenValue;
                }
            }

            value = total + current;
            return true;
        }

        // Single-token compounds such as "twothousandfive" are handled by splitting around known
        // scale words, then recursively parsing the left factor and right remainder. This branch
        // is intentionally after the space-delimited path so mixed-token phrases keep the simpler
        // left-to-right reduction.
        foreach (var scale in profile.LargeScales)
        {
            var index = words.IndexOf(scale, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = words[..index].Trim();
            var right = words[(index + scale.Length)..].Trim();
            var factor = 1;

            if ((string.IsNullOrEmpty(left) || TryParseCardinal(left, out factor)) &&
                TryParseOptional(right, out var remainder))
            {
                value = factor * profile.CardinalMap[scale] + remainder;
                return true;
            }
        }

        // The tens fallback handles glued compounds where the tens stem is written as one token
        // followed by a directly attached unit word. We only accept units in the 1-9 range so the
        // fallback cannot swallow unrelated larger cardinals.
        foreach (var tens in profile.Tens)
        {
            if (!words.StartsWith(tens, StringComparison.Ordinal))
            {
                continue;
            }

            var remainder = words[tens.Length..];
            if (string.IsNullOrEmpty(remainder))
            {
                value = profile.CardinalMap[tens];
                return true;
            }

            if (TryParseCardinal(remainder, out var unit) && unit is >= 1 and <= 9)
            {
                value = profile.CardinalMap[tens] + unit;
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Parses an optional remainder after a scale word.
    /// </summary>
    /// <param name="words">The remainder text following a scale token.</param>
    /// <param name="value">When this method returns, the parsed remainder value.</param>
    /// <returns><c>true</c> if the remainder is empty or parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseOptional(string words, out int value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        if (!string.IsNullOrEmpty(profile.IgnoredToken))
        {
            var ignoredTokenWithSpace = profile.IgnoredToken + " ";
            if (words.StartsWith(ignoredTokenWithSpace, StringComparison.Ordinal))
            {
                words = words[ignoredTokenWithSpace.Length..];
            }
            else if (words.StartsWith(profile.IgnoredToken, StringComparison.Ordinal))
            {
                words = words[profile.IgnoredToken.Length..];
            }
        }

        return TryParseCardinal(words, out value);
    }

    /// <summary>
    /// Normalizes a phrase before parsing.
    /// </summary>
    /// <remarks>
    /// Normalization removes punctuation and hyphens so locale data can focus on lexical meaning
    /// instead of every orthographic variant users might type.
    /// </remarks>
    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    /// <summary>
    /// Builds the ordinal lookup table used by compound-scale locales.
    /// </summary>
    /// <param name="converter">The number-to-words converter used to render ordinals.</param>
    /// <returns>A frozen map from normalized ordinal text to the corresponding integer.</returns>
    internal static FrozenDictionary<string, int> BuildOrdinalMap(INumberToWordsConverter converter)
    {
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        // This generation-time bridge lets parsing reuse an existing ordinal renderer when the
        // locale would otherwise have to duplicate hundreds of ordinal spellings in YAML.
        for (var number = 1; number <= 200; number++)
        {
            ordinals[Normalize(converter.ConvertToOrdinal(number))] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}

/// <summary>
/// Immutable locale data for <see cref="CompoundScaleWordsToNumberConverter"/>.
/// </summary>
sealed class CompoundScaleWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    string[] tens,
    string[] largeScales,
    string ignoredToken,
    FrozenDictionary<string, int> ordinalMap,
    string[] negativePrefixes,
    int? sequenceMultiplierThreshold = null)
{
    /// <summary>
    /// Gets the exact token-to-value map for cardinal words and scale words.
    /// </summary>
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;

    /// <summary>
    /// Gets the ordered tens stems that may participate in glued compounds.
    /// </summary>
    public string[] Tens { get; } = tens;

    /// <summary>
    /// Gets the scale words that split a compound phrase into left-factor and remainder segments.
    /// </summary>
    public string[] LargeScales { get; } = largeScales;

    /// <summary>
    /// Gets a filler token that should be skipped during parsing when present.
    /// </summary>
    public string IgnoredToken { get; } = ignoredToken;

    /// <summary>
    /// Gets the exact ordinal-token map accepted by the parser.
    /// </summary>
    public FrozenDictionary<string, int> OrdinalMap { get; } = ordinalMap;

    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;

    /// <summary>
    /// Gets the threshold above which adjacent values should multiply instead of simply adding.
    /// </summary>
    public int? SequenceMultiplierThreshold { get; } = sequenceMultiplierThreshold;
}
