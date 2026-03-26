namespace Humanizer;

internal class SlovenianWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nič"] = 0,
        ["nic"] = 0,
        ["en"] = 1,
        ["ena"] = 1,
        ["eno"] = 1,
        ["dva"] = 2,
        ["dve"] = 2,
        ["tri"] = 3,
        ["štiri"] = 4,
        ["stiri"] = 4,
        ["pet"] = 5,
        ["šest"] = 6,
        ["sest"] = 6,
        ["sedem"] = 7,
        ["osem"] = 8,
        ["devet"] = 9,
        ["deset"] = 10,
        ["enajst"] = 11,
        ["dvanajst"] = 12,
        ["trinajst"] = 13,
        ["štirinajst"] = 14,
        ["stirinajst"] = 14,
        ["petnajst"] = 15,
        ["šestnajst"] = 16,
        ["sestnajst"] = 16,
        ["sedemnajst"] = 17,
        ["osemnajst"] = 18,
        ["devetnajst"] = 19,
        ["dvajset"] = 20,
        ["trideset"] = 30,
        ["štirideset"] = 40,
        ["stirideset"] = 40,
        ["petdeset"] = 50,
        ["šestdeset"] = 60,
        ["sestdeset"] = 60,
        ["sedemdeset"] = 70,
        ["osemdeset"] = 80,
        ["devetdeset"] = 90,
        ["sto"] = 100,
        ["dvesto"] = 200,
        ["tristo"] = 300,
        ["štiristo"] = 400,
        ["stiristo"] = 400,
        ["petsto"] = 500,
        ["šeststo"] = 600,
        ["seststo"] = 600,
        ["sedemsto"] = 700,
        ["osemsto"] = 800,
        ["devetsto"] = 900,
        ["tisoč"] = 1_000,
        ["tisoc"] = 1_000,
        ["milijon"] = 1_000_000,
        ["milijona"] = 1_000_000,
        ["milijonov"] = 1_000_000,
        ["milijarda"] = 1_000_000_000,
        ["milijardi"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> UnitPrefixes = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["en"] = 1,
        ["ena"] = 1,
        ["dva"] = 2,
        ["tri"] = 3,
        ["štiri"] = 4,
        ["stiri"] = 4,
        ["pet"] = 5,
        ["šest"] = 6,
        ["sest"] = 6,
        ["sedem"] = 7,
        ["osem"] = 8,
        ["devet"] = 9
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly (string Word, int Value)[] TensWords =
    [
        ("devetdeset", 90),
        ("osemdeset", 80),
        ("sedemdeset", 70),
        ("šestdeset", 60),
        ("sestdeset", 60),
        ("petdeset", 50),
        ("štirideset", 40),
        ("stirideset", 40),
        ("trideset", 30),
        ("dvajset", 20)
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

        var normalized = Normalize(words);

        if (TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            return true;
        }

        if (normalized.StartsWith("minus ", StringComparison.Ordinal) &&
            TryParseCardinal(normalized["minus ".Length..], out parsedValue, out unrecognizedWord))
        {
            parsedValue = -parsedValue;
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
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var rawToken = tokenSpan.ToString();

            if (!TryParseToken(rawToken, out var numeric))
            {
                value = default;
                unrecognizedWord = rawToken;
                return false;
            }

            if (numeric >= 1000)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
                continue;
            }

            current += numeric;
        }

        value = total + current;
        return true;
    }

    static bool TryParseToken(string token, out int value)
    {
        if (CardinalMap.TryGetValue(token, out value))
        {
            return true;
        }

        foreach (var (word, tensValue) in TensWords)
        {
            if (!token.EndsWith(word, StringComparison.Ordinal))
            {
                continue;
            }

            var prefix = token[..^word.Length];
            if (!prefix.EndsWith("in", StringComparison.Ordinal))
            {
                continue;
            }

            prefix = prefix[..^2];
            if (UnitPrefixes.TryGetValue(prefix, out var unit))
            {
                value = tensValue + unit;
                return true;
            }
        }

        value = default;
        return false;
    }
}
