namespace Humanizer;

/// <summary>
/// Parses languages that attach scale words and tens stems as prefixes inside a glued compound.
/// </summary>
internal class PrefixedTensScaleWordsToNumberConverter(PrefixedTensScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    const int MaximumParseDepth = 128;

    readonly PrefixedTensScaleWordsToNumberProfile profile = profile;

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

        foreach (var prefix in profile.NegativePrefixes)
        {
            if (!normalized.StartsWith(prefix, StringComparison.Ordinal))
            {
                continue;
            }

            negative = true;
            normalized = normalized[prefix.Length..];
            break;
        }

        normalized = CollapseCompoundSeparators(normalized);

        var value = default(long);
        if (long.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue) ||
            TryParseCardinal(normalized, out value))
        {
            if (parsedValue == default && value != default)
            {
                parsedValue = value;
            }

            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = normalized;
        parsedValue = default;
        return false;
    }

    /// <summary>
    /// Parses a normalized and separator-collapsed cardinal phrase.
    /// </summary>
    /// <param name="word">The normalized phrase to parse.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string word, out long value)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = default;
            return false;
        }

        if (profile.CardinalMap.TryGetValue(word, out value))
        {
            return true;
        }

        var remainingWork = (long)word.Length * 32L;
        return TryParseCardinal(word.AsSpan(), 0, ref remainingWork, out value);
    }

    /// <summary>
    /// Recursively parses a cardinal phrase while bounding stack depth and shared parsing work.
    /// </summary>
    /// <param name="word">The remaining phrase to parse.</param>
    /// <param name="depth">The current recursion depth.</param>
    /// <param name="remainingWork">The shared parse-tree budget; a negative value stops alternative branches.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(ReadOnlySpan<char> word, int depth, ref long remainingWork, out long value)
    {
        if (TryGetValue(profile.CardinalMap, word, out value))
        {
            return true;
        }

        if (word.IsEmpty || depth >= MaximumParseDepth || remainingWork-- <= 0)
        {
            if (!word.IsEmpty)
            {
                remainingWork = -1;
            }

            value = default;
            return false;
        }

        // Scale decomposition must run before prefix/tens handling because many glued compounds
        // contain tokens that look like smaller prefixes. Once a scale token matches, the left and
        // right fragments define the only valid recursive split for that branch.
        foreach (var scale in profile.Scales)
        {
            var scaleToken = scale.Token.AsSpan();
            var index = word.IndexOf(scaleToken, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var scaleRemainder = word;
            var scaledValue = 0L;
            var branchIsValid = true;
            var consumedScale = false;

            while ((index = scaleRemainder.IndexOf(scaleToken, StringComparison.Ordinal)) >= 0)
            {
                if (remainingWork-- <= 0)
                {
                    remainingWork = -1;
                    value = default;
                    return false;
                }

                var factorWords = scaleRemainder[..index];
                var factor = 1L;
                if (!factorWords.IsEmpty && !TryParseCardinal(factorWords, depth + 1, ref remainingWork, out factor))
                {
                    if (remainingWork < 0)
                    {
                        value = default;
                        return false;
                    }

                    branchIsValid = consumedScale;
                    break;
                }

                try
                {
                    scaledValue = checked(scaledValue + checked(factor * scale.Value));
                }
                catch (OverflowException)
                {
                    value = default;
                    return false;
                }

                scaleRemainder = scaleRemainder[(index + scaleToken.Length)..];
                consumedScale = true;
            }

            if (!branchIsValid)
            {
                continue;
            }

            if (!TryParseOptional(scaleRemainder, depth + 1, ref remainingWork, out var remainder))
            {
                if (remainingWork < 0)
                {
                    value = default;
                    return false;
                }

                continue;
            }

            try
            {
                value = checked(scaledValue + remainder);
                return true;
            }
            catch (OverflowException)
            {
                value = default;
                return false;
            }
        }

        // Prefix rules only accept 1..9 as their suffix because the prefix already contributes the
        // tens portion. Allowing larger suffix parses here would double-count scales or decades
        // that the earlier branches are responsible for.
        foreach (var rule in profile.PrefixedTens)
        {
            var prefix = rule.Prefix.AsSpan();
            if (word.StartsWith(prefix, StringComparison.Ordinal) &&
                TryParseCardinal(word[prefix.Length..], depth + 1, ref remainingWork, out var suffixValue) &&
                suffixValue is >= 1 and <= 9)
            {
                value = rule.BaseValue + suffixValue;
                return true;
            }

            if (remainingWork < 0)
            {
                value = default;
                return false;
            }
        }

        // Tens prefixes are last for the same reason: after exact tokens, scales, and explicit
        // prefixed forms have had their chance, the remaining valid suffix is a simple unit value.
        foreach (var tens in profile.TensMap)
        {
            var tensToken = tens.Key.AsSpan();
            if (!word.StartsWith(tensToken, StringComparison.Ordinal))
            {
                continue;
            }

            var remainder = word[tensToken.Length..];
            if (remainder.IsEmpty)
            {
                value = tens.Value;
                return true;
            }

            if (TryParseCardinal(remainder, depth + 1, ref remainingWork, out var unitValue) && unitValue is >= 1 and <= 9)
            {
                value = tens.Value + unitValue;
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
    /// Parses an optional remainder after a scale or prefix token.
    /// </summary>
    /// <param name="word">The remainder to parse.</param>
    /// <param name="depth">The current recursion depth.</param>
    /// <param name="remainingWork">The shared parse-tree budget.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the remainder is empty or parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseOptional(ReadOnlySpan<char> word, int depth, ref long remainingWork, out long value)
    {
        if (word.IsEmpty)
        {
            value = 0;
            return true;
        }

        return TryParseCardinal(word, depth, ref remainingWork, out value);
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

    /// <summary>
    /// Normalizes punctuation, apostrophes, casing, and repeated whitespace before parsing.
    /// </summary>
    static string Normalize(string words) =>
        Regex.Replace(RemoveDiacritics(words)
                .Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("’", string.Empty)
                .Replace("'", string.Empty)
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    /// <summary>
    /// Removes compound separators so glued scale and tens tokens can be matched as one word.
    /// </summary>
    static string CollapseCompoundSeparators(string words) =>
        words.Replace("-", string.Empty)
            .Replace(" ", string.Empty);

    /// <summary>
    /// Removes non-spacing marks from the supplied text.
    /// </summary>
    static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(text.Length);

        foreach (var ch in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(ch);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}

/// <summary>
/// Immutable locale data used by <see cref="PrefixedTensScaleWordsToNumberConverter"/>.
/// </summary>
sealed class PrefixedTensScaleWordsToNumberProfile(
    FrozenDictionary<string, long> cardinalMap,
    FrozenDictionary<string, long> tensMap,
    PrefixedScaleWord[] scales,
    PrefixedTensRule[] prefixedTens,
    string[] negativePrefixes)
{
    /// <summary>
    /// Gets the token-to-value map used by the parser.
    /// </summary>
    public FrozenDictionary<string, long> CardinalMap { get; } = cardinalMap;
    /// <summary>
    /// Gets the tens tokens and their numeric values.
    /// </summary>
    public FrozenDictionary<string, long> TensMap { get; } = tensMap;
    /// <summary>
    /// Gets the scale words that can appear inside prefixed compounds.
    /// </summary>
    public PrefixedScaleWord[] Scales { get; } = scales;
    /// <summary>
    /// Gets the prefixes that add a fixed base value to the following unit token.
    /// </summary>
    public PrefixedTensRule[] PrefixedTens { get; } = prefixedTens;
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
}

/// <summary>
/// Represents a scale token that may appear as part of a glued prefix compound.
/// </summary>
readonly record struct PrefixedScaleWord(string Token, long Value);

/// <summary>
/// Represents a locale-specific prefix that yields a base value when followed by a unit token.
/// </summary>
readonly record struct PrefixedTensRule(string Prefix, long BaseValue);