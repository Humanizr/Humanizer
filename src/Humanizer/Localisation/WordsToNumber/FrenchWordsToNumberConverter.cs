namespace Humanizer;

internal class FrenchWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["zéro"] = 0,
        ["un"] = 1,
        ["une"] = 1,
        ["deux"] = 2,
        ["trois"] = 3,
        ["quatre"] = 4,
        ["cinq"] = 5,
        ["six"] = 6,
        ["sept"] = 7,
        ["huit"] = 8,
        ["neuf"] = 9,
        ["dix"] = 10,
        ["onze"] = 11,
        ["douze"] = 12,
        ["treize"] = 13,
        ["quatorze"] = 14,
        ["quinze"] = 15,
        ["seize"] = 16,
        ["vingt"] = 20,
        ["trente"] = 30,
        ["quarante"] = 40,
        ["cinquante"] = 50,
        ["soixante"] = 60,
        ["cent"] = 100,
        ["cents"] = 100,
        ["mille"] = 1000,
        ["million"] = 1_000_000,
        ["millions"] = 1_000_000,
        ["milliard"] = 1_000_000_000,
        ["milliards"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> OrdinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["premier"] = 1,
        ["première"] = 1
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

        var normalized = Normalize(words);
        var negative = false;

        if (normalized.StartsWith("moins ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["moins ".Length..].Trim();
        }

        if (OrdinalMap.TryGetValue(normalized, out parsedValue) ||
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

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var tokens = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            if (token == "et")
            {
                continue;
            }

            if (token == "quatre" &&
                i + 1 < tokens.Length &&
                tokens[i + 1] is "vingt" or "vingts")
            {
                current += 80;
                i++;
                continue;
            }

            if (token is "dix" && current is 60 or 80 && i + 1 < tokens.Length &&
                CardinalMap.TryGetValue(tokens[i + 1], out var teenPart) &&
                teenPart is >= 1 and <= 9)
            {
                current += 10 + teenPart;
                i++;
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
}
