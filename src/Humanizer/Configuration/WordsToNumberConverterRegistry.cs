using Humanizer.Localisation.WordsToNumber;

namespace Humanizer.Configuration
{
    internal class WordsToNumberConverterRegistry : LocaliserRegistry<IWordsToNumberConverter>
    {
        public WordsToNumberConverterRegistry()
            : base((culture) => new DefaultWordsToNumberConverter(culture))
        {
            Register("en", new EnglishWordsToNumberConverter());
        }
    }
}
