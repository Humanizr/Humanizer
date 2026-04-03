namespace Humanizer;

/// <summary>
/// Parses localized number words using a configurable token map and ordinal rules.
/// </summary>
/// <remarks>
/// The parser works in phases: normalize the input, strip sign and ordinal affixes, then try the
/// locale's ordinal grammar before falling back to a left-to-right cardinal reducer with
/// lookahead. That ordering is what lets the same locale data handle exact ordinals, glued
/// compounds, and scale words without one branch shadowing the others.
/// </remarks>
internal class TokenMapWordsToNumberConverter(TokenMapWordsToNumberRules rules) : GenderlessWordsToNumberConverter
{
    readonly TokenMapWordsToNumberRules rules = rules;
    readonly FrozenDictionary<string, long>? exactOrdinalMap = rules.ExactOrdinalMap;
    readonly FrozenDictionary<string, long>? ordinalScaleMap = rules.OrdinalScaleMap;
    readonly FrozenDictionary<string, long>? gluedOrdinalScaleSuffixes = rules.GluedOrdinalScaleSuffixes;

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

        if (rules.AllowInvariantIntegerInput &&
            long.TryParse(words.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        var normalized = TokenMapWordsToNumberNormalizer.Normalize(words, rules.NormalizationProfile);
        var negative = false;

        foreach (var negativePrefix in rules.NegativePrefixes)
        {
            if (!normalized.StartsWith(negativePrefix, StringComparison.Ordinal))
            {
                continue;
            }

            negative = true;
            normalized = normalized[negativePrefix.Length..].Trim();
            break;
        }

        foreach (var ordinalPrefix in rules.OrdinalPrefixes)
        {
            if (!normalized.StartsWith(ordinalPrefix, StringComparison.Ordinal))
            {
                continue;
            }

            normalized = normalized[ordinalPrefix.Length..].Trim();
            break;
        }

        if (!negative)
        {
            foreach (var negativeSuffix in rules.NegativeSuffixes)
            {
                if (!normalized.EndsWith(negativeSuffix, StringComparison.Ordinal))
                {
                    continue;
                }

                negative = true;
                normalized = normalized[..^negativeSuffix.Length].Trim();
                break;
            }
        }

        if (TryParseOrdinal(normalized, out var value) ||
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
    /// Parses an ordinal phrase using abbreviation handling, exact ordinal lookup, and ordinal
    /// scale rules.
    /// </summary>
    /// <param name="words">A normalized ordinal phrase.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was parsed as an ordinal; otherwise, <c>false</c>.</returns>
    bool TryParseOrdinal(string words, out long value)
    {
        if (TryParseOrdinalAbbreviation(words, out value))
        {
            return true;
        }

        return ordinalScaleMap is null
            ? TryParseExactOrdinal(words, out value)
            : TryParseExtendedOrdinal(words, ordinalScaleMap, out value);
    }

    /// <summary>
    /// Parses an exact ordinal string or a glued ordinal-scale suffix.
    /// </summary>
    /// <param name="words">The normalized ordinal phrase.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was recognized; otherwise, <c>false</c>.</returns>
    bool TryParseExactOrdinal(string words, out long value)
    {
        if (exactOrdinalMap?.TryGetValue(words, out value) == true)
        {
            return true;
        }

        return TryParseGluedOrdinalScale(words, out value);
    }

    /// <summary>
    /// Parses an exact ordinal string, an ordinal scale value, or a glued ordinal-scale suffix.
    /// </summary>
    /// <param name="words">The normalized ordinal phrase.</param>
    /// <param name="ordinalScaleMap">The ordinal scale lookup table.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase was recognized; otherwise, <c>false</c>.</returns>
    bool TryParseExtendedOrdinal(string words, FrozenDictionary<string, long> ordinalScaleMap, out long value)
    {
        if (exactOrdinalMap?.TryGetValue(words, out value) == true)
        {
            return true;
        }

        if (ordinalScaleMap.TryGetValue(words, out var scaleValue))
        {
            value = scaleValue;
            return true;
        }

        return TryParseGluedOrdinalScale(words, out value);
    }

    /// <summary>
    /// Parses ordinal abbreviations such as <c>21st</c> when the locale supports them.
    /// </summary>
    /// <param name="words">The normalized ordinal phrase.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the text is a supported ordinal abbreviation; otherwise, <c>false</c>.</returns>
    bool TryParseOrdinalAbbreviation(string words, out long value)
    {
        if (rules.OrdinalAbbreviationSuffixes.Length == 0)
        {
            value = default;
            return false;
        }

        var span = words.AsSpan();
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
        foreach (var candidate in rules.OrdinalAbbreviationSuffixes)
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
    /// Parses a cardinal phrase using token values, scale rules, ordinal terminals, and lookahead
    /// compounds.
    /// </summary>
    /// <param name="words">A normalized cardinal phrase.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token that was not recognized.</param>
    /// <returns><c>true</c> if the phrase was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCardinal(string words, out long value, out string? unrecognizedWord)
    {
        try
        {
            // Exact phrase matches win before tokenization so locale data can preserve irregular or
            // ambiguous spellings that would otherwise be decomposed into the wrong grammar branch.
            if (rules.CardinalMap.TryGetValue(words, out var directValue))
            {
                value = directValue;
                unrecognizedWord = null;
                return true;
            }

            long total = 0;
            long current = 0;
            unrecognizedWord = null;
            var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
            string? pendingToken = null;

            // TokenMap locales are intentionally resolved in a strict order:
            // 1. terminal ordinals, which must only win when they finish the phrase
            // 2. composite scale pairs, which combine adjacent tokens before cardinal lookup
            // 3. direct token values, including suffix-stripped variants
            // 4. lookahead compounds such as "two" + "hundred" or "two" + "teen"
            // 5. large scales and multiplier tokens
            //
            // This order keeps ambiguous inputs from being consumed by the wrong grammar branch.
            while (TryReadNextToken(ref tokenizer, ref pendingToken, out var token))
            {
                if (TryParseTerminalOrdinal(token, ref tokenizer, ref pendingToken, total, current, out value, out unrecognizedWord))
                {
                    return true;
                }

                if (TryGetCompositeScaleValue(token, ref tokenizer, ref pendingToken, out var compositeScaleValue))
                {
                    total = checked(total + checked((current == 0 ? 1 : current) * compositeScaleValue));
                    current = 0;
                    continue;
                }

                if (!TryGetTokenValue(token, out var tokenValue))
                {
                    value = default;
                    unrecognizedWord = token;
                    return false;
                }

                if (TryApplyLookaheadCompound(tokenValue, ref tokenizer, ref pendingToken, out var compoundValue))
                {
                    current = checked(current + compoundValue);
                    continue;
                }

                if (tokenValue >= rules.ScaleThreshold)
                {
                    // Large scales close the current group so "two hundred thousand" becomes
                    // (2 * 100) * 1000 rather than 2 * (100 * 1000) or any other accidental nesting.
                    total = checked(total + checked((current == 0 ? 1 : current) * tokenValue));
                    current = 0;
                    continue;
                }

                if (ShouldMultiplyToken(token, tokenValue))
                {
                    // Some locales encode multiplication with an explicit token rather than a scale
                    // word, so the current group is rewritten in place instead of flushed.
                    current = ApplyMultiplierToken(current, tokenValue);
                    continue;
                }

                // Anything left is additive by default. This is the final fallback so that odd token
                // shapes do not silently bypass the scale and multiplier rules above.
                current = checked(current + tokenValue);
            }

            value = checked(total + current);
            return true;
        }
        catch (OverflowException)
        {
            value = default;
            unrecognizedWord = words;
            return false;
        }
    }

    /// <summary>
    /// Attempts to parse an ordinal token only when it appears at the end of the phrase.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="tokenizer">The active tokenizer.</param>
    /// <param name="pendingToken">A token that should be returned before reading from <paramref name="tokenizer"/>.</param>
    /// <param name="total">The accumulated large-scale value.</param>
    /// <param name="current">The accumulated local group value.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token that was not recognized.</param>
    /// <returns><c>true</c> if the current token completed an ordinal parse; otherwise, <c>false</c>.</returns>
    bool TryParseTerminalOrdinal(
        string token,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        long total,
        long current,
        out long value,
        out string? unrecognizedWord)
    {
        if (!rules.AllowTerminalOrdinalToken)
        {
            value = default;
            unrecognizedWord = null;
            return false;
        }

        // Terminal ordinals are only valid when nothing follows them. If a trailing token remains,
        // the same lexeme should be treated as a cardinal token so the parser can continue.
        return ordinalScaleMap is null
            ? TryParseTerminalExactOrdinal(token, exactOrdinalMap, ref tokenizer, ref pendingToken, total, current, out value, out unrecognizedWord)
            : TryParseTerminalExtendedOrdinal(token, exactOrdinalMap, ordinalScaleMap, ref tokenizer, ref pendingToken, total, current, out value, out unrecognizedWord);
    }

    /// <summary>
    /// Parses a terminal exact ordinal.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="exactOrdinalMap">The exact ordinal lookup table.</param>
    /// <param name="tokenizer">The active tokenizer.</param>
    /// <param name="pendingToken">A token that should be returned before reading from <paramref name="tokenizer"/>.</param>
    /// <param name="total">The accumulated large-scale value.</param>
    /// <param name="current">The accumulated local group value.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token that was not recognized.</param>
    /// <returns><c>true</c> if the current token completed an exact ordinal parse; otherwise, <c>false</c>.</returns>
    bool TryParseTerminalExactOrdinal(
        string token,
        FrozenDictionary<string, long>? exactOrdinalMap,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        long total,
        long current,
        out long value,
        out string? unrecognizedWord)
    {
        if (exactOrdinalMap?.TryGetValue(token, out var exactOrdinalValue) != true)
        {
            value = default;
            unrecognizedWord = null;
            return false;
        }

        // Reject a trailing exact ordinal if there is any remaining token; otherwise inputs like
        // "third million" would be misclassified before the scale parser sees the full phrase.
        if (TryReadNextToken(ref tokenizer, ref pendingToken, out var trailingToken))
        {
            pendingToken = trailingToken;
            value = default;
            unrecognizedWord = null;
            return false;
        }

        try
        {
            value = checked(total + current + exactOrdinalValue);
            unrecognizedWord = null;
            return true;
        }
        catch (OverflowException)
        {
            value = default;
            unrecognizedWord = token;
            return false;
        }
    }

    /// <summary>
    /// Parses a terminal exact ordinal or ordinal-scale token when extended ordinal rules are available.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="exactOrdinalMap">The exact ordinal lookup table.</param>
    /// <param name="ordinalScaleMap">The ordinal scale lookup table.</param>
    /// <param name="tokenizer">The active tokenizer.</param>
    /// <param name="pendingToken">A token that should be returned before reading from <paramref name="tokenizer"/>.</param>
    /// <param name="total">The accumulated large-scale value.</param>
    /// <param name="current">The accumulated local group value.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <param name="unrecognizedWord">When parsing fails, the token that was not recognized.</param>
    /// <returns><c>true</c> if the current token completed an ordinal parse; otherwise, <c>false</c>.</returns>
    bool TryParseTerminalExtendedOrdinal(
        string token,
        FrozenDictionary<string, long>? exactOrdinalMap,
        FrozenDictionary<string, long> ordinalScaleMap,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        long total,
        long current,
        out long value,
        out string? unrecognizedWord)
    {
        var ordinalValue = default(long);
        var ordinalScaleValue = default(long);
        var isExactOrdinal = exactOrdinalMap?.TryGetValue(token, out ordinalValue) == true;
        var isOrdinalScale = ordinalScaleMap.TryGetValue(token, out ordinalScaleValue);
        if (!isExactOrdinal && !isOrdinalScale)
        {
            value = default;
            unrecognizedWord = null;
            return false;
        }

        // The extended ordinal path still requires phrase termination; otherwise the current token
        // is just a candidate and the cardinal parser must keep consuming.
        if (TryReadNextToken(ref tokenizer, ref pendingToken, out var trailingToken))
        {
            pendingToken = trailingToken;
            value = default;
            unrecognizedWord = null;
            return false;
        }

        try
        {
            value = isOrdinalScale
                ? checked(total + checked((current == 0 ? 1 : current) * ordinalScaleValue))
                : checked(total + current + ordinalValue);

            unrecognizedWord = null;
            return true;
        }
        catch (OverflowException)
        {
            value = default;
            unrecognizedWord = token;
            return false;
        }
    }

    /// <summary>
    /// Parses a glued ordinal scale such as a cardinal stem followed by a scale suffix.
    /// </summary>
    /// <param name="words">The normalized ordinal phrase.</param>
    /// <param name="value">When this method returns, the parsed numeric value.</param>
    /// <returns><c>true</c> if the phrase matched a glued ordinal scale; otherwise, <c>false</c>.</returns>
    bool TryParseGluedOrdinalScale(string words, out long value)
    {
        if (gluedOrdinalScaleSuffixes is null || gluedOrdinalScaleSuffixes.Count == 0)
        {
            value = default;
            return false;
        }

        foreach (var suffix in gluedOrdinalScaleSuffixes)
        {
            if (!words.EndsWith(suffix.Key, StringComparison.Ordinal) || words.Length == suffix.Key.Length)
            {
                continue;
            }

            if (!TryParseCardinal(words[..^suffix.Key.Length], out value, out _))
            {
                continue;
            }

            try
            {
                value = checked(value * suffix.Value);
                return true;
            }
            catch (OverflowException)
            {
                value = default;
                return false;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Returns <c>true</c> when a token is configured to be ignored during parsing.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <returns><c>true</c> if the token should be skipped; otherwise, <c>false</c>.</returns>
    bool ShouldIgnore(string token)
    {
        foreach (var ignoredToken in rules.IgnoredTokens)
        {
            if (token == ignoredToken)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns <c>true</c> when a token should multiply the current group rather than add to it.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <param name="tokenValue">The numeric value associated with the token.</param>
    /// <returns><c>true</c> if the token should multiply the current group; otherwise, <c>false</c>.</returns>
    bool ShouldMultiplyToken(string token, long tokenValue)
    {
        if (rules.UseHundredMultiplier && tokenValue == 100)
        {
            return true;
        }

        foreach (var multiplierToken in rules.MultiplierTokens)
        {
            if (token == multiplierToken)
            {
                return true;
            }
        }

        return false;
    }

    long ApplyMultiplierToken(long current, long tokenValue)
    {
        if (current == 0)
        {
            return tokenValue;
        }

        if (tokenValue == 10 &&
            current >= 100 &&
            TryGetUnitTokenValue(current % 100, out var trailingUnit))
        {
            // Locales such as Vietnamese build "hundred + unit + tens-suffix" compounds where the
            // tens token only multiplies the trailing unit rather than the whole accumulated group.
            return checked(current - trailingUnit + trailingUnit * tokenValue);
        }

        return checked(current * tokenValue);
    }

    /// <summary>
    /// Tries to resolve a scale value from two adjacent tokens.
    /// </summary>
    /// <param name="token">The current token.</param>
    /// <param name="tokenizer">The active tokenizer.</param>
    /// <param name="pendingToken">A token that should be returned before reading from <paramref name="tokenizer"/>.</param>
    /// <param name="tokenValue">When this method returns, the combined scale value.</param>
    /// <returns><c>true</c> if the token pair forms a composite scale; otherwise, <c>false</c>.</returns>
    bool TryGetCompositeScaleValue(
        string token,
        ref WordsToNumberTokenizer.Enumerator tokenizer,
        ref string? pendingToken,
        out long tokenValue)
    {
        if (rules.CompositeScaleMap is null ||
            rules.CompositeScaleMap.Count == 0 ||
            !TryReadNextToken(ref tokenizer, ref pendingToken, out var nextToken))
        {
            tokenValue = default;
            return false;
        }

        // Composite scales are resolved before direct token lookup so phrases such as
        // "million billion" can collapse into a single grammar symbol instead of being parsed as
        // two unrelated magnitudes.
        if (rules.CompositeScaleMap.TryGetValue(token + " " + nextToken, out tokenValue))
        {
            return true;
        }

        pendingToken = nextToken;
        tokenValue = default;
        return false;
    }

    /// <summary>
    /// Uses lookahead to turn a unit token plus a suffix token into a compound value.
    /// </summary>
    /// <param name="tokenValue">The numeric value of the current token.</param>
    /// <param name="tokenizer">The active tokenizer.</param>
    /// <param name="pendingToken">A token that should be returned before reading from <paramref name="tokenizer"/>.</param>
    /// <param name="compoundValue">When this method returns, the combined compound value.</param>
    /// <returns><c>true</c> if the token pair formed a recognized compound; otherwise, <c>false</c>.</returns>
    bool TryApplyLookaheadCompound(long tokenValue, ref WordsToNumberTokenizer.Enumerator tokenizer, ref string? pendingToken, out long compoundValue)
    {
        if (!TryGetUnitTokenValue(tokenValue, out var unitValue) ||
            !TryReadNextToken(ref tokenizer, ref pendingToken, out var nextToken))
        {
            compoundValue = default;
            return false;
        }

        // A unit token only becomes a compound when the next token confirms the grammar.
        // Otherwise the lookahead token is pushed back so the normal cardinal flow can see it.
        if (IsMatch(rules.TeenSuffixTokens, nextToken))
        {
            compoundValue = rules.TeenBaseValue + unitValue;
            return true;
        }

        // The hundred-suffix range is sentinel-gated in the rule object. If the locale never
        // supplies a real min/max pair, this comparison intentionally fails and the parser falls
        // back to ordinary cardinal handling.
        if (unitValue >= rules.HundredSuffixMinValue &&
            unitValue <= rules.HundredSuffixMaxValue &&
            IsMatch(rules.HundredSuffixTokens, nextToken))
        {
            compoundValue = unitValue * rules.HundredSuffixValue;
            return true;
        }

        pendingToken = nextToken;
        compoundValue = default;
        return false;
    }

    /// <summary>
    /// Reads the next token while applying token-prefix trimming and ignore rules.
    /// </summary>
    /// <param name="tokenizer">The active tokenizer.</param>
    /// <param name="pendingToken">A token that should be returned before reading from <paramref name="tokenizer"/>.</param>
    /// <param name="token">When this method returns, the next meaningful token.</param>
    /// <returns><c>true</c> if a meaningful token was produced; otherwise, <c>false</c>.</returns>
    bool TryReadNextToken(ref WordsToNumberTokenizer.Enumerator tokenizer, ref string? pendingToken, out string token)
    {
        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var rawToken))
        {
            token = NormalizeToken(rawToken);
            if (token.Length == 0 || ShouldIgnore(token))
            {
                continue;
            }

            // Normalization and ignore filtering happen before any value lookup so the rest of the
            // parser can assume every token that reaches this point is potentially meaningful.
            return true;
        }

        token = string.Empty;
        return false;
    }

    /// <summary>
    /// Removes configured leading prefixes from a token before lookup.
    /// </summary>
    /// <param name="token">The raw token to normalize.</param>
    /// <returns>The trimmed token.</returns>
    string NormalizeToken(string token)
    {
        foreach (var prefix in rules.LeadingTokenPrefixesToTrim)
        {
            if (token.StartsWith(prefix, StringComparison.Ordinal))
            {
                // Some locales prefix tokens with a grammatical marker that should not participate
                // in lookup. Remove the first matching prefix and keep the rest of the token intact.
                return token[prefix.Length..];
            }
        }

        return token;
    }

    /// <summary>
    /// Returns <c>true</c> when a token value falls within the locale's unit-token range.
    /// </summary>
    /// <param name="tokenValue">The candidate token value.</param>
    /// <param name="unitValue">When this method returns, the unit token value.</param>
    /// <returns><c>true</c> if the value is a supported unit token; otherwise, <c>false</c>.</returns>
    bool TryGetUnitTokenValue(long tokenValue, out long unitValue)
    {
        if (tokenValue >= rules.UnitTokenMinValue && tokenValue <= rules.UnitTokenMaxValue)
        {
            unitValue = tokenValue;
            return true;
        }

        unitValue = default;
        return false;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="token"/> matches any candidate string.
    /// </summary>
    /// <param name="candidates">The candidate strings to compare.</param>
    /// <param name="token">The token to inspect.</param>
    /// <returns><c>true</c> if the token matches one of the candidates; otherwise, <c>false</c>.</returns>
    static bool IsMatch(string[] candidates, string token)
    {
        foreach (var candidate in candidates)
        {
            if (token == candidate)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Tries to resolve a token from the cardinal map, including suffix-stripped variants.
    /// </summary>
    /// <param name="token">The token to inspect.</param>
    /// <param name="tokenValue">When this method returns, the resolved numeric value.</param>
    /// <returns><c>true</c> if the token was recognized; otherwise, <c>false</c>.</returns>
    bool TryGetTokenValue(string token, out long tokenValue)
    {
        if (rules.CardinalMap.TryGetValue(token, out tokenValue))
        {
            return true;
        }

        // Suffix stripping is the last resort because it can turn a valid token into a false
        // positive if applied too early. Keep the exact match fast path first.
        foreach (var suffix in rules.TokenSuffixesToStrip)
        {
            if (!token.EndsWith(suffix, StringComparison.Ordinal) || token.Length == suffix.Length)
            {
                continue;
            }

            if (rules.CardinalMap.TryGetValue(token[..^suffix.Length], out tokenValue))
            {
                return true;
            }
        }

        tokenValue = default;
        return false;
    }
}
/// <summary>
/// Immutable rule set used by <see cref="TokenMapWordsToNumberConverter"/>.
/// </summary>
/// <remarks>
/// The fields are grouped by parser phase: token lookup maps, normalization strategy, sign and
/// ordinal affixes, lookahead and suffix rules, and scalar thresholds. The parser reads this type
/// as a recipe for how a locale should be normalized and reduced, not as a single monolithic
/// grammar table.
/// </remarks>
internal sealed class TokenMapWordsToNumberRules
{
    /// <summary>
    /// Gets the primary token-to-value map used for cardinal parsing.
    /// </summary>
    public required FrozenDictionary<string, long> CardinalMap { get; init; }
    /// <summary>
    /// Gets the exact ordinal lookup map.
    /// </summary>
    public FrozenDictionary<string, long>? ExactOrdinalMap { get; init; }
    /// <summary>
    /// Gets the ordinal-scale lookup map used by locales with ordinal scale words.
    /// </summary>
    public FrozenDictionary<string, long>? OrdinalScaleMap { get; init; }
    /// <summary>
    /// Gets the suffixes that may be glued onto a cardinal stem to form an ordinal scale.
    /// </summary>
    public FrozenDictionary<string, long>? GluedOrdinalScaleSuffixes { get; init; }
    /// <summary>
    /// Gets composite scale tokens that are formed from adjacent words.
    /// </summary>
    public FrozenDictionary<string, long>? CompositeScaleMap { get; init; }
    /// <summary>
    /// Gets the normalization strategy used before parsing.
    /// </summary>
    public TokenMapNormalizationProfile NormalizationProfile { get; init; }
    /// <summary>
    /// Gets the prefixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativePrefixes { get; init; } = [];
    /// <summary>
    /// Gets the suffixes that mark a negative number phrase.
    /// </summary>
    public string[] NegativeSuffixes { get; init; } = [];
    /// <summary>
    /// Gets the prefixes that should be removed before parsing ordinals.
    /// </summary>
    public string[] OrdinalPrefixes { get; init; } = [];
    /// <summary>
    /// Gets the tokens that should be ignored during parsing.
    /// </summary>
    public string[] IgnoredTokens { get; init; } = [];
    /// <summary>
    /// Gets the leading prefixes that should be trimmed from each token.
    /// </summary>
    public string[] LeadingTokenPrefixesToTrim { get; init; } = [];
    /// <summary>
    /// Gets the tokens that should multiply the current group.
    /// </summary>
    public string[] MultiplierTokens { get; init; } = [];
    /// <summary>
    /// Gets the suffixes that should be stripped from token candidates before lookup.
    /// </summary>
    public string[] TokenSuffixesToStrip { get; init; } = [];
    /// <summary>
    /// Gets the suffixes accepted by ordinal abbreviations.
    /// </summary>
    public string[] OrdinalAbbreviationSuffixes { get; init; } = [];
    /// <summary>
    /// Gets the suffix tokens that can form teen compounds with a leading unit token.
    /// </summary>
    public string[] TeenSuffixTokens { get; init; } = [];
    /// <summary>
    /// Gets the suffix tokens that can form hundred compounds with a leading unit token.
    /// </summary>
    public string[] HundredSuffixTokens { get; init; } = [];
    /// <summary>
    /// Gets a value indicating whether a terminal ordinal token may end the phrase.
    /// </summary>
    public bool AllowTerminalOrdinalToken { get; init; }
    /// <summary>
    /// Gets a value indicating whether the numeric value 100 should multiply the current group.
    /// </summary>
    public bool UseHundredMultiplier { get; init; }
    /// <summary>
    /// Gets a value indicating whether invariant integer input is accepted directly.
    /// </summary>
    public bool AllowInvariantIntegerInput { get; init; }
    /// <summary>
    /// Gets the base value used when a unit token forms a teen compound.
    /// </summary>
    public long TeenBaseValue { get; init; } = 10;
    /// <summary>
    /// Gets the multiplier used when a unit token forms a hundred compound.
    /// </summary>
    public long HundredSuffixValue { get; init; } = 100;
    /// <summary>
    /// Gets the minimum unit-token value accepted by lookahead compounds.
    /// </summary>
    public long UnitTokenMinValue { get; init; } = 1;
    /// <summary>
    /// Gets the maximum unit-token value accepted by lookahead compounds.
    /// </summary>
    public long UnitTokenMaxValue { get; init; } = 9;
    /// <summary>
    /// Gets the lower bound used when recognizing hundred suffix compounds.
    /// The default sentinel disables the hundred-suffix path until a locale explicitly configures
    /// both bounds to a real range.
    /// </summary>
    public long HundredSuffixMinValue { get; init; } = long.MaxValue;
    /// <summary>
    /// Gets the upper bound used when recognizing hundred suffix compounds.
    /// The default sentinel disables the hundred-suffix path until a locale explicitly configures
    /// both bounds to a real range.
    /// </summary>
    public long HundredSuffixMaxValue { get; init; } = long.MinValue;
    /// <summary>
    /// Gets the value at or above which tokens are treated as large scales.
    /// </summary>
    public long ScaleThreshold { get; init; } = 1000;
}

/// <summary>
/// Describes how a token-map locale should be normalized before parsing.
/// </summary>
internal enum TokenMapNormalizationProfile
{
    /// <summary>
    /// Collapses repeated whitespace without changing case or punctuation.
    /// </summary>
    CollapseWhitespace,
    /// <summary>
    /// Lowercases text and removes periods.
    /// </summary>
    LowercaseRemovePeriods,
    /// <summary>
    /// Lowercases text and turns periods into spaces.
    /// </summary>
    LowercaseReplacePeriodsWithSpaces,
    /// <summary>
    /// Lowercases text, removes periods, and strips diacritics.
    /// </summary>
    LowercaseRemovePeriodsAndDiacritics,
    /// <summary>
    /// Converts punctuation to spaces and strips diacritics.
    /// </summary>
    PunctuationToSpacesRemoveDiacritics,
    /// <summary>
    /// Applies Persian-specific normalization rules.
    /// </summary>
    Persian
}

/// <summary>
/// Normalizes token-map input before parsing.
/// </summary>
static class TokenMapWordsToNumberNormalizer
{
    /// <summary>
    /// Normalizes a number phrase using the supplied profile.
    /// </summary>
    /// <param name="words">The text to normalize.</param>
    /// <param name="profile">The normalization profile to apply.</param>
    /// <returns>The normalized text.</returns>
    public static string Normalize(string words, TokenMapNormalizationProfile profile)
    {
        // Fast-path normalizers only allocate when the input actually needs rewriting. Profiles that
        // require broader punctuation or diacritic handling fall back to the character-by-character
        // builder below.
        return profile switch
        {
            TokenMapNormalizationProfile.CollapseWhitespace => NormalizeCollapseWhitespace(words),
            TokenMapNormalizationProfile.LowercaseRemovePeriods => NormalizeLowercaseRemovePeriods(words),
            TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces => NormalizeLowercaseReplacePeriodsWithSpaces(words),
            TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics => NormalizeWithBuilder(RemoveDiacritics(words), profile),
            TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics => NormalizeWithBuilder(RemoveDiacritics(words), profile),
            TokenMapNormalizationProfile.Persian => NormalizeWithBuilder(NormalizePersian(words), profile),
            _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, null)
        };
    }

    /// <summary>
    /// Collapses repeated whitespace without changing punctuation or case.
    /// </summary>
    static string NormalizeCollapseWhitespace(string words)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        // Scan first so already-normalized text can return a trimmed slice without building a new
        // string.
        var previousWasSpace = false;
        foreach (var current in source)
        {
            if (char.IsWhiteSpace(current))
            {
                if (previousWasSpace)
                {
                    return NormalizeWithBuilder(words, TokenMapNormalizationProfile.CollapseWhitespace);
                }

                previousWasSpace = true;
                continue;
            }

            previousWasSpace = false;
        }

        return source.Length == words.Length ? words : source.ToString();
    }

    /// <summary>
    /// Lowercases text while removing punctuation that should not affect token lookup.
    /// </summary>
    static string NormalizeLowercaseRemovePeriods(string words)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        // This profile stays allocation-free when the input is already lowercase and only trimmed
        // whitespace needs to be preserved.
        var needsNormalization = false;
        var previousWasSpace = false;

        foreach (var current in source)
        {
            if (current is ',' or '.')
            {
                needsNormalization = true;
                break;
            }

            if (current == '-')
            {
                needsNormalization = true;
                break;
            }

            if (char.IsWhiteSpace(current))
            {
                if (current != ' ' || previousWasSpace)
                {
                    needsNormalization = true;
                    break;
                }

                previousWasSpace = true;
                continue;
            }

            if (char.IsUpper(current))
            {
                needsNormalization = true;
                break;
            }

            previousWasSpace = false;
        }

        if (!needsNormalization)
        {
            return source.Length == words.Length ? words : source.ToString();
        }

        return NormalizeWithBuilder(words, TokenMapNormalizationProfile.LowercaseRemovePeriods);
    }

    /// <summary>
    /// Lowercases text while treating some punctuation as token separators.
    /// </summary>
    static string NormalizeLowercaseReplacePeriodsWithSpaces(string words)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        // Some locales treat periods and hyphens as explicit token separators, so the fast path is
        // intentionally narrower than the builder path below.
        var needsNormalization = false;
        var previousWasSpace = false;

        foreach (var current in source)
        {
            if (current == ',')
            {
                needsNormalization = true;
                break;
            }

            if (current is '.' or '-')
            {
                needsNormalization = true;
                break;
            }

            if (char.IsWhiteSpace(current))
            {
                if (current != ' ' || previousWasSpace)
                {
                    needsNormalization = true;
                    break;
                }

                previousWasSpace = true;
                continue;
            }

            if (char.IsUpper(current))
            {
                needsNormalization = true;
                break;
            }

            previousWasSpace = false;
        }

        if (!needsNormalization)
        {
            return source.Length == words.Length ? words : source.ToString();
        }

        return NormalizeWithBuilder(words, TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces);
    }

    /// <summary>
    /// Applies the selected normalization profile character by character.
    /// </summary>
    static string NormalizeWithBuilder(string words, TokenMapNormalizationProfile profile)
    {
        var source = words.AsSpan().Trim();
        if (source.IsEmpty)
        {
            return string.Empty;
        }

        // The builder path is the canonical normalizer for profiles that need punctuation folding,
        // diacritic stripping, or culture-specific replacements in one pass.
        var builder = new StringBuilder(source.Length);
        var previousWasSpace = false;

        foreach (var sourceChar in source)
        {
            var current = sourceChar;
            switch (profile)
            {
                case TokenMapNormalizationProfile.CollapseWhitespace:
                    break;
                case TokenMapNormalizationProfile.LowercaseRemovePeriods:
                    if (current == ',' || current == '.')
                    {
                        continue;
                    }

                    if (current == '-')
                    {
                        current = ' ';
                    }

                    current = char.ToLowerInvariant(current);
                    break;
                case TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces:
                    if (current == ',')
                    {
                        continue;
                    }

                    if (current is '.' or '-')
                    {
                        current = ' ';
                    }

                    current = char.ToLowerInvariant(current);
                    break;
                case TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics:
                    if (current == ',' || current == '.')
                    {
                        continue;
                    }

                    if (current == '-')
                    {
                        current = ' ';
                    }

                    current = char.ToLowerInvariant(current);
                    break;
                case TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics:
                    switch (current)
                    {
                        case ',':
                        case '.':
                        case ':':
                        case ';':
                        case '"':
                        case '\'':
                        case '״':
                        case '׳':
                        case '-':
                        case '־':
                        case '/':
                        case '\\':
                        case '(':
                        case ')':
                        case '[':
                        case ']':
                        case '{':
                        case '}':
                            current = ' ';
                            break;
                    }
                    break;
                case TokenMapNormalizationProfile.Persian:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(profile), profile, null);
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
    /// Applies the Persian-specific normalization rules.
    /// </summary>
    static string NormalizePersian(string words) =>
        words.Replace(",", string.Empty)
             .Replace(".", string.Empty)
             .Replace("،", string.Empty)
             .Replace("\u200c", " ")
             .Replace('ي', 'ی')
             .Replace('ك', 'ک');

    /// <summary>
    /// Removes non-spacing marks from the supplied text.
    /// </summary>
    /// <param name="text">The text to normalize.</param>
    /// <returns>The input text without combining marks.</returns>
    internal static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(text.Length);

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
