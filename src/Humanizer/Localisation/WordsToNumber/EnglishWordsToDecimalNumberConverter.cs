namespace Humanizer;

/// <summary>
/// Parses English decimal number words while delegating the integer part to the existing
/// words-to-number converter.
/// </summary>
internal sealed class EnglishWordsToDecimalNumberConverter(CultureInfo culture) : IWordsToDecimalNumberConverter
{
    const int MaxDecimalScale = 28;

    static readonly Regex DecimalMarker = new(
        @"(?<!\S)point(?!\S)",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    static readonly FrozenDictionary<string, char> FractionalDigits =
        new Dictionary<string, char>(StringComparer.OrdinalIgnoreCase)
        {
            ["zero"] = '0',
            ["one"] = '1',
            ["two"] = '2',
            ["three"] = '3',
            ["four"] = '4',
            ["five"] = '5',
            ["six"] = '6',
            ["seven"] = '7',
            ["eight"] = '8',
            ["nine"] = '9'
        }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

    readonly IWordsToNumberConverter integerConverter = Configurator.GetWordsToNumberConverter(culture);

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

        var markers = DecimalMarker.Matches(words);
        if (markers.Count != 1)
        {
            unrecognizedNumber = markers.Count > 1 ? "point" : words;
            return false;
        }

        var marker = markers[0];
        var integerWords = words[..marker.Index].Trim();
        var fractionWords = words[(marker.Index + marker.Length)..].Trim();
        var negativePrefix = StripNegativePrefix(ref integerWords);

        if (fractionWords.Length == 0)
        {
            unrecognizedNumber = "point";
            return false;
        }

        var integerDigits = "0";
        if (integerWords.Length > 0 &&
            !TryConvertIntegerPart(integerWords, out integerDigits, out unrecognizedNumber))
        {
            return false;
        }

        var fraction = new StringBuilder();
        foreach (var token in fractionWords.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
        {
            if (!FractionalDigits.TryGetValue(token, out var digit))
            {
                unrecognizedNumber = token;
                return false;
            }

            if (fraction.Length == MaxDecimalScale)
            {
                unrecognizedNumber = token;
                return false;
            }

            fraction.Append(digit);
        }

        if (fraction.Length == 0)
        {
            unrecognizedNumber = "point";
            return false;
        }

        var signedInteger = negativePrefix is null ? integerDigits : $"-{integerDigits}";
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

    static string? StripNegativePrefix(ref string words)
    {
        foreach (var prefix in new[] { "minus", "negative" })
        {
            if (!words.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ||
                words.Length > prefix.Length && !char.IsWhiteSpace(words[prefix.Length]))
            {
                continue;
            }

            words = words[prefix.Length..].TrimStart();
            return prefix;
        }

        return null;
    }
}