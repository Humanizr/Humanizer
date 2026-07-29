namespace Humanizer;

/// <summary>
/// Parses localized decimal number words using the locale's existing number-word converters.
/// </summary>
internal sealed class LocalizedWordsToDecimalNumberConverter : IWordsToDecimalNumberConverter
{
    const int MaxDecimalScale = 28;

    readonly CompareInfo compareInfo;
    readonly string decimalMarker;
    readonly FractionalDigit[] fractionalDigits;
    readonly IWordsToNumberConverter integerConverter;
    readonly bool allowsJoinedTokens;
    readonly bool allowsEnglishOverflowComposition;
    readonly NegativeAffix[] negativePrefixes;
    readonly NegativeAffix[] negativeSuffixes;

    public LocalizedWordsToDecimalNumberConverter(
        CultureInfo culture,
        string decimalMarker,
        string[] negativePrefixes,
        string[] negativeSuffixes)
    {
        compareInfo = culture.CompareInfo;
        this.decimalMarker = NormalizeWhitespace(decimalMarker);
        integerConverter = Configurator.GetWordsToNumberConverter(culture);
        allowsJoinedTokens = integerConverter is EastAsianPositionalWordsToNumberConverter;
        allowsEnglishOverflowComposition =
            string.Equals(culture.TwoLetterISOLanguageName, "en", StringComparison.OrdinalIgnoreCase);
        this.negativePrefixes = NormalizePrefixes(negativePrefixes);
        this.negativeSuffixes = NormalizeSuffixes(negativeSuffixes);
        fractionalDigits = CreateFractionalDigits(culture);
    }

    internal string DecimalMarker => decimalMarker;

    /// <inheritdoc />
    public decimal Convert(string words)
    {
        ArgumentNullException.ThrowIfNull(words);

        if (!TryConvert(words, out var parsedValue, out var unrecognizedNumber))
        {
            throw new FormatException($"Unrecognized decimal number word: {unrecognizedNumber}");
        }

        return parsedValue;
    }

    /// <inheritdoc />
    public bool TryConvert(string words, out decimal parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    /// <inheritdoc />
    public bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber)
    {
        parsedValue = default;
        unrecognizedNumber = words ?? string.Empty;

        if (words is null || string.IsNullOrWhiteSpace(words))
        {
            return false;
        }

        var normalizedWords = NormalizeWhitespace(words);
        var negative = StripNegativeAffix(ref normalizedWords);
        var markerIndex = FindDecimalMarker(normalizedWords);
        if (markerIndex < 0)
        {
            return false;
        }

        if (FindDecimalMarker(normalizedWords, markerIndex + decimalMarker.Length) >= 0)
        {
            unrecognizedNumber = decimalMarker;
            return false;
        }

        var integerWords = normalizedWords[..markerIndex].Trim();
        var fractionWords = normalizedWords[(markerIndex + decimalMarker.Length)..].Trim();

        if (fractionWords.Length == 0)
        {
            unrecognizedNumber = decimalMarker;
            return false;
        }

        var integerDigits = "0";
        if (integerWords.Length > 0 &&
            !TryConvertIntegerPart(integerWords, out integerDigits, out unrecognizedNumber))
        {
            return false;
        }

        var fraction = new StringBuilder();
        for (var index = 0; index < fractionWords.Length;)
        {
            while (index < fractionWords.Length && char.IsWhiteSpace(fractionWords[index]))
            {
                index++;
            }

            if (index == fractionWords.Length)
            {
                break;
            }

            var matched = false;
            foreach (var digit in fractionalDigits)
            {
                if (index + digit.Word.Length > fractionWords.Length ||
                    compareInfo.Compare(
                        fractionWords,
                        index,
                        digit.Word.Length,
                        digit.Word,
                        0,
                        digit.Word.Length,
                        CompareOptions.IgnoreCase) != 0)
                {
                    continue;
                }

                var digitEnd = index + digit.Word.Length;
                if (!allowsJoinedTokens &&
                    digitEnd < fractionWords.Length &&
                    !char.IsWhiteSpace(fractionWords[digitEnd]))
                {
                    continue;
                }

                if (fraction.Length == MaxDecimalScale)
                {
                    unrecognizedNumber = digit.Word;
                    return false;
                }

                fraction.Append(digit.Value);
                index = digitEnd;
                matched = true;
                break;
            }

            if (!matched)
            {
                var tokenEnd = index;
                while (tokenEnd < fractionWords.Length && !char.IsWhiteSpace(fractionWords[tokenEnd]))
                {
                    tokenEnd++;
                }

                unrecognizedNumber = fractionWords[index..tokenEnd];
                return false;
            }
        }

        if (fraction.Length == 0)
        {
            unrecognizedNumber = decimalMarker;
            return false;
        }

        var signedInteger = negative ? $"-{integerDigits}" : integerDigits;
        var invariantValue = $"{signedInteger}.{fraction}";

        if (!decimal.TryParse(
                invariantValue,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out parsedValue) ||
            (decimal.GetBits(parsedValue)[3] >> 16 & 0xFF) != fraction.Length)
        {
            parsedValue = default;
            unrecognizedNumber = words;
            return false;
        }

        unrecognizedNumber = null;
        return true;
    }

    bool TryConvertIntegerPart(string words, out string digits, out string? unrecognizedNumber)
    {
        try
        {
            if (integerConverter.TryConvert(words, out var value, out unrecognizedNumber))
            {
                digits = value.ToString(CultureInfo.InvariantCulture);
                return true;
            }

            var initialUnrecognizedNumber = unrecognizedNumber;
            if (!allowsEnglishOverflowComposition)
            {
                digits = string.Empty;
                return false;
            }

            var tokens = words.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            for (var index = tokens.Length - 1; index > 0; index--)
            {
                if (!integerConverter.TryConvert(tokens[index], out var scale, out _) ||
                    !TryGetScaleDigits(scale, out var scaleDigits))
                {
                    continue;
                }

                var highWords = string.Join(" ", tokens, 0, index);
                if (!integerConverter.TryConvert(highWords, out var high, out _) || high <= 0)
                {
                    continue;
                }

                var lowWords = string.Join(" ", tokens, index + 1, tokens.Length - index - 1);
                var low = 0L;
                if (lowWords.Length > 0 &&
                    !integerConverter.TryConvert(lowWords, out low, out _) ||
                    low < 0 ||
                    low >= scale)
                {
                    continue;
                }

                digits = string.Concat(
                    high.ToString(CultureInfo.InvariantCulture),
                    low.ToString($"D{scaleDigits}", CultureInfo.InvariantCulture));
                unrecognizedNumber = null;
                return true;
            }

            digits = string.Empty;
            unrecognizedNumber = initialUnrecognizedNumber;
            return false;
        }
        catch (OverflowException)
        {
            digits = string.Empty;
            unrecognizedNumber = words;
            return false;
        }
    }

    int FindDecimalMarker(string words, int startIndex = 0)
    {
        while (startIndex <= words.Length - decimalMarker.Length)
        {
            var markerIndex = compareInfo.IndexOf(
                words,
                decimalMarker,
                startIndex,
                CompareOptions.IgnoreCase);
            if (markerIndex < 0)
            {
                return -1;
            }

            var markerEnd = markerIndex + decimalMarker.Length;
            if (allowsJoinedTokens ||
                ((markerIndex == 0 || char.IsWhiteSpace(words[markerIndex - 1])) &&
                 (markerEnd == words.Length || char.IsWhiteSpace(words[markerEnd]))))
            {
                return markerIndex;
            }

            startIndex = markerIndex + decimalMarker.Length;
        }

        return -1;
    }

    bool StripNegativeAffix(ref string words)
    {
        foreach (var prefix in negativePrefixes)
        {
            if (!compareInfo.IsPrefix(words, prefix.Text, CompareOptions.IgnoreCase) ||
                prefix.RequiresSeparator &&
                words.Length > prefix.Text.Length &&
                !char.IsWhiteSpace(words[prefix.Text.Length]))
            {
                continue;
            }

            words = words[prefix.Text.Length..].TrimStart();
            return true;
        }

        foreach (var suffix in negativeSuffixes)
        {
            var suffixStart = words.Length - suffix.Text.Length;
            if (!compareInfo.IsSuffix(words, suffix.Text, CompareOptions.IgnoreCase) ||
                suffix.RequiresSeparator &&
                suffixStart > 0 &&
                !char.IsWhiteSpace(words[suffixStart - 1]))
            {
                continue;
            }

            words = words[..suffixStart].TrimEnd();
            return true;
        }

        return false;
    }

    static FractionalDigit[] CreateFractionalDigits(CultureInfo culture)
    {
        var digits = new List<FractionalDigit>(10);
        for (var value = 0; value <= 9; value++)
        {
            var word = GetDigitWord(value, culture);
            if (word.Length > 0)
            {
                digits.Add(new(word, (char)('0' + value)));
            }
        }

        return [.. digits.OrderByDescending(static digit => digit.Word.Length)];
    }

    static string GetDigitWord(int value, CultureInfo culture)
    {
        for (var current = culture; !string.IsNullOrEmpty(current.Name); current = current.Parent)
        {
            var word = Configurator.GetNumberToWordsConverter(current).Convert(value);
            if (!string.IsNullOrWhiteSpace(word))
            {
                return NormalizeWhitespace(word);
            }
        }

        return string.Empty;
    }

    static bool TryGetScaleDigits(long value, out int digits)
    {
        digits = 0;
        if (value < 1000)
        {
            return false;
        }

        while (value % 10 == 0)
        {
            value /= 10;
            digits++;
        }

        return value == 1;
    }

    static NegativeAffix[] NormalizePrefixes(string[] affixes) =>
        [.. affixes
            .Select(static affix => new NegativeAffix(
                NormalizeWhitespace(affix),
                affix.Length > 0 && char.IsWhiteSpace(affix[^1])))
            .Where(static affix => affix.Text.Length > 0)];

    static NegativeAffix[] NormalizeSuffixes(string[] affixes) =>
        [.. affixes
            .Select(static affix => new NegativeAffix(
                NormalizeWhitespace(affix),
                affix.Length > 0 && char.IsWhiteSpace(affix[0])))
            .Where(static affix => affix.Text.Length > 0)];

    static string NormalizeWhitespace(string value) =>
        string.Join(" ", value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    readonly record struct FractionalDigit(string Word, char Value);

    readonly record struct NegativeAffix(string Text, bool RequiresSeparator);
}