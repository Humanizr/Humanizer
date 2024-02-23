using System.Collections.Frozen;

namespace Humanizer
{
    class ToTitleCase : ICulturedStringTransformer
    {
        public string Transform(string input) =>
            Transform(input, null);

        static Regex regex = new(@"(\w|[^\u0000-\u007F])+'?\w*", RegexOptions.Compiled);
        public string Transform(string input, CultureInfo? culture)
        {
            culture ??= CultureInfo.CurrentCulture;
            var matches = regex.Matches(input);
            var builder = new StringBuilder(input);
            var textInfo = culture.TextInfo;
            foreach (Match word in matches)
            {
                var value = word.Value;
                if (AllCapitals(value))
                {
                    continue;
                }

                if (lookups.Contains(value))
                {
                    continue;
                }

                builder[word.Index] = textInfo.ToUpper(value[0]);
                Overwrite(builder, word.Index + 1, textInfo.ToLower(value[1..]));
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
    }
}
