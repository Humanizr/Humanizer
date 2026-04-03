using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer;

/// <summary>
/// Parses languages that place scale words after the quantity they modify.
/// </summary>
internal class SuffixScaleWordsToNumberConverter(SuffixScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
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
            parsedLong = -parsedLong;
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
        long total = 0;
        unrecognizedWord = null;
        var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
        string? pendingToken = null;

        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var token))
        {
            if (!TryParseCompound(token, out var segmentValue))
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
                    // Keep the intermediate arithmetic in long so large suffix-scale values can be
                    // validated before the final int cast.
                    total += segmentValue * scaleValue;
                    continue;
                }

                pendingToken = scaleToken;
            }

            total += segmentValue;
        }

        value = total;
        return true;
    }

    /// <summary>
    /// Parses a single token or glued compound that may itself contain scale words.
    /// </summary>
    /// <param name="words">The token to parse.</param>
    /// <param name="value">When this method returns, the parsed long value.</param>
    /// <returns><c>true</c> if the token was parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseCompound(string words, out long value)
    {
        if (profile.CardinalMap.TryGetValue(words, out value) || profile.BareScaleMap.TryGetValue(words, out value))
        {
            return true;
        }

        // The compound parser walks from the strongest shape to the weakest: scale words first,
        // then hundred/tens/teen suffixes. That keeps a word like "hundredthousand" from being
        // misread as a smaller suffix chain.
        foreach (var scale in profile.Scales)
        {
            if (words.StartsWith(scale.Singular, StringComparison.Ordinal))
            {
                var remainder = words[scale.Singular.Length..];
                if (TryParseOptional(remainder, out var remainderValue))
                {
                    value = scale.Value + remainderValue;
                    return true;
                }
            }

            var pluralIndex = words.IndexOf(scale.Plural, StringComparison.Ordinal);
            if (pluralIndex > 0)
            {
                var left = words[..pluralIndex];
                var right = words[(pluralIndex + scale.Plural.Length)..];

                if (TryParseCompound(left, out var multiplier) &&
                    TryParseOptional(right, out var remainderValue))
                {
                    value = multiplier * scale.Value + remainderValue;
                    return true;
                }
            }
        }

        if (words.StartsWith(profile.HundredSingularToken, StringComparison.Ordinal))
        {
            var remainder = words[profile.HundredSingularToken.Length..];
            if (TryParseOptional(remainder, out var remainderValue))
            {
                value = 100 + remainderValue;
                return true;
            }
        }

        var hundredPluralIndex = words.IndexOf(profile.HundredPluralToken, StringComparison.Ordinal);
        if (hundredPluralIndex > 0)
        {
            var left = words[..hundredPluralIndex];
            var right = words[(hundredPluralIndex + profile.HundredPluralToken.Length)..];

            if (TryParseCompound(left, out var multiplier) &&
                multiplier is >= 2 and <= 9 &&
                TryParseOptional(right, out var remainderValue))
            {
                // The plural hundred form only permits a small multiplier; larger values would
                // belong to the earlier scale branch and should not be accepted here.
                value = multiplier * 100 + remainderValue;
                return true;
            }
        }

        var tensIndex = words.IndexOf(profile.TensSuffixToken, StringComparison.Ordinal);
        if (tensIndex > 0)
        {
            var left = words[..tensIndex];
            var right = words[(tensIndex + profile.TensSuffixToken.Length)..];

            if (TryParseCompound(left, out var multiplier) &&
                multiplier is >= 2 and <= 9 &&
                TryParseOptional(right, out var remainderValue))
            {
                // Tens compounds follow the same multiplier rule as hundreds, but they are
                // checked later so the stronger scale and hundred forms can win first.
                value = multiplier * 10 + remainderValue;
                return true;
            }
        }

        var teensIndex = words.IndexOf(profile.TeenSuffixToken, StringComparison.Ordinal);
        if (teensIndex > 0)
        {
            var left = words[..teensIndex];
            if (TryParseCompound(left, out var unit) && unit is >= 1 and <= 9)
            {
                // Teen compounds are the narrowest suffix form, so only a single unit digit is
                // accepted to avoid swallowing broader cardinal phrases.
                value = 10 + unit;
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Parses an optional remainder following a compound token.
    /// </summary>
    /// <param name="words">The remainder to parse.</param>
    /// <param name="value">When this method returns, the parsed long value.</param>
    /// <returns><c>true</c> if the remainder is empty or parsed successfully; otherwise, <c>false</c>.</returns>
    bool TryParseOptional(string words, out long value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        return TryParseCompound(words, out value);
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
