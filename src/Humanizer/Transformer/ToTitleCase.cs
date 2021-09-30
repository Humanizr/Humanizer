using System.Collections.Generic;
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
            var firstWord = true;
            foreach (Match word in matches)
            {
                if (!AllCapitals(word.Value))
                {
                    result = ReplaceWithTitleCase(word, result, culture, firstWord);
                }
                firstWord = false;
            }

            return result;
        }

        private static bool AllCapitals(string input)
        {
            return input.ToCharArray().All(char.IsUpper);
        }

        private static string ReplaceWithTitleCase(Match word, string source, CultureInfo culture, bool firstWord)
        {
            var articles = new List<string> { "a", "an", "the" };
            var conjunctions = new List<string> { "and", "as", "but", "if", "nor", "or", "so", "yet" };
            var prepositions = new List<string> { "as", "at", "by", "for", "in", "of", "off", "on", "to", "up", "via" };
            
            var wordToConvert = word.Value;
            string replacement;

            
            if (firstWord || 
                (!articles.Contains(wordToConvert) &&
                !conjunctions.Contains(wordToConvert) &&
                !prepositions.Contains(wordToConvert)))
            {
                replacement = culture.TextInfo.ToUpper(wordToConvert[0]) + culture.TextInfo.ToLower(wordToConvert.Remove(0, 1));
                
            }
            else
            {
                replacement = culture.TextInfo.ToLower(wordToConvert);
            }
            return source.Substring(0, word.Index) + replacement + source.Substring(word.Index + word.Length);
        }
    }
}
