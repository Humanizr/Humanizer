using System.Linq;
using System.Text.RegularExpressions;

namespace Humanizer
{
    internal class ToTitleCase : IStringTransformer
    {
        public string Transform(string input)
        {
            var result = input;
            var matches = Regex.Matches(input, @"(\w|[^\u0000-\u007F])+'?\w*");
            foreach (Match word in matches)
            {
                if (!AllCapitals(word.Value))
                {
                    result = ReplaceWithTitleCase(word, result);
                }
            }

            return result;
        }

        private static bool AllCapitals(string input)
        {
            return input.ToCharArray().All(char.IsUpper);
        }

        private static string ReplaceWithTitleCase(Match word, string source)
        {
            var wordToConvert = word.Value;
            var replacement = char.ToUpper(wordToConvert[0]) + wordToConvert.Remove(0, 1).ToLower();
            return source.Substring(0, word.Index) + replacement + source.Substring(word.Index + word.Length);
        }
    }
}
