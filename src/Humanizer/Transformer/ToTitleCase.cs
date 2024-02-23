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

            var matches = Regex.Matches(input, @"(\w|[^\u0000-\u007F])+'?\w*");
            var firstWord = true;
            var builder = new StringBuilder(input);
            var textInfo = culture.TextInfo;
            foreach (Match word in matches)
            {
                var value = word.Value;
                if (!AllCapitals(value))
                {
                    if (firstWord ||
                        !lookups.Contains(value))
                    {
                        builder[word.Index] = textInfo.ToUpper(value[0]);
                        Overwrite(builder, word.Index + 1, textInfo.ToLower(value[1..]));
                    }
                    else
                    {
                        var replacement = textInfo.ToLower(value);
                        Overwrite(builder, word.Index, replacement);
                    }
                }

                firstWord = false;
            }

            return builder.ToString();
        }

        static void Overwrite(StringBuilder builder, int index, string replacement) =>
            builder
                .Remove(index, replacement.Length)
                .Insert(index, replacement);

        static bool AllCapitals(string input)
        {
            foreach (var ch in input)
            {
                if (!char.IsUpper(ch))
                {
                    return false;
                }
            }

            return true;
        }

        static FrozenSet<string> lookups;

        static ToTitleCase()
        {
            var articles = new List<string> { "a", "an", "the" };
            var conjunctions = new List<string> { "and", "as", "but", "if", "nor", "or", "so", "yet" };
            var prepositions = new List<string> { "as", "at", "by", "for", "in", "of", "off", "on", "to", "up", "via" };

            lookups = articles.Concat(conjunctions).Concat(prepositions).ToFrozenSet();
        }

        static string Replacement(bool firstWord, string wordToConvert, TextInfo textInfo)
        {
            if (firstWord ||
                !lookups.Contains(wordToConvert))
            {
                return textInfo.ToUpper(wordToConvert[0]) + textInfo.ToLower(wordToConvert[1..]);
            }

            return textInfo.ToLower(wordToConvert);
        }
    }
}
