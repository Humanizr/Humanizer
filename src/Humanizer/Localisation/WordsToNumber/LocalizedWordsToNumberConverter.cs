namespace Humanizer;

internal class LocalizedWordsToNumberConverter(CultureInfo culture) : GenderlessWordsToNumberConverter
{
    private const int MinSupportedValue = -10000;
    private const int MaxSupportedValue = 10000;
    private const int MaxOrdinalSupportedValue = 1000;
    private static readonly Regex MultiWhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    private readonly CultureInfo cultureInfo = culture;
    private readonly Lazy<FrozenDictionary<string, int>> valueMap = new(() => BuildValueMap(culture));

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord ?? words}");
        }

        return parsedValue;
    }

    public override bool TryConvert(string words, out int parsedValue) => TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        if (int.TryParse(words, NumberStyles.Integer, cultureInfo, out parsedValue) ||
            int.TryParse(words, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        var normalizedWords = Normalize(words);
        if (valueMap.Value.TryGetValue(normalizedWords, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        parsedValue = default;
        unrecognizedWord = GetUnrecognizedWord(normalizedWords);
        return false;
    }

    private static FrozenDictionary<string, int> BuildValueMap(CultureInfo culture)
    {
        var map = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var value = MinSupportedValue; value <= MaxSupportedValue; value++)
        {
            TryAddMapping(map, value, () => value.ToWords(culture));
            TryAddMapping(map, value, () => value.ToWords(false, culture));
        }

        for (var value = 0; value <= MaxOrdinalSupportedValue; value++)
        {
            TryAddMapping(map, value, () => value.ToOrdinalWords(culture));
        }

        return map.ToFrozenDictionary(StringComparer.Ordinal);
    }

    private static void TryAddMapping(Dictionary<string, int> map, int value, Func<string> resolveWords)
    {
        try
        {
            AddMapping(map, resolveWords(), value);
        }
        catch (NotSupportedException)
        {
        }
        catch (ArgumentOutOfRangeException)
        {
        }
    }

    private static void AddMapping(Dictionary<string, int> map, string words, int value)
    {
        var normalizedWords = Normalize(words);
        if (normalizedWords.Length == 0 || map.ContainsKey(normalizedWords))
        {
            return;
        }

        map[normalizedWords] = value;
    }

    private static string Normalize(string words)
    {
        var normalizedWords = words.Normalize(NormalizationForm.FormKC)
            .ToLowerInvariant()
            .Replace(",", " ")
            .Replace("،", " ")
            .Replace("٬", " ")
            .Replace("-", " ")
            .Replace("‑", " ")
            .Trim();

        return MultiWhitespaceRegex.Replace(normalizedWords, " ");
    }

    private static string? GetUnrecognizedWord(string words)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            return null;
        }

        var separatorIndex = words.IndexOf(' ');
        return separatorIndex > -1 ? words[..separatorIndex] : words;
    }
}
