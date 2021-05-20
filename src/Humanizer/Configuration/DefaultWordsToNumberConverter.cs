using System.Globalization;
using Humanizer.Localisation.WordsToNumber;

namespace Humanizer.Configuration
{
    internal class DefaultWordsToNumberConverter : GenderlessWordsToNumberConverter
    {
        private readonly CultureInfo _culture;

        public DefaultWordsToNumberConverter(CultureInfo culture)
        {
            _culture = culture;
        }

        public override int Convert(string words)
        {
            return words.ToNumber(_culture);
        }
    }
}
