namespace Humanizer;

internal partial class PortugueseWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["zero"] = 0,
        ["um"] = 1,
        ["uma"] = 1,
        ["dois"] = 2,
        ["duas"] = 2,
        ["três"] = 3,
        ["quatro"] = 4,
        ["cinco"] = 5,
        ["seis"] = 6,
        ["sete"] = 7,
        ["oito"] = 8,
        ["nove"] = 9,
        ["dez"] = 10,
        ["onze"] = 11,
        ["doze"] = 12,
        ["treze"] = 13,
        ["quatorze"] = 14,
        ["quinze"] = 15,
        ["dezasseis"] = 16,
        ["dezassete"] = 17,
        ["dezoito"] = 18,
        ["dezanove"] = 19,
        ["vinte"] = 20,
        ["trinta"] = 30,
        ["quarenta"] = 40,
        ["cinquenta"] = 50,
        ["sessenta"] = 60,
        ["setenta"] = 70,
        ["oitenta"] = 80,
        ["noventa"] = 90,
        ["cem"] = 100,
        ["cento"] = 100,
        ["duzentos"] = 200,
        ["duzentas"] = 200,
        ["trezentos"] = 300,
        ["trezentas"] = 300,
        ["quatrocentos"] = 400,
        ["quatrocentas"] = 400,
        ["quinhentos"] = 500,
        ["quinhentas"] = 500,
        ["seiscentos"] = 600,
        ["seiscentas"] = 600,
        ["setecentos"] = 700,
        ["setecentas"] = 700,
        ["oitocentos"] = 800,
        ["oitocentas"] = 800,
        ["novecentos"] = 900,
        ["novecentas"] = 900,
        ["mil"] = 1000,
        ["milhão"] = 1_000_000,
        ["milhões"] = 1_000_000,
        ["milhar"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> OrdinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["primeiro"] = 1,
        ["primeira"] = 1,
        ["vigésimo primeiro"] = 21
    }.ToFrozenDictionary(StringComparer.Ordinal);

#if NET7_0_OR_GREATER
    [GeneratedRegex(@"^(?<number>\d+)(?<suffix>º|ª)$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex OrdinalAbbreviationRegexGenerated();

    private static Regex OrdinalAbbreviationRegex() => OrdinalAbbreviationRegexGenerated();
#else
    private static readonly Regex OrdinalAbbreviationRegexField = new(@"^(?<number>\d+)(?<suffix>º|ª)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

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

        if (TryParseOrdinalAbbreviation(words, out parsedValue) ||
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
        var match = OrdinalAbbreviationRegex().Match(words.Trim());
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
            if (token == "e")
            {
                continue;
            }

            if (token == "mil" &&
                WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var milhoesToken))
            {
                if (milhoesToken == "milhões")
                {
                    total += (current == 0 ? 1 : current) * 1_000_000_000;
                    current = 0;
                    continue;
                }

                pendingToken = milhoesToken;
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
            else
            {
                current += numeric;
            }
        }

        value = total + current;
        return true;
    }
}
