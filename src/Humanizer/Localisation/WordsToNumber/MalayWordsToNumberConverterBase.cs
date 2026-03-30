namespace Humanizer;

class MalayFamilyWordsToNumberConverter(MalayWordsToNumberProfile profile) : GenderlessWordsToNumberConverter
{
    readonly MalayWordsToNumberProfile profile = profile;

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

        if (normalized.StartsWith(profile.MinusWord + " ", StringComparison.Ordinal))
        {
            negative = true;
            normalized = normalized[(profile.MinusWord.Length + 1)..].Trim();
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

        unrecognizedWord = WordsToNumberTokenizer.GetLastTokenOrSelf(normalized);
        parsedValue = default;
        return false;
    }

    protected virtual string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    bool TryParseCardinal(string words, out int value)
    {
        if (profile.Cardinals.TryGetValue(words, out value))
        {
            return true;
        }

        var total = 0;
        var current = 0;

        foreach (var tokenSpan in WordsToNumberTokenizer.Enumerate(words))
        {
            var token = tokenSpan.ToString();

            if (token == "dan")
            {
                continue;
            }

            if (!profile.Cardinals.TryGetValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (token == "belas")
            {
                current = (current == 0 ? 1 : current) + 10;
            }
            else if (token == "puluh")
            {
                current = (current == 0 ? 1 : current) * 10;
            }
            else if (tokenValue >= 1000)
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

sealed class MalayWordsToNumberProfile(string minusWord, FrozenDictionary<string, int> cardinals)
{
    public string MinusWord { get; } = minusWord;
    public FrozenDictionary<string, int> Cardinals { get; } = cardinals;
}
