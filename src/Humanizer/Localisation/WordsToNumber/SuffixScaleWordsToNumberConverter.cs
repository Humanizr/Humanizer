namespace Humanizer;

/// <summary>
/// Parses languages that place scale words after the quantity they modify.
/// </summary>
internal class SuffixScaleWordsToNumberConverter(SuffixScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    const int MaximumParseDepth = 128;
    const int ParseWorkPerCharacter = 32;
    // Short ambiguous compounds need room to preserve the parser's historical backtracking order.
    const int MinimumParseWork = 1_000_000;
    readonly SuffixScaleWordsToNumberProfile profile = profile;

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

        if (long.TryParse(words.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
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
            normalized = normalized[prefix.Length..].Trim();
            break;
        }

        if (!TryParsePhrase(normalized, out var parsedLong, out unrecognizedWord))
        {
            parsedValue = default;
            return false;
        }

        if (negative)
        {
            try
            {
                parsedLong = checked(-parsedLong);
            }
            catch (OverflowException)
            {
                parsedValue = default;
                unrecognizedWord = normalized;
                return false;
            }
        }

        parsedValue = parsedLong;
        unrecognizedWord = null;
        return true;
    }

    /// <summary>
    /// Parses a whitespace-delimited phrase that may contain suffix-based scale compounds.
    /// </summary>
    /// <param name="words">A normalized phrase ready for tokenization.</param>
    /// <param name="value">When this method returns, the parsed long value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token that was not recognized.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParsePhrase(string words, out long value, out string? unrecognizedWord)
    {
        var remainingWork = Math.Max((long)words.Length * ParseWorkPerCharacter, MinimumParseWork);
        long total = 0;
        unrecognizedWord = null;
        var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
        string? pendingToken = null;

        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var token))
        {
            if (!TryParseOptional(token, 0, ref remainingWork, out var segmentValue))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            // Suffix-scale locales often express "three hundred million" as a segment followed by a
            // bare scale token. The lookahead keeps that scale attached to the segment instead of
            // treating it as a separate additive word.
            if (segmentValue < 1000 &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var scaleToken))
            {
                if (profile.BareScaleMap.TryGetValue(scaleToken, out var scaleValue) && scaleValue >= 1_000)
                {
                    try
                    {
                        total = checked(total + checked(segmentValue * scaleValue));
                    }
                    catch (OverflowException)
                    {
                        value = default;
                        unrecognizedWord = scaleToken;
                        return false;
                    }

                    continue;
                }

                pendingToken = scaleToken;
            }

            try
            {
                total = checked(total + segmentValue);
            }
            catch (OverflowException)
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }
        }

        value = total;
        return true;
    }

    /// <summary>
    /// Parses a single token or glued compound that may itself contain scale words.
    /// </summary>
    /// <param name="words">The token to parse.</param>
    /// <param name="depth">The current recursion depth.</param>
    /// <param name="remainingWork">The shared parse-tree budget.</param>
    /// <param name="value">When this method returns, the parsed long value.</param>
    /// <returns><c>true</c> if the token was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCompound(string words, int depth, ref long remainingWork, out long value)
    {
        if (profile.CardinalMap.TryGetValue(words, out value) || profile.BareScaleMap.TryGetValue(words, out value))
        {
            return true;
        }

        return TryParseCompound(words.AsSpan(), depth, ref remainingWork, out value);
    }

    /// <summary>
    /// Parses a compound span while bounding nested shapes and sharing work across the parse tree.
    /// </summary>
    /// <param name="words">The remaining compound to parse.</param>
    /// <param name="depth">The current recursion depth.</param>
    /// <param name="remainingWork">The shared parse-tree budget; a negative value stops alternative branches.</param>
    /// <param name="value">When this method returns, the parsed long value.</param>
    /// <returns><c>true</c> if the compound was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCompound(ReadOnlySpan<char> words, int depth, ref long remainingWork, out long value)
    {
        if (TryGetValue(profile.CardinalMap, words, out value) || TryGetValue(profile.BareScaleMap, words, out value))
        {
            return true;
        }

        if (words.Length > MaximumParseDepth && TryParseLongAdditiveChain(words, ref remainingWork, out value))
        {
            return true;
        }

        if (words.IsEmpty || depth >= MaximumParseDepth || remainingWork-- <= 0)
        {
            if (!words.IsEmpty)
            {
                remainingWork = -1;
            }

            value = default;
            return false;
        }

        foreach (var scale in profile.Scales)
        {
            var singular = scale.Singular.AsSpan();
            if (words.StartsWith(singular, StringComparison.Ordinal) &&
                TryParseOptional(words[singular.Length..], depth + 1, ref remainingWork, out var remainderValue))
            {
                return TryAdd(scale.Value, remainderValue, ref remainingWork, out value);
            }

            if (remainingWork < 0)
            {
                value = default;
                return false;
            }

            var plural = scale.Plural.AsSpan();
            var pluralIndex = IndexOf(words, plural, ref remainingWork);
            if (pluralIndex > 0 &&
                TryParseCompound(words[..pluralIndex], depth + 1, ref remainingWork, out var multiplier) &&
                TryParseOptional(words[(pluralIndex + plural.Length)..], depth + 1, ref remainingWork, out remainderValue))
            {
                return TryMultiplyAdd(multiplier, scale.Value, remainderValue, ref remainingWork, out value);
            }

            if (remainingWork < 0)
            {
                value = default;
                return false;
            }
        }

        var hundredSingular = profile.HundredSingularToken.AsSpan();
        if (words.StartsWith(hundredSingular, StringComparison.Ordinal) &&
            TryParseOptional(words[hundredSingular.Length..], depth + 1, ref remainingWork, out var hundredRemainder))
        {
            return TryAdd(100, hundredRemainder, ref remainingWork, out value);
        }

        if (remainingWork < 0)
        {
            value = default;
            return false;
        }

        var hundredPlural = profile.HundredPluralToken.AsSpan();
        var hundredPluralIndex = IndexOf(words, hundredPlural, ref remainingWork);
        if (hundredPluralIndex > 0 &&
            TryParseCompound(words[..hundredPluralIndex], depth + 1, ref remainingWork, out var hundredMultiplier) &&
            hundredMultiplier is >= 2 and <= 9 &&
            TryParseOptional(words[(hundredPluralIndex + hundredPlural.Length)..], depth + 1, ref remainingWork, out hundredRemainder))
        {
            return TryMultiplyAdd(hundredMultiplier, 100, hundredRemainder, ref remainingWork, out value);
        }

        if (remainingWork < 0)
        {
            value = default;
            return false;
        }

        var tensSuffix = profile.TensSuffixToken.AsSpan();
        var tensIndex = IndexOf(words, tensSuffix, ref remainingWork);
        if (tensIndex > 0 &&
            TryParseCompound(words[..tensIndex], depth + 1, ref remainingWork, out var tensMultiplier) &&
            tensMultiplier is >= 2 and <= 9 &&
            TryParseOptional(words[(tensIndex + tensSuffix.Length)..], depth + 1, ref remainingWork, out var tensRemainder))
        {
            return TryMultiplyAdd(tensMultiplier, 10, tensRemainder, ref remainingWork, out value);
        }

        if (remainingWork < 0)
        {
            value = default;
            return false;
        }

        var teenSuffix = profile.TeenSuffixToken.AsSpan();
        var teensIndex = IndexOf(words, teenSuffix, ref remainingWork);
        if (teensIndex > 0 &&
            TryParseCompound(words[..teensIndex], depth + 1, ref remainingWork, out var unit) &&
            unit is >= 1 and <= 9)
        {
            return TryAdd(10, unit, ref remainingWork, out value);
        }

        value = default;
        return false;
    }

    bool TryParseLongAdditiveChain(ReadOnlySpan<char> words, ref long remainingWork, out long value)
    {
        remainingWork -= words.Length;
        if (remainingWork < 0)
        {
            remainingWork = -1;
            value = default;
            return false;
        }

        var remainder = words;
        var lowestScaleRank = -1;
        var lastAtomRank = -1;
        var lastAtomValue = 0L;
        var valueBeforeLastAtom = 0L;
        var lastAtomWasSingularScale = false;
        value = 0;

        while (!remainder.IsEmpty)
        {
            if (TryGetValue(profile.CardinalMap, remainder, out var terminal))
            {
                return TryAdd(value, terminal, ref remainingWork, out value);
            }

            if (TryConsumeBareScale(remainder, out var bareScale, out var bareLength, out var bareRank))
            {
                var bareRemainder = remainder[bareLength..];
                if (bareRank < lowestScaleRank)
                {
                    if (!TryMultiplyAdd(value, bareScale, 0, ref remainingWork, out value))
                    {
                        return false;
                    }

                    valueBeforeLastAtom = 0;
                    lastAtomValue = value;
                }
                else if (bareRank == lowestScaleRank && lastAtomRank == bareRank && !bareRemainder.IsEmpty)
                {
                    if (!lastAtomWasSingularScale)
                    {
                        value = default;
                        return false;
                    }

                    if (!TryMultiplyAdd(lastAtomValue, bareScale, valueBeforeLastAtom, ref remainingWork, out value))
                    {
                        return false;
                    }

                    lastAtomValue = value - valueBeforeLastAtom;
                }
                else
                {
                    if (lowestScaleRank < 0)
                    {
                        value = default;
                        return false;
                    }

                    valueBeforeLastAtom = value;
                    if (!TryAdd(value, bareScale, ref remainingWork, out value))
                    {
                        return false;
                    }

                    lastAtomValue = bareScale;
                }

                lowestScaleRank = bareRank;
                lastAtomRank = bareRank;
                lastAtomWasSingularScale = false;
                remainder = bareRemainder;
                continue;
            }

            if (TryConsumeScalePromotion(
                    remainder,
                    lowestScaleRank,
                    out var promotedScale,
                    out var promotedMultiplier,
                    out var promotedLength,
                    out var promotedRank))
            {
                if (!TryAdd(value, promotedMultiplier, ref remainingWork, out var promotedValue) ||
                    !TryMultiplyAdd(promotedValue, promotedScale, 0, ref remainingWork, out value))
                {
                    return false;
                }

                lowestScaleRank = promotedRank;
                lastAtomRank = promotedRank;
                lastAtomValue = value;
                valueBeforeLastAtom = 0;
                lastAtomWasSingularScale = false;
                remainder = remainder[promotedLength..];
                continue;
            }

            if (TryConsumeSimpleSuffix(remainder, out var suffixValue, out var consumedLength, out var suffixRank))
            {
                if (suffixRank < lowestScaleRank)
                {
                    value = default;
                    return false;
                }

                valueBeforeLastAtom = value;
                if (!TryAdd(value, suffixValue, ref remainingWork, out value))
                {
                    value = default;
                    return false;
                }

                lowestScaleRank = suffixRank;
                lastAtomRank = suffixRank;
                lastAtomValue = suffixValue;
                lastAtomWasSingularScale = false;
                remainder = remainder[consumedLength..];
                continue;
            }

            var consumed = false;
            for (var scaleIndex = 0; scaleIndex < profile.Scales.Length; scaleIndex++)
            {
                var scale = profile.Scales[scaleIndex];
                if (!remainder.StartsWith(scale.Singular, StringComparison.Ordinal))
                {
                    continue;
                }

                valueBeforeLastAtom = value;
                if (!TryAdd(value, scale.Value, ref remainingWork, out value))
                {
                    return false;
                }

                lowestScaleRank = Math.Max(lowestScaleRank, scaleIndex);
                lastAtomRank = scaleIndex;
                lastAtomValue = scale.Value;
                lastAtomWasSingularScale = true;
                remainder = remainder[scale.Singular.Length..];
                consumed = true;
                break;
            }

            if (consumed)
            {
                continue;
            }

            if (remainder.StartsWith(profile.HundredSingularToken, StringComparison.Ordinal))
            {
                valueBeforeLastAtom = value;
                if (!TryAdd(value, 100, ref remainingWork, out value))
                {
                    return false;
                }

                lowestScaleRank = profile.Scales.Length;
                lastAtomRank = profile.Scales.Length;
                lastAtomValue = 100;
                lastAtomWasSingularScale = false;
                remainder = remainder[profile.HundredSingularToken.Length..];
                continue;
            }

            value = default;
            return false;
        }

        return true;
    }

    bool TryConsumeBareScale(ReadOnlySpan<char> words, out long value, out int consumedLength, out int scaleRank)
    {
        for (var index = 0; index < profile.Scales.Length; index++)
        {
            var scale = profile.Scales[index];
            if (!words.StartsWith(scale.Plural, StringComparison.Ordinal))
            {
                continue;
            }

            value = scale.Value;
            consumedLength = scale.Plural.Length;
            scaleRank = index;
            return true;
        }

        value = default;
        consumedLength = default;
        scaleRank = default;
        return false;
    }

    bool TryConsumeScalePromotion(
        ReadOnlySpan<char> words,
        int lowestScaleRank,
        out long scaleValue,
        out long multiplier,
        out int consumedLength,
        out int scaleRank)
    {
        for (var index = 0; index < lowestScaleRank; index++)
        {
            var scale = profile.Scales[index];
            foreach (var candidate in profile.CardinalMap)
            {
                if (words.StartsWith(candidate.Key, StringComparison.Ordinal) &&
                    words[candidate.Key.Length..].StartsWith(scale.Plural, StringComparison.Ordinal))
                {
                    scaleValue = scale.Value;
                    multiplier = candidate.Value;
                    consumedLength = candidate.Key.Length + scale.Plural.Length;
                    scaleRank = index;
                    return true;
                }
            }
        }

        scaleValue = default;
        multiplier = default;
        consumedLength = default;
        scaleRank = default;
        return false;
    }

    bool TryConsumeSimpleSuffix(ReadOnlySpan<char> words, out long value, out int consumedLength, out int scaleRank)
    {
        for (var scaleIndex = 0; scaleIndex < profile.Scales.Length; scaleIndex++)
        {
            var scale = profile.Scales[scaleIndex];
            foreach (var multiplier in profile.CardinalMap)
            {
                if (TryConsumeSimpleSuffix(words, multiplier.Key, scale.Plural, multiplier.Value, scale.Value, out value, out consumedLength))
                {
                    scaleRank = scaleIndex;
                    return true;
                }
            }
        }

        foreach (var multiplier in profile.CardinalMap)
        {
            if (multiplier.Value is >= 2 and <= 9 &&
                TryConsumeSimpleSuffix(words, multiplier.Key, profile.HundredPluralToken, multiplier.Value, 100, out value, out consumedLength))
            {
                scaleRank = profile.Scales.Length;
                return true;
            }

            if (multiplier.Value is >= 2 and <= 9 &&
                TryConsumeSimpleSuffix(words, multiplier.Key, profile.TensSuffixToken, multiplier.Value, 10, out value, out consumedLength))
            {
                scaleRank = profile.Scales.Length + 1;
                return true;
            }
        }

        value = default;
        consumedLength = 0;
        scaleRank = default;
        return false;
    }

    static bool TryConsumeSimpleSuffix(
        ReadOnlySpan<char> words,
        ReadOnlySpan<char> multiplierToken,
        ReadOnlySpan<char> suffixToken,
        long multiplier,
        long suffixValue,
        out long value,
        out int consumedLength)
    {
        if (!words.StartsWith(multiplierToken, StringComparison.Ordinal))
        {
            value = default;
            consumedLength = 0;
            return false;
        }

        var suffixStart = multiplierToken.Length;
        if (!words[suffixStart..].StartsWith(suffixToken, StringComparison.Ordinal))
        {
            value = default;
            consumedLength = 0;
            return false;
        }

        try
        {
            value = checked(multiplier * suffixValue);
            consumedLength = suffixStart + suffixToken.Length;
            return true;
        }
        catch (OverflowException)
        {
            value = default;
            consumedLength = 0;
            return false;
        }
    }

    static int IndexOf(ReadOnlySpan<char> words, ReadOnlySpan<char> suffix, ref long remainingWork)
    {
        var index = words.IndexOf(suffix, StringComparison.Ordinal);
        remainingWork -= index < 0 ? words.Length : index + suffix.Length;
        if (remainingWork >= 0)
        {
            return index;
        }

        remainingWork = -1;
        return -1;
    }

    static bool TryAdd(long left, long right, out long value)
    {
        try
        {
            value = checked(left + right);
            return true;
        }
        catch (OverflowException)
        {
            value = default;
            return false;
        }
    }

    static bool TryAdd(long left, long right, ref long remainingWork, out long value)
    {
        if (TryAdd(left, right, out value))
        {
            return true;
        }

        remainingWork = -1;
        return false;
    }

    static bool TryMultiplyAdd(long multiplier, long scale, long remainder, ref long remainingWork, out long value)
    {
        try
        {
            value = checked(checked(multiplier * scale) + remainder);
            return true;
        }
        catch (OverflowException)
        {
            remainingWork = -1;
            value = default;
            return false;
        }
    }

    bool TryParseOptional(ReadOnlySpan<char> words, int depth, ref long remainingWork, out long value)
    {
        if (words.IsEmpty)
        {
            value = 0;
            return true;
        }

        return TryParseCompound(words, depth, ref remainingWork, out value);
    }

    /// <summary>
    /// Parses an optional remainder following a compound token.
    /// </summary>
    /// <param name="words">The remainder to parse.</param>
    /// <param name="depth">The current recursion depth.</param>
    /// <param name="remainingWork">The shared parse-tree budget.</param>
    /// <param name="value">When this method returns, the parsed long value.</param>
    /// <returns><c>true</c> if the remainder is empty or parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseOptional(string words, int depth, ref long remainingWork, out long value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        return TryParseCompound(words, depth, ref remainingWork, out value);
    }

    static bool TryGetValue(FrozenDictionary<string, long> values, ReadOnlySpan<char> words, out long value)
    {
        foreach (var candidate in values)
        {
            if (words.Equals(candidate.Key.AsSpan(), StringComparison.Ordinal))
            {
                value = candidate.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Normalizes punctuation, hyphenation, casing, and repeated whitespace before parsing.
    /// </summary>
    static string Normalize(string words) =>
        Regex.Replace(RemoveDiacritics(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant())
                .Trim(),
            @"\s+",
            " ");

    /// <summary>
    /// Removes non-spacing marks from the supplied text.
    /// </summary>
    static string RemoveDiacritics(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}

/// <summary>
/// Immutable locale data used by <see cref="SuffixScaleWordsToNumberConverter"/>.
/// </summary>
sealed class SuffixScaleWordsToNumberProfile(
    FrozenDictionary<string, long> cardinalMap,
    FrozenDictionary<string, long> bareScaleMap,
    SuffixScaleWord[] scales,
    string hundredSingularToken,
    string hundredPluralToken,
    string tensSuffixToken,
    string teenSuffixToken,
    string[] negativePrefixes)
{
    /// <summary>
    /// Gets the token-to-value map used for ordinary cardinals.
    /// </summary>
    public FrozenDictionary<string, long> CardinalMap { get; } = cardinalMap;
    /// <summary>
    /// Gets the scale tokens that can stand on their own when the phrase omits a multiplier.
    /// </summary>
    public FrozenDictionary<string, long> BareScaleMap { get; } = bareScaleMap;
    /// <summary>
    /// Gets the suffix-scale definitions that drive compound parsing.
    /// </summary>
    public SuffixScaleWord[] Scales { get; } = scales;
    /// <summary>
    /// Gets the singular token used for "hundred".
    /// </summary>
    public string HundredSingularToken { get; } = hundredSingularToken;
    /// <summary>
    /// Gets the plural token used for "hundred".
    /// </summary>
    public string HundredPluralToken { get; } = hundredPluralToken;
    /// <summary>
    /// Gets the suffix token used to express tens compounds.
    /// </summary>
    public string TensSuffixToken { get; } = tensSuffixToken;
    /// <summary>
    /// Gets the suffix token used to express teen compounds.
    /// </summary>
    public string TeenSuffixToken { get; } = teenSuffixToken;
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; } = negativePrefixes;
}

/// <summary>
/// Represents a scale word in both singular and plural forms.
/// </summary>
readonly record struct SuffixScaleWord(string Singular, string Plural, long Value);