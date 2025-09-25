namespace Humanizer;

class ToTitleCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, null);

    static readonly Regex Regex = new(@"(\w|[^\u0000-\u007F])+'?\w*", RegexOptions.Compiled);

    public string Transform(string input, CultureInfo? culture)
    {
        culture ??= CultureInfo.CurrentCulture;
        var matches = Regex.Matches(input);
        var builder = new StringBuilder(input);
        var textInfo = culture.TextInfo;
        foreach (Match word in matches)
        {
            var value = word.Value;
            if (AllCapitals(value) || IsArticleOrConjunctionOrPreposition(value))
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

    private static bool IsArticleOrConjunctionOrPreposition(string word) =>
        word is

            // articles
            "a" or "an" or "the" or

            // conjunctions
            "and" or  "as" or  "but" or  "if" or  "nor" or  "or" or  "so" or  "yet" or

            // prepositions
            "as" or  "at" or  "by" or  "for" or  "in" or  "of" or  "off" or  "on" or  "to" or  "up" or  "via";
}