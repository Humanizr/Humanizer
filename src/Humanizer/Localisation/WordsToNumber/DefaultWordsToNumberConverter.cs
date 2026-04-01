namespace Humanizer;

internal class DefaultWordsToNumberConverter(CultureInfo culture) : GenderlessWordsToNumberConverter
{
    readonly CultureInfo cultureInfo = culture;

    public override int Convert(string words)
    {
        TryConvert(words, out var parsedValue);

        return parsedValue;
    }
    public override bool TryConvert(string words, out int parsedValue) => TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        throw new NotSupportedException($"Words-to-number conversion is not supported for '{cultureInfo.TwoLetterISOLanguageName}'.");
    }

}
