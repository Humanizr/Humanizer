namespace Humanizer;

internal partial class CatalanWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["zero"] = 0,
        ["un"] = 1,
        ["u"] = 1,
        ["una"] = 1,
        ["dos"] = 2,
        ["dues"] = 2,
        ["tres"] = 3,
        ["quatre"] = 4,
        ["cinc"] = 5,
        ["sis"] = 6,
        ["set"] = 7,
        ["vuit"] = 8,
        ["nou"] = 9,
        ["deu"] = 10,
        ["onze"] = 11,
        ["dotze"] = 12,
        ["tretze"] = 13,
        ["catorze"] = 14,
        ["quinze"] = 15,
        ["setze"] = 16,
        ["disset"] = 17,
        ["divuit"] = 18,
        ["dinou"] = 19,
        ["vint"] = 20,
        ["trenta"] = 30,
        ["quaranta"] = 40,
        ["cinquanta"] = 50,
        ["seixanta"] = 60,
        ["setanta"] = 70,
        ["vuitanta"] = 80,
        ["noranta"] = 90,
        ["cent"] = 100,
        ["cents"] = 100,
        ["centes"] = 100,
        ["mil"] = 1000,
        ["milió"] = 1_000_000,
        ["milions"] = 1_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> OrdinalMap = BuildOrdinalMap();

#if NET7_0_OR_GREATER
    [GeneratedRegex(@"^(?<number>\d+)(?<suffix>r|n|a|è)$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex OrdinalAbbreviationRegexGenerated();

    private static Regex OrdinalAbbreviationRegex() => OrdinalAbbreviationRegexGenerated();
#else
    private static readonly Regex OrdinalAbbreviationRegexField = new(@"^(?<number>\d+)(?<suffix>r|n|a|è)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

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

        if (normalized.StartsWith("menys ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["menys ".Length..].Trim();
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
        var tokens = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var token in tokens)
        {
            if (token == "i")
            {
                continue;
            }

            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
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

    static FrozenDictionary<string, int> BuildOrdinalMap()
    {
        var converter = new CatalanNumberToWordsConverter();
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            ordinals[converter.ConvertToOrdinal(number, GrammaticalGender.Masculine).ToLowerInvariant()] = number;
            ordinals[converter.ConvertToOrdinal(number, GrammaticalGender.Feminine).ToLowerInvariant()] = number;
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }
}
