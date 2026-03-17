namespace Humanizer;

internal class FilipinoWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> Cardinals = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["sero"] = 0,
        ["isa"] = 1,
        ["dalawa"] = 2,
        ["tatlo"] = 3,
        ["apat"] = 4,
        ["lima"] = 5,
        ["anim"] = 6,
        ["pito"] = 7,
        ["walo"] = 8,
        ["siyam"] = 9,
        ["sampu"] = 10,
        ["labing"] = 10,
        ["dalawampu"] = 20,
        ["tatlumpu"] = 30,
        ["apatnapu"] = 40,
        ["limampu"] = 50,
        ["animnapu"] = 60,
        ["pitumpu"] = 70,
        ["walumpu"] = 80,
        ["siyamnapu"] = 90,
        ["daan"] = 100,
        ["libo"] = 1000,
        ["milyon"] = 1_000_000,
        ["bilyon"] = 1_000_000_000
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

        if (normalized.StartsWith("minus ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized["minus ".Length..].Trim();
        }

        if (TryParseCardinal(normalized, out parsedValue))
        {
            if (negative)
            {
                parsedValue = -parsedValue;
            }

            unrecognizedWord = null;
            return true;
        }

        unrecognizedWord = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? normalized;
        parsedValue = default;
        return false;
    }

    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .Replace("'", string.Empty)
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static bool TryParseCardinal(string words, out int value)
    {
        if (Cardinals.TryGetValue(words, out value))
        {
            return true;
        }

        var tokens = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var total = 0;
        var current = 0;

        foreach (var token in tokens)
        {
            if (token == "at")
            {
                continue;
            }

            if (token.StartsWith("labing", StringComparison.Ordinal) && token.Length > "labing".Length)
            {
                var unitToken = token["labing".Length..];
                if (TryParseCardinal(unitToken, out var teenUnit))
                {
                    current += 10 + teenUnit;
                    continue;
                }
            }

            if (token.EndsWith("ng", StringComparison.Ordinal))
            {
                var root = token[..^2];
                if (TryParseCardinal(root, out var linkedUnit))
                {
                    current += linkedUnit;
                    continue;
                }
            }

            if (token.EndsWith('t'))
            {
                var root = token[..^1];
                if (Cardinals.TryGetValue(root, out var linkedTens))
                {
                    current += linkedTens;
                    continue;
                }
            }

            if (!Cardinals.TryGetValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (tokenValue >= 1000)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
            }
            else if (tokenValue == 100)
            {
                current = (current == 0 ? 1 : current) * tokenValue;
            }
            else
            {
                current += tokenValue;
            }
        }

        value = total + current;
        return true;
    }
}
