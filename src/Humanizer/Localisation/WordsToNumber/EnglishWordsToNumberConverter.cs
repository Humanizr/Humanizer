namespace Humanizer;

internal partial class EnglishWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    const string OrdinalSuffixPattern = @"\b(\d+)(st|nd|rd|th)\b";

#if NET7_0_OR_GREATER
    [GeneratedRegex(OrdinalSuffixPattern)]
    private static partial Regex OrdinalSuffixRegexGenerated();

    static Regex OrdinalSuffixRegex() => OrdinalSuffixRegexGenerated();
#else
    static readonly Regex ordinalSuffixRegex = new(OrdinalSuffixPattern, RegexOptions.Compiled);

    static Regex OrdinalSuffixRegex() => ordinalSuffixRegex;
#endif

    static readonly FrozenDictionary<string, int> numbersMap = new Dictionary<string, int>
    {
        { "zero", 0 }, { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 },
        { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 }, { "ten", 10 },
        { "eleven", 11 }, { "twelve", 12 }, { "thirteen", 13 }, { "fourteen", 14 },
        { "fifteen", 15 }, { "sixteen", 16 }, { "seventeen", 17 }, { "eighteen", 18 },
        { "nineteen", 19 }, { "twenty", 20 }, { "thirty", 30 }, { "forty", 40 },
        { "fifty", 50 }, { "sixty", 60 }, { "seventy", 70 }, { "eighty", 80 },
        { "ninety", 90 }, { "hundred", 100 }, { "thousand", 1000 },
        { "million", 1_000_000 }, { "billion", 1_000_000_000 }
    }.ToFrozenDictionary();

    static readonly FrozenDictionary<string, int> ordinalsMap = new Dictionary<string, int>
    {
        { "first", 1 }, { "second", 2 }, { "third", 3 }, { "fourth", 4 }, { "fifth", 5 },
        { "sixth", 6 }, { "seventh", 7 }, { "eighth", 8 }, { "ninth", 9 }, { "tenth", 10 },
        { "eleventh", 11 }, { "twelfth", 12 }, { "thirteenth", 13 }, { "fourteenth", 14 },
        { "fifteenth", 15 }, { "sixteenth", 16 }, { "seventeenth", 17 }, { "eighteenth", 18 },
        { "nineteenth", 19 }, { "twentieth", 20 }, { "thirtieth", 30 },
        { "fortieth", 40 }, { "fiftieth", 50 }, { "sixtieth", 60 }, { "seventieth", 70 },
        { "eightieth", 80 }, { "ninetieth", 90 }, { "hundredth", 100 }, { "thousandth", 1000 }
    }.ToFrozenDictionary();

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var result, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return result;
    }

    public override bool TryConvert(string words, out int parsedValue) => TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        unrecognizedWord = null;

        if (words.Length <= 20 && TryConvertFastCardinal(words.AsSpan(), out parsedValue))
        {
            return true;
        }

        words = words.Replace(",", "")
                     .Replace(" and ", " ")
                     .ToLowerInvariant()
                     .Trim();

        var isNegative = words.StartsWith("minus ", StringComparison.Ordinal) ||
                         words.StartsWith("negative ", StringComparison.Ordinal);
        if (isNegative)
        {
            words = words.Replace("minus ", string.Empty)
                         .Replace("negative ", string.Empty)
                         .Trim();
        }

        words = OrdinalSuffixRegex().Replace(words, "$1");
        words = words.Replace("-", " ");

        if (int.TryParse(words, out var numericValue))
        {
            parsedValue = isNegative ? -numericValue : numericValue;
            return true;
        }

        if (ordinalsMap.TryGetValue(words, out var ordinalValue))
        {
            parsedValue = isNegative ? -ordinalValue : ordinalValue;
            return true;
        }

        if (TryConvertWordsToNumber(words, out var numberValue, out var unrecognizedNumberWord))
        {
            parsedValue = isNegative ? -numberValue : numberValue;
            return true;
        }

        unrecognizedWord = unrecognizedNumberWord;
        parsedValue = default;
        return false;
    }

    static bool TryConvertWordsToNumber(string words, out int result, out string? unrecognizedWord)
    {
        var wordsArray = words.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        result = 0;
        unrecognizedWord = null;
        var current = 0;
        var hasOrdinal = false;

        foreach (var word in wordsArray)
        {
            if (ordinalsMap.TryGetValue(word, out var ordinalValue))
            {
                result += current + ordinalValue;
                hasOrdinal = true;
                break;
            }

            if (!numbersMap.TryGetValue(word, out var value))
            {
                unrecognizedWord = word;
                return false;
            }

            if (value == 100)
            {
                current = (current == 0 ? 1 : current) * 100;
            }
            else if (value >= 1000)
            {
                result += (current == 0 ? 1 : current) * value;
                current = 0;
            }
            else
            {
                current += value;
            }
        }

        if (!hasOrdinal)
        {
            result += current;
        }

        return true;
    }

    static bool TryConvertFastCardinal(ReadOnlySpan<char> words, out int value)
    {
        words = words.Trim();
        if (words.IsEmpty)
        {
            value = default;
            return false;
        }

        var isNegative = false;
        if (TryStripPrefix(ref words, "minus") || TryStripPrefix(ref words, "negative"))
        {
            isNegative = true;
        }

        if (words.IsEmpty)
        {
            value = default;
            return false;
        }

        long total = 0;
        long current = 0;
        var tokenCount = 0;

        while (TryReadToken(ref words, out var token))
        {
            if (IsIgnoredToken(token))
            {
                continue;
            }

            tokenCount++;
            if (!TryGetFastCardinalTokenValue(token, out var tokenValue))
            {
                value = default;
                return false;
            }

            if (tokenValue == 100)
            {
                current = (current == 0 ? 1 : current) * 100;
                continue;
            }

            if (tokenValue >= 1000)
            {
                total += (current == 0 ? 1 : current) * tokenValue;
                current = 0;
                continue;
            }

            current += tokenValue;
        }

        if (tokenCount == 0)
        {
            value = default;
            return false;
        }

        var parsed = total + current;
        if (parsed is > int.MaxValue or < int.MinValue)
        {
            value = default;
            return false;
        }

        value = (int)parsed;
        if (isNegative)
        {
            value = -value;
        }

        return true;
    }

    static bool TryStripPrefix(ref ReadOnlySpan<char> words, string prefix)
    {
        if (words.Length <= prefix.Length ||
            !EqualsIgnoreCase(words[..prefix.Length], prefix) ||
            !char.IsWhiteSpace(words[prefix.Length]))
        {
            return false;
        }

        words = words[(prefix.Length + 1)..].TrimStart();
        return true;
    }

    static bool TryReadToken(ref ReadOnlySpan<char> words, out ReadOnlySpan<char> token)
    {
        while (!words.IsEmpty && IsTokenSeparator(words[0]))
        {
            words = words[1..];
        }

        if (words.IsEmpty)
        {
            token = default;
            return false;
        }

        var tokenLength = 0;
        while (tokenLength < words.Length && !IsTokenSeparator(words[tokenLength]))
        {
            tokenLength++;
        }

        token = words[..tokenLength];
        words = tokenLength == words.Length ? [] : words[tokenLength..];
        return true;
    }

    static bool IsTokenSeparator(char value) => value == ',' || value == '-' || char.IsWhiteSpace(value);

    static bool IsIgnoredToken(ReadOnlySpan<char> token) => EqualsIgnoreCase(token, "and");

    static bool TryGetFastCardinalTokenValue(ReadOnlySpan<char> token, out int value)
    {
        switch (token.Length)
        {
            case 3:
                if (EqualsIgnoreCase(token, "one")) { value = 1; return true; }
                if (EqualsIgnoreCase(token, "two")) { value = 2; return true; }
                if (EqualsIgnoreCase(token, "six")) { value = 6; return true; }
                if (EqualsIgnoreCase(token, "ten")) { value = 10; return true; }
                break;
            case 4:
                if (EqualsIgnoreCase(token, "zero")) { value = 0; return true; }
                if (EqualsIgnoreCase(token, "five")) { value = 5; return true; }
                if (EqualsIgnoreCase(token, "four")) { value = 4; return true; }
                if (EqualsIgnoreCase(token, "nine")) { value = 9; return true; }
                break;
            case 5:
                if (EqualsIgnoreCase(token, "three")) { value = 3; return true; }
                if (EqualsIgnoreCase(token, "seven")) { value = 7; return true; }
                if (EqualsIgnoreCase(token, "eight")) { value = 8; return true; }
                if (EqualsIgnoreCase(token, "forty")) { value = 40; return true; }
                if (EqualsIgnoreCase(token, "fifty")) { value = 50; return true; }
                if (EqualsIgnoreCase(token, "sixty")) { value = 60; return true; }
                break;
            case 6:
                if (EqualsIgnoreCase(token, "eleven")) { value = 11; return true; }
                if (EqualsIgnoreCase(token, "twelve")) { value = 12; return true; }
                if (EqualsIgnoreCase(token, "twenty")) { value = 20; return true; }
                if (EqualsIgnoreCase(token, "thirty")) { value = 30; return true; }
                if (EqualsIgnoreCase(token, "ninety")) { value = 90; return true; }
                if (EqualsIgnoreCase(token, "hundred")) { value = 100; return true; }
                break;
            case 7:
                if (EqualsIgnoreCase(token, "thirteen")) { value = 13; return true; }
                if (EqualsIgnoreCase(token, "fourteen")) { value = 14; return true; }
                if (EqualsIgnoreCase(token, "fifteen")) { value = 15; return true; }
                if (EqualsIgnoreCase(token, "sixteen")) { value = 16; return true; }
                if (EqualsIgnoreCase(token, "seventy")) { value = 70; return true; }
                if (EqualsIgnoreCase(token, "million")) { value = 1_000_000; return true; }
                if (EqualsIgnoreCase(token, "billion")) { value = 1_000_000_000; return true; }
                break;
            case 8:
                if (EqualsIgnoreCase(token, "seventeen")) { value = 17; return true; }
                if (EqualsIgnoreCase(token, "eighteen")) { value = 18; return true; }
                if (EqualsIgnoreCase(token, "nineteen")) { value = 19; return true; }
                if (EqualsIgnoreCase(token, "eighty")) { value = 80; return true; }
                if (EqualsIgnoreCase(token, "thousand")) { value = 1000; return true; }
                break;
        }

        value = default;
        return false;
    }

    static bool EqualsIgnoreCase(ReadOnlySpan<char> token, string candidate)
    {
        if (token.Length != candidate.Length)
        {
            return false;
        }

        for (var i = 0; i < token.Length; i++)
        {
            if (char.ToLowerInvariant(token[i]) != candidate[i])
            {
                return false;
            }
        }

        return true;
    }
}
