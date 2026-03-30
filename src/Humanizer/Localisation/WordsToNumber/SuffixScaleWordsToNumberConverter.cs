using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer;

internal class SuffixScaleWordsToNumberConverter(SuffixScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly SuffixScaleWordsToNumberProfile profile = profile;

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    public override bool TryConvert(string words, out int parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        if (int.TryParse(words.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
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

        if (parsedLong is > int.MaxValue or < int.MinValue)
        {
            parsedValue = default;
            unrecognizedWord = normalized;
            return false;
        }

        parsedValue = (int)parsedLong;
        unrecognizedWord = null;
        return true;
    }

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

            if (segmentValue < 1000 &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var scaleToken))
            {
                if (profile.BareScaleMap.TryGetValue(scaleToken, out var scaleValue) && scaleValue >= 1_000)
                {
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

    bool TryParseCompound(string words, out long value)
    {
        if (profile.CardinalMap.TryGetValue(words, out value) || profile.BareScaleMap.TryGetValue(words, out value))
        {
            return true;
        }

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
                value = 10 + unit;
                return true;
            }
        }

        value = default;
        return false;
    }

    bool TryParseOptional(string words, out long value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        return TryParseCompound(words, out value);
    }

    static string Normalize(string words) =>
        Regex.Replace(RemoveDiacritics(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant())
                .Trim(),
            @"\s+",
            " ");

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
    public FrozenDictionary<string, long> CardinalMap { get; } = cardinalMap;
    public FrozenDictionary<string, long> BareScaleMap { get; } = bareScaleMap;
    public SuffixScaleWord[] Scales { get; } = scales;
    public string HundredSingularToken { get; } = hundredSingularToken;
    public string HundredPluralToken { get; } = hundredPluralToken;
    public string TensSuffixToken { get; } = tensSuffixToken;
    public string TeenSuffixToken { get; } = teenSuffixToken;
    public string[] NegativePrefixes { get; } = negativePrefixes;
}

readonly record struct SuffixScaleWord(string Singular, string Plural, long Value);
