using System.Collections.Frozen;

namespace Humanizer
{
    class ToTitleCase : ICulturedStringTransformer
    {
        public string Transform(string input) =>
            Transform(input, null);

        public string Transform(string input, CultureInfo? culture)
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

        static bool AllCapitals(string input) =>
            input.All(char.IsUpper);

        static FrozenSet<string> lookups;

        static ToTitleCase()
        {
            var articles = new List<string> { "a", "an", "the" };
            var conjunctions = new List<string> { "and", "as", "but", "if", "nor", "or", "so", "yet" };
            var prepositions = new List<string> { "as", "at", "by", "for", "in", "of", "off", "on", "to", "up", "via" };

            lookups = articles.Concat(conjunctions).Concat(prepositions).ToFrozenSet();
        }

        static string ReplaceWithTitleCase(Match word, string source, CultureInfo culture, bool firstWord)
        {
            var wordToConvert = word.Value;
            string replacement;

            if (firstWord ||
                !lookups.Contains(wordToConvert))
            {
                replacement = culture.TextInfo.ToUpper(wordToConvert[0]) + culture.TextInfo.ToLower(wordToConvert[1..]);

            }
            else
            {
                replacement = culture.TextInfo.ToLower(wordToConvert);
            }

            var span = source.AsSpan();
            return $"{span[..word.Index]}{replacement}{span[(word.Index + word.Length)..]}";
        }
    }
}
