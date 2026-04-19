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
        for (var i = 0; i < matches.Count; i++)
        {
            var word = matches[i];
            if (AllCapitals(input, word.Index, word.Length) ||
                (i > 0 && IsArticleOrConjunctionOrPreposition(input.AsSpan(word.Index, word.Length))))
            {
                continue;
            }

            builder[word.Index] = textInfo.ToUpper(input[word.Index]);
            OverwriteLowercase(builder, input, word.Index + 1, word.Length - 1, textInfo);
        }

        return builder.ToString();
    }

    static void OverwriteLowercase(StringBuilder builder, string input, int index, int length, TextInfo textInfo)
    {
        if (ContainsNonAscii(input, index, length))
        {
            Overwrite(builder, index, textInfo.ToLower(input.Substring(index, length)));
            return;
        }

        var end = index + length;
        for (var i = index; i < end; i++)
        {
            builder[i] = textInfo.ToLower(input[i]);
        }
    }

    static void Overwrite(StringBuilder builder, int index, string replacement)
    {
        for (var i = 0; i < replacement.Length; i++)
        {
            builder[index + i] = replacement[i];
        }
    }

    static bool ContainsNonAscii(string input, int index, int length)
    {
        var end = index + length;
        for (var i = index; i < end; i++)
        {
            if (input[i] > '\u007F')
            {
                return true;
            }
        }

        return false;
    }

    static bool AllCapitals(string input, int index, int length)
    {
        var end = index + length;
        for (var i = index; i < end; i++)
        {
            if (!char.IsUpper(input[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsArticleOrConjunctionOrPreposition(ReadOnlySpan<char> word) =>
        word switch
        {
            "a" or "an" or "as" or "at" or "by" or "if" or "in" or "of" or "on" or "or" or "so" or "to" or "up" => true,
            "and" or "but" or "for" or "nor" or "off" or "the" or "via" or "yet" => true,
            _ => false
        };
}