using System.Globalization;

namespace Humanizer;

internal class DefaultWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    private readonly CultureInfo cultureInfo;

    public DefaultWordsToNumberConverter(CultureInfo culture) => cultureInfo = culture;

    public override int Convert(string words)
    {
        TryConvert(words, out var parsedValue);

        return parsedValue;
    }
    public override bool TryConvert(string words, out int parsedValue) => TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (cultureInfo.TwoLetterISOLanguageName == "en")
        {
            return new EnglishWordsToNumberConverter().TryConvert(words, out parsedValue, out unrecognizedWord);
        }
        throw new NotSupportedException($"Words-to-number conversion is not supported for '{cultureInfo.TwoLetterISOLanguageName}'.");
    }

}
