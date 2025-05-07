using System.Globalization;

namespace Humanizer;

internal class DefaultWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    private readonly CultureInfo cultureInfo;

    public DefaultWordsToNumberConverter(CultureInfo culture) => cultureInfo = culture;

    public override int Convert(string words)
    {
        if (cultureInfo.TwoLetterISOLanguageName == "en")
        {
            return new EnglishWordsToNumberConverter().Convert(words);
        }
        throw new NotSupportedException($"Words-to-number conversion is not supported for '{cultureInfo.TwoLetterISOLanguageName}'.");
    }
}
