using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer;

internal class PrefixedTensScaleWordsToNumberConverter(PrefixedTensScaleWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly PrefixedTensScaleWordsToNumberProfile profile = profile;

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

        if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue))
        {
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

    bool TryParseCardinal(string word, out int value)
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

        foreach (var scale in profile.Scales)
        {
            var index = word.IndexOf(scale.Token, StringComparison.Ordinal);
            if (index < 0)
            {
                continue;
            }

            var left = word[..index];
            var right = word[(index + scale.Token.Length)..];
            var factor = 1;

            if (!string.IsNullOrEmpty(left) && !TryParseCardinal(left, out factor))
            {
                continue;
            }

            if (!TryParseOptional(right, out var remainder))
            {
                continue;
            }

            value = checked(factor * scale.Value + remainder);
            return true;
        }

        foreach (var rule in profile.PrefixRules)
        {
            if (word.StartsWith(rule.Prefix, StringComparison.Ordinal) &&
                TryParseCardinal(word[rule.Prefix.Length..], out var suffixValue) &&
                suffixValue is >= 1 and <= 9)
            {
                value = rule.BaseValue + suffixValue;
                return true;
            }
        }

        foreach (var tens in profile.TensMap)
        {
            if (!word.StartsWith(tens.Key, StringComparison.Ordinal))
            {
                continue;
            }

            var remainder = word[tens.Key.Length..];
            if (string.IsNullOrEmpty(remainder))
            {
                value = tens.Value;
                return true;
            }

            if (TryParseCardinal(remainder, out var unitValue) && unitValue is >= 1 and <= 9)
            {
                value = tens.Value + unitValue;
                return true;
            }
        }

        value = default;
        return false;
    }

    bool TryParseOptional(string word, out int value)
    {
        if (string.IsNullOrEmpty(word))
        {
            value = 0;
            return true;
        }

        return TryParseCardinal(word, out value);
    }

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

    static string CollapseCompoundSeparators(string words) =>
        words.Replace("-", string.Empty)
            .Replace(" ", string.Empty);

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

sealed class PrefixedTensScaleWordsToNumberProfile(
    FrozenDictionary<string, int> cardinalMap,
    FrozenDictionary<string, int> tensMap,
    PrefixedScaleWord[] scales,
    PrefixedTensRule[] prefixRules,
    string[] negativePrefixes)
{
    public FrozenDictionary<string, int> CardinalMap { get; } = cardinalMap;
    public FrozenDictionary<string, int> TensMap { get; } = tensMap;
    public PrefixedScaleWord[] Scales { get; } = scales;
    public PrefixedTensRule[] PrefixRules { get; } = prefixRules;
    public string[] NegativePrefixes { get; } = negativePrefixes;
}

readonly record struct PrefixedScaleWord(string Token, int Value);

readonly record struct PrefixedTensRule(string Prefix, int BaseValue);
