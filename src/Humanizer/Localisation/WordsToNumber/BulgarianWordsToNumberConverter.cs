using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Humanizer;

internal class BulgarianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, long> CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
    {
        ["нула"] = 0,
        ["един"] = 1,
        ["една"] = 1,
        ["едно"] = 1,
        ["два"] = 2,
        ["две"] = 2,
        ["три"] = 3,
        ["четири"] = 4,
        ["пет"] = 5,
        ["шест"] = 6,
        ["седем"] = 7,
        ["осем"] = 8,
        ["девет"] = 9,
        ["десет"] = 10,
        ["единадесет"] = 11,
        ["дванадесет"] = 12,
        ["дванайсет"] = 12,
        ["тринадесет"] = 13,
        ["тринайсет"] = 13,
        ["четиринадесет"] = 14,
        ["четиринайсет"] = 14,
        ["петнадесет"] = 15,
        ["петнайсет"] = 15,
        ["шестнадесет"] = 16,
        ["шестнайсет"] = 16,
        ["седемнадесет"] = 17,
        ["седемнайсет"] = 17,
        ["осемнадесет"] = 18,
        ["осемнайсет"] = 18,
        ["деветнадесет"] = 19,
        ["деветнайсет"] = 19,
        ["двадесет"] = 20,
        ["двайсет"] = 20,
        ["тридесет"] = 30,
        ["трийсет"] = 30,
        ["четиридесет"] = 40,
        ["петдесет"] = 50,
        ["шестдесет"] = 60,
        ["седемдесет"] = 70,
        ["осемдесет"] = 80,
        ["деветдесет"] = 90,
        ["сто"] = 100,
        ["двеста"] = 200,
        ["триста"] = 300,
        ["четиристотин"] = 400,
        ["петстотин"] = 500,
        ["шестстотин"] = 600,
        ["седемстотин"] = 700,
        ["осемстотин"] = 800,
        ["деветстотин"] = 900
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, long> ScaleMap = new Dictionary<string, long>(StringComparer.Ordinal)
    {
        ["хиляда"] = 1_000,
        ["хиляди"] = 1_000,
        ["милион"] = 1_000_000,
        ["милиона"] = 1_000_000,
        ["милиард"] = 1_000_000_000,
        ["милиарда"] = 1_000_000_000,
        ["трилион"] = 1_000_000_000_000,
        ["трилиона"] = 1_000_000_000_000,
        ["квадрилион"] = 1_000_000_000_000_000,
        ["квадрилиона"] = 1_000_000_000_000_000,
        ["квинтилион"] = 1_000_000_000_000_000_000,
        ["квинтилиона"] = 1_000_000_000_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

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

        if (normalized.StartsWith("минус ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["минус ".Length..].Trim();
        }

        if (!TryParseCardinal(normalized, out var parsedLong, out unrecognizedWord))
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
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", " ")
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static bool TryParseCardinal(string words, out long value, out string? unrecognizedWord)
    {
        long total = 0;
        long current = 0;
        unrecognizedWord = null;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (token == "и")
            {
                continue;
            }

            if (ScaleMap.TryGetValue(token, out var scale))
            {
                total += (current == 0 ? 1 : current) * scale;
                current = 0;
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            current += numeric;
        }

        value = total + current;
        return true;
    }
}
