namespace Humanizer;

/// <summary>
/// Parses localized decimal number words using token-map rules from
/// <see cref="TokenMapWordsToNumberConverter"/>.
/// </summary>
internal sealed class TokenMapWordsToDecimalNumberConverter(TokenMapWordsToNumberConverter converter)
    : GenderlessWordsToDecimalNumberConverter
{
    readonly TokenMapWordsToNumberConverter converter = converter;

    /// <inheritdoc />
    public override decimal Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    /// <inheritdoc />
    public override bool TryConvert(string words, out decimal parsedValue) =>
        converter.TryConvertDecimal(words, out parsedValue, out _);

    /// <inheritdoc />
    public override bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber) =>
        converter.TryConvertDecimal(words, out parsedValue, out unrecognizedNumber);
}
