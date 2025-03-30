using System.Globalization;

namespace Humanizer
{
    internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
    {
        public WordsToNumberConverterRegistry()
            : base(culture => culture.TwoLetterISOLanguageName == "en"
                ? new EnglishWordsToNumberConverter()
                : new DefaultWordsToNumberConverter(culture)) =>
                 Register("en", new EnglishWordsToNumberConverter());
    }
}
