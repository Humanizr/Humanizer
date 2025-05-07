using System.Globalization;
using Humanizer;
namespace Humanizer
{
    /// <summary>
    /// Transform humanized string to number; e.g. one => 1
    /// </summary>
    public static class WordsToNumberExtension
    {
        public static int ToNumber(this string words, CultureInfo culture) => Configurator.GetWordsToNumberConverter(culture).Convert(words);
    }
}
