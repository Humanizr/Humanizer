namespace Humanizer;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal class ItalianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["zero"] = 0,
        ["uno"] = 1,
        ["una"] = 1,
        ["un"] = 1,
        ["due"] = 2,
        ["tre"] = 3,
        ["quattro"] = 4,
        ["cinque"] = 5,
        ["sei"] = 6,
        ["sette"] = 7,
        ["otto"] = 8,
        ["nove"] = 9,
        ["dieci"] = 10,
        ["undici"] = 11,
        ["dodici"] = 12,
        ["tredici"] = 13,
        ["quattordici"] = 14,
        ["quindici"] = 15,
        ["sedici"] = 16,
        ["diciassette"] = 17,
        ["diciotto"] = 18,
        ["diciannove"] = 19,
        ["venti"] = 20,
        ["ventuno"] = 21,
        ["ventotto"] = 28,
        ["trenta"] = 30,
        ["trentuno"] = 31,
        ["trentotto"] = 38,
        ["quaranta"] = 40,
        ["quarantuno"] = 41,
        ["quarantotto"] = 48,
        ["cinquanta"] = 50,
        ["cinquantuno"] = 51,
        ["cinquantotto"] = 58,
        ["sessanta"] = 60,
        ["sessantuno"] = 61,
        ["sessantotto"] = 68,
        ["settanta"] = 70,
        ["settantuno"] = 71,
        ["settantotto"] = 78,
        ["ottanta"] = 80,
        ["ottantuno"] = 81,
        ["ottantotto"] = 88,
        ["novanta"] = 90,
        ["novantuno"] = 91,
        ["novantotto"] = 98,
        ["cento"] = 100,
        ["mille"] = 1000,
        ["mila"] = 1000,
        ["milione"] = 1_000_000,
        ["milioni"] = 1_000_000,
        ["miliardo"] = 1_000_000_000,
        ["miliardi"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly string[] CardinalTokenOrder = CardinalMap.Keys
        .OrderByDescending(key => key.Length)
        .ToArray();

    static readonly FrozenDictionary<string, int> OrdinalMap = BuildOrdinalMap();
    static readonly Regex OrdinalAbbreviationRegex = new(@"^(?<number>\d+)(?:º|°|ª|o|a)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedNumberWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedNumberWord}");
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

        var trimmed = words.Trim();
        var normalized = Normalize(trimmed);
        var negative = false;

        if (normalized.StartsWith("meno ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["meno ".Length..].Trim();
        }
        else if (normalized == "meno")
        {
            negative = true;
            normalized = string.Empty;
        }

        if (TryParseOrdinalAbbreviation(trimmed, out parsedValue) ||
            OrdinalMap.TryGetValue(normalized, out parsedValue) ||
            TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
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

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        value = default;
        unrecognizedWord = null;

        var total = 0;
        var current = 0;
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
                var start = position;
                while (position < words.Length && !char.IsWhiteSpace(words[position]))
                {
                    position++;
                }

                unrecognizedWord = words[start..position];
                return false;
            }

            if (token == "e" || token == "ed")
            {
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                unrecognizedWord = token;
                return false;
            }

            if (numeric == 100)
            {
                current = (current == 0 ? 1 : current) * numeric;
            }
            else if (numeric >= 1000)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
            }
            else
            {
                current += numeric;
            }
        }

        value = total + current;
        return true;
    }

    static bool TryParseOrdinalAbbreviation(string words, out int value)
    {
        var match = OrdinalAbbreviationRegex.Match(words.Trim());
        if (match.Success && int.TryParse(match.Groups["number"].Value, out value))
        {
            return true;
        }

        value = default;
        return false;
    }

    static bool TryReadToken(string words, ref int position, out string token)
    {
        foreach (var candidate in CardinalTokenOrder)
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

    static string Normalize(string words) =>
        Regex.Replace(RemoveDiacritics(words)
                .Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("'", string.Empty)
                .Replace("’", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant(),
            @"\s+",
            " ")
        .Trim();

    static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(text.Length);

        foreach (var ch in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (category != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(ch);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new ItalianNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            var masculine = Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Masculine));
            if (!ordinals.ContainsKey(masculine))
            {
                ordinals[masculine] = number;
            }

            var feminine = Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine));
            if (!ordinals.ContainsKey(feminine))
            {
                ordinals[feminine] = number;
            }
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}
