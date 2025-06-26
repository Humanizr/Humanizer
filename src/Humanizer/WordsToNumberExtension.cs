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
        public static bool TryConvertToNumber(this string words, out int parsedNumber, CultureInfo culture) => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber);
        public static bool TryConvertToNumber(this string words, out int parsedNumber, CultureInfo culture, out string? unrecognizedWord) => Configurator.GetWordsToNumberConverter(culture).TryConvert(words, out parsedNumber, out unrecognizedWord);


    }
}
