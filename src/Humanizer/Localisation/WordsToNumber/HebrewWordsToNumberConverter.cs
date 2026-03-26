using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Humanizer;

internal class HebrewWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> NumberTokens = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["אפס"] = 0,
        ["אחת"] = 1,
        ["אחד"] = 1,
        ["שתיים"] = 2,
        ["שתים"] = 2,
        ["שניים"] = 2,
        ["שנים"] = 2,
        ["שתי"] = 2,
        ["שני"] = 2,
        ["שלוש"] = 3,
        ["שלושה"] = 3,
        ["שלשת"] = 3,
        ["שלושת"] = 3,
        ["ארבע"] = 4,
        ["ארבעה"] = 4,
        ["ארבעת"] = 4,
        ["חמש"] = 5,
        ["חמישה"] = 5,
        ["חמשת"] = 5,
        ["שש"] = 6,
        ["שישה"] = 6,
        ["ששת"] = 6,
        ["שבע"] = 7,
        ["שבעה"] = 7,
        ["שבעת"] = 7,
        ["שמונה"] = 8,
        ["שמונת"] = 8,
        ["תשע"] = 9,
        ["תשעה"] = 9,
        ["תשעת"] = 9,
        ["עשר"] = 10,
        ["עשרה"] = 10,
        ["עשרת"] = 10,
        ["עשרים"] = 20,
        ["שלושים"] = 30,
        ["ארבעים"] = 40,
        ["חמישים"] = 50,
        ["שישים"] = 60,
        ["שבעים"] = 70,
        ["שמונים"] = 80,
        ["תשעים"] = 90,
        ["מאה"] = 100,
        ["מאתיים"] = 200,
        ["אלפיים"] = 2000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    static readonly FrozenDictionary<string, int> ScaleTokens = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["אלף"] = 1_000,
        ["אלפים"] = 1_000,
        ["מיליון"] = 1_000_000,
        ["מיליונים"] = 1_000_000,
        ["מליון"] = 1_000_000,
        ["מליונים"] = 1_000_000,
        ["מיליארד"] = 1_000_000_000,
        ["מיליארדים"] = 1_000_000_000
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
        if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
        {
            unrecognizedWord = null;
            return true;
        }

        var tokenizer = WordsToNumberTokenizer.Enumerate(normalized).GetEnumerator();
        string? pendingToken = null;
        if (!WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var firstToken))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        var negative = false;
        pendingToken = firstToken;
        if (IsNegative(firstToken))
        {
            negative = true;
            pendingToken = null;

            if (!WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var numberToken))
            {
                parsedValue = default;
                unrecognizedWord = "מינוס";
                return false;
            }

            pendingToken = numberToken;
        }

        if (!TryParse(ref tokenizer, ref pendingToken, out var parsedLong, out unrecognizedWord))
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

    static bool TryParse(ref WordsToNumberTokenizer.Enumerator tokenizer, ref string? pendingToken, out long parsedValue, out string? unrecognizedWord)
    {
        long total = 0;
        long current = 0;
        unrecognizedWord = null;

        while (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var rawToken))
        {
            if (rawToken.Length == 0 || rawToken == "ו")
            {
                continue;
            }

            var token = TrimLeadingConjunction(rawToken);
            if (token.Length == 0)
            {
                continue;
            }

            if (ScaleTokens.TryGetValue(token, out var scale))
            {
                total += (current == 0 ? 1 : current) * scale;
                current = 0;
                continue;
            }

            if (NumberTokens.TryGetValue(token, out var directValue) && directValue is 2000 or 200 or 100 or >= 20)
            {
                current += directValue;
                continue;
            }

            if (!TryGetUnitValue(token, out var unitValue))
            {
                unrecognizedWord = rawToken;
                parsedValue = default;
                return false;
            }

            if (WordsToNumberTokenizer.TryReadNext(ref tokenizer, ref pendingToken, out var nextToken))
            {
                nextToken = TrimLeadingConjunction(nextToken);
                if (IsTeenSuffix(nextToken))
                {
                    current += 10 + unitValue;
                    continue;
                }

                if (nextToken == "מאות" && unitValue is >= 3 and <= 9)
                {
                    current += unitValue * 100;
                    continue;
                }

                pendingToken = nextToken;
            }

            current += unitValue;
        }

        parsedValue = total + current;
        return true;
    }

    static bool TryGetUnitValue(string token, out int unitValue)
    {
        if (NumberTokens.TryGetValue(token, out var value) && value is >= 0 and <= 10)
        {
            unitValue = value;
            return true;
        }

        unitValue = default;
        return false;
    }

    static bool IsTeenSuffix(string token) => token is "עשר" or "עשרה";

    static bool IsNegative(string token) => TrimLeadingConjunction(token) is "מינוס" or "שלילי";

    static string TrimLeadingConjunction(string token) =>
        token.Length > 1 && token[0] == 'ו'
            ? token[1..]
            : token;

    static string Normalize(string words)
    {
        var builder = new StringBuilder(words.Length);

        foreach (var character in words.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            switch (character)
            {
                case ',':
                case '.':
                case ':':
                case ';':
                case '"':
                case '\'':
                case '״':
                case '׳':
                case '-':
                case '־':
                case '/':
                case '\\':
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                    builder.Append(' ');
                    break;
                default:
                    builder.Append(char.IsWhiteSpace(character) ? ' ' : character);
                    break;
            }
        }

        return Regex.Replace(builder.ToString().Normalize(NormalizationForm.FormC).Trim(), @"\s+", " ");
    }
}
