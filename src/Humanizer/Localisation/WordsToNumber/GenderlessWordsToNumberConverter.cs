namespace Humanizer.Localisation.WordsToNumber
{
    internal abstract class GenderlessWordsToNumberConverter : IWordsToNumberConverter
    {
        public abstract int Convert(string words);
    }
}