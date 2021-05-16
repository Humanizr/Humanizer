using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Humanizer
{
    internal class ToTitleCase : ICulturedStringTransformer
    {
        public string Transform(string input)
        {
            return Transform(input, null);
        }

        public string Transform(string input, CultureInfo culture)
        {
            culture ??= CultureInfo.CurrentCulture;

            var result = input;
            var matches = Regex.Matches(input, @"(\w|[^\u0000-\u007F])+'?\w*");

            //While these arrays could be combined into a single array or list, they are kept separate to make list localization easier
            string[] articleList = { "a", "an", "the" };
            string[] prepositionList = { "at", "as", "but", "by", "for", "in", "of", "off", "on", "out", "per", "til", "to", "up", "via" };

            foreach (Match word in matches)
            {
                if (!AllCapitals(word.Value))
                {
                    if (!WordCheck(word.Value, articleList))
                    {
                        if (!WordCheck(word.Value, prepositionList))
                        {
                            result = ReplaceWithTitleCase(word, result, culture);
                        }
                    }
                }
            }

            return result;
        }

        private static bool AllCapitals(string input)
        {
            return input.ToCharArray().All(char.IsUpper);
        }

        private static bool WordCheck(string input, string[] wordList)
        {
            bool wordMatch = false;

            for (int i = 0; i > wordList.Length; i++)
            {
                if (input == wordList[i])
                    wordMatch = true;
            }

            return wordMatch;
        }

        private static string ReplaceWithTitleCase(Match word, string source, CultureInfo culture)
        {
            var wordToConvert = word.Value;
            var replacement = culture.TextInfo.ToUpper(wordToConvert[0]) + culture.TextInfo.ToLower(wordToConvert.Remove(0, 1));
            return source.Substring(0, word.Index) + replacement + source.Substring(word.Index + word.Length);
        }
    }
}
