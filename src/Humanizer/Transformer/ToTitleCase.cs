namespace Humanizer;

partial class ToTitleCase : ICulturedStringTransformer
{
    public string Transform(string input) =>
        Transform(input, CultureInfo.CurrentCulture);

    private const string WordPattern = @"(\w|[^\u0000-\u007F])+'?\w*";

#if NET7_0_OR_GREATER
    [GeneratedRegex(WordPattern)]
    private static partial Regex WordRegexGenerated();

    private static Regex WordRegex() => WordRegexGenerated();
#else
    private static readonly Regex WordRegexDefinition = new(WordPattern, RegexOptions.Compiled);

    private static Regex WordRegex() => WordRegexDefinition;
#endif

    public string Transform(string input, CultureInfo culture)
    {
        var matches = WordRegex().Matches(input);
        var builder = new StringBuilder(input);
        var textInfo = culture.TextInfo;
        foreach (Match word in matches)
        {
            var value = word.Value;
            var isSentenceStart = IsSentenceStart(input, word.Index);

            if (AllCapitals(value))
            {
                continue;
            }

            if (IsArticleOrConjunctionOrPreposition(value) && !isSentenceStart)
            {
                continue;
            }


            builder[word.Index] = textInfo.ToUpper(value[0]);
            Overwrite(builder, word.Index + 1, textInfo.ToLower(value[1..]));
        }

        return builder.ToString();
    }

    static void Overwrite(StringBuilder builder, int index, string replacement)
    {
        // Directly overwrite characters instead of Remove + Insert
        for (var i = 0; i < replacement.Length; i++)
        {
            builder[index + i] = replacement[i];
        }
    }

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
            "and" or "as" or "but" or "if" or "nor" or "or" or "so" or "yet" or

            // prepositions
            "as" or "at" or "by" or "for" or "in" or "of" or "off" or "on" or "to" or "up" or "via";


    static bool IsSentenceStart(string input, int wordIndex)
    {
        if (wordIndex == 0)
        {
            return true;
        }

        for (var i = wordIndex - 1; i >= 0; i--)
        {
            var ch = input[i];

            if (char.IsWhiteSpace(ch))
            {
                continue;
            }

            return ch is '.' or '!' or '?';
        }

        return true;
    }

}