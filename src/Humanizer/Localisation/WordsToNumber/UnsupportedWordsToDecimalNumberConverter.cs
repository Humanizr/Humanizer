namespace Humanizer;

/// <summary>
/// Fallback converter used when a locale does not support decimal word parsing.
/// </summary>
internal sealed class UnsupportedWordsToDecimalNumberConverter : GenderlessWordsToDecimalNumberConverter
{
    /// <summary>
    /// Gets the shared fallback converter instance.
    /// </summary>
    internal static UnsupportedWordsToDecimalNumberConverter Instance { get; } = new();

    UnsupportedWordsToDecimalNumberConverter()
    {
    }

    /// <inheritdoc />
    public override decimal Convert(string words) =>
        throw new NotSupportedException("Decimal word parsing is not supported for the requested culture.");

    /// <inheritdoc />
    public override bool TryConvert(string words, out decimal parsedValue)
    {
        parsedValue = default;
        return false;
    }

    /// <inheritdoc />
    public override bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber)
    {
        parsedValue = default;
        unrecognizedNumber = words;
        return false;
    }
}
