namespace Humanizer;

using System.Collections.Generic;
using System.Text.RegularExpressions;

internal partial class SpanishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["cero"] = 0,
        ["uno"] = 1,
        ["una"] = 1,
        ["un"] = 1,
        ["dos"] = 2,
        ["tres"] = 3,
        ["cuatro"] = 4,
        ["cinco"] = 5,
        ["seis"] = 6,
        ["siete"] = 7,
        ["ocho"] = 8,
        ["nueve"] = 9,
        ["diez"] = 10,
        ["once"] = 11,
        ["doce"] = 12,
        ["trece"] = 13,
        ["catorce"] = 14,
        ["quince"] = 15,
        ["dieciséis"] = 16,
        ["dieciseis"] = 16,
        ["diecisiete"] = 17,
        ["dieciocho"] = 18,
        ["diecinueve"] = 19,
        ["veinte"] = 20,
        ["veintiuno"] = 21,
        ["veintiún"] = 21,
        ["veintiun"] = 21,
        ["veintiuna"] = 21,
        ["veintidós"] = 22,
        ["veintidos"] = 22,
        ["veintitrés"] = 23,
        ["veintitres"] = 23,
        ["veinticuatro"] = 24,
        ["veinticinco"] = 25,
        ["veintiséis"] = 26,
        ["veintiseis"] = 26,
        ["veintisiete"] = 27,
        ["veintiocho"] = 28,
        ["veintinueve"] = 29,
        ["treinta"] = 30,
        ["cuarenta"] = 40,
        ["cincuenta"] = 50,
        ["sesenta"] = 60,
        ["setenta"] = 70,
        ["ochenta"] = 80,
        ["noventa"] = 90,
        ["cien"] = 100,
        ["ciento"] = 100,
        ["doscientos"] = 200,
        ["doscientas"] = 200,
        ["trescientos"] = 300,
        ["trescientas"] = 300,
        ["cuatrocientos"] = 400,
        ["cuatrocientas"] = 400,
        ["quinientos"] = 500,
        ["quinientas"] = 500,
        ["seiscientos"] = 600,
        ["seiscientas"] = 600,
        ["setecientos"] = 700,
        ["setecientas"] = 700,
        ["ochocientos"] = 800,
        ["ochocientas"] = 800,
        ["novecientos"] = 900,
        ["novecientas"] = 900,
        ["mil"] = 1_000,
        ["millón"] = 1_000_000,
        ["millon"] = 1_000_000,
        ["millones"] = 1_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> OrdinalMap = BuildOrdinalMap();

#if NET7_0_OR_GREATER
    [GeneratedRegex(@"^(?<number>\d+)(?:\.?)(?<suffix>º|ª)$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex OrdinalAbbreviationRegexGenerated();

    private static Regex OrdinalAbbreviationRegex() => OrdinalAbbreviationRegexGenerated();
#else
    private static readonly Regex OrdinalAbbreviationRegexField = new(@"^(?<number>\d+)(?:\.?)(?<suffix>º|ª)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static Regex OrdinalAbbreviationRegex() => OrdinalAbbreviationRegexField;
#endif

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

        if (normalized.StartsWith("menos ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["menos ".Length..].Trim();
        }

        if (TryParseOrdinalAbbreviation(normalized, out parsedValue) ||
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

    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static bool TryParseOrdinalAbbreviation(string words, out int value)
    {
        var match = OrdinalAbbreviationRegex().Match(words);
        if (match.Success && int.TryParse(match.Groups["number"].Value, out value))
        {
            return true;
        }

        value = default;
        return false;
    }

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var current = 0;
        unrecognizedWord = null;
        var tokenizer = WordsToNumberTokenizer.Enumerate(words).GetEnumerator();
        string? pendingToken = null;

        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var token))
        {
            if (token == "y")
            {
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (numeric >= 1000)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
            }
            else if (numeric == 100)
            {
                current = (current == 0 ? 1 : current) * numeric;
            }
            else
            {
                current += numeric;
            }
        }

        value = total + current;
        return true;
    }

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new SpanishNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            AddOrdinal(ordinals, converter.ConvertToOrdinal(number, GrammaticalGender.Masculine), number);
            AddOrdinal(ordinals, converter.ConvertToOrdinal(number, GrammaticalGender.Feminine), number);
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);

        static void AddOrdinal(Dictionary<string, int> map, string ordinal, int number)
        {
            var normalized = Normalize(ordinal);
            if (!string.IsNullOrEmpty(normalized))
            {
                map[normalized] = number;
            }
        }
    }
}
