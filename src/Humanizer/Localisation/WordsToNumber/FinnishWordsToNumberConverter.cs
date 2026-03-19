using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer;

internal class FinnishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, long> CardinalMap = BuildDictionary(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["nolla"] = 0,
            ["yksi"] = 1,
            ["kaksi"] = 2,
            ["kolme"] = 3,
            ["neljä"] = 4,
            ["viisi"] = 5,
            ["kuusi"] = 6,
            ["seitsemän"] = 7,
            ["kahdeksan"] = 8,
            ["yhdeksän"] = 9,
            ["kymmenen"] = 10,
            ["yksitoista"] = 11,
            ["kaksitoista"] = 12,
            ["kolmetoista"] = 13,
            ["neljätoista"] = 14,
            ["viisitoista"] = 15,
            ["kuusitoista"] = 16,
            ["seitsemäntoista"] = 17,
            ["kahdeksantoista"] = 18,
            ["yhdeksäntoista"] = 19
        });

    static readonly FrozenDictionary<string, long> BareScaleMap = BuildDictionary(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["sata"] = 100,
            ["tuhat"] = 1_000,
            ["tuhatta"] = 1_000,
            ["miljoona"] = 1_000_000,
            ["miljoonaa"] = 1_000_000,
            ["miljardi"] = 1_000_000_000,
            ["miljardia"] = 1_000_000_000
        });

    static readonly (string Singular, string Plural, long Value)[] Scales =
    [
        ("miljardi", "miljardia", 1_000_000_000),
        ("miljoona", "miljoonaa", 1_000_000),
        ("tuhat", "tuhatta", 1_000)
    ];

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

        if (normalized.StartsWith("miinus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["miinus ".Length..].Trim();
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

    static string Normalize(string words) =>
        Regex.Replace(RemoveDiacritics(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant())
                .Trim(),
            @"\s+",
            " ");

    static bool TryParsePhrase(string words, out long value, out string? unrecognizedWord)
    {
        var tokens = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        long total = 0;
        unrecognizedWord = null;

        for (var i = 0; i < tokens.Length; i++)
        {
            if (!TryParseCompound(tokens[i], out var segmentValue))
            {
                value = default;
                unrecognizedWord = tokens[i];
                return false;
            }

            if (segmentValue < 1000 &&
                i + 1 < tokens.Length &&
                BareScaleMap.TryGetValue(tokens[i + 1], out var scaleValue) &&
                scaleValue >= 1_000)
            {
                total += segmentValue * scaleValue;
                i++;
                continue;
            }

            total += segmentValue;
        }

        value = total;
        return true;
    }

    static bool TryParseCompound(string words, out long value)
    {
        if (CardinalMap.TryGetValue(words, out value) || BareScaleMap.TryGetValue(words, out value))
        {
            return true;
        }

        foreach (var (singular, plural, scaleValue) in Scales)
        {
            if (words.StartsWith(singular, StringComparison.Ordinal))
            {
                var remainder = words[singular.Length..];
                if (TryParseOptional(remainder, out var remainderValue))
                {
                    value = scaleValue + remainderValue;
                    return true;
                }
            }

            var pluralIndex = words.IndexOf(plural, StringComparison.Ordinal);
            if (pluralIndex > 0)
            {
                var left = words[..pluralIndex];
                var right = words[(pluralIndex + plural.Length)..];

                if (TryParseCompound(left, out var multiplier) &&
                    TryParseOptional(right, out var remainderValue))
                {
                    value = multiplier * scaleValue + remainderValue;
                    return true;
                }
            }
        }

        if (words.StartsWith("sata", StringComparison.Ordinal))
        {
            var remainder = words["sata".Length..];
            if (TryParseOptional(remainder, out var remainderValue))
            {
                value = 100 + remainderValue;
                return true;
            }
        }

        var hundredsIndex = words.IndexOf("sataa", StringComparison.Ordinal);
        if (hundredsIndex > 0)
        {
            var left = words[..hundredsIndex];
            var right = words[(hundredsIndex + "sataa".Length)..];

            if (TryParseCompound(left, out var multiplier) &&
                multiplier is >= 2 and <= 9 &&
                TryParseOptional(right, out var remainderValue))
            {
                value = multiplier * 100 + remainderValue;
                return true;
            }
        }

        var tensIndex = words.IndexOf("kymmentä", StringComparison.Ordinal);
        if (tensIndex > 0)
        {
            var left = words[..tensIndex];
            var right = words[(tensIndex + "kymmentä".Length)..];

            if (TryParseCompound(left, out var multiplier) &&
                multiplier is >= 2 and <= 9 &&
                TryParseOptional(right, out var remainderValue))
            {
                value = multiplier * 10 + remainderValue;
                return true;
            }
        }

        var teensIndex = words.IndexOf("toista", StringComparison.Ordinal);
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

    static bool TryParseOptional(string words, out long value)
    {
        if (string.IsNullOrEmpty(words))
        {
            value = 0;
            return true;
        }

        return TryParseCompound(words, out value);
    }

    static FrozenDictionary<string, long> BuildDictionary(Dictionary<string, long> source)
    {
        var result = new Dictionary<string, long>(StringComparer.Ordinal);

        foreach (var pair in source)
        {
            result[pair.Key] = pair.Value;
            result[RemoveDiacritics(pair.Key)] = pair.Value;
        }

        return result.ToFrozenDictionary(StringComparer.Ordinal);
    }

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
