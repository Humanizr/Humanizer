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
        
        // Check if this appears to be a multi-word result from humanizing separator-based input
        bool isMultiWordFromSeparators = ContainsMultipleAllCapsWords(input);
        
        foreach (Match word in matches)
        {
            var value = word.Value;
            if ((AllCapitals(value, isMultiWordFromSeparators)) || IsArticleOrConjunctionOrPreposition(value))
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

    static bool AllCapitals(string input, bool isMultiWordFromSeparators = false)
    {
        foreach (var ch in input)
        {
            if (!char.IsUpper(ch))
            {
                return false;
            }
        }

        // If this appears to be from separator-based input, be more restrictive about preserving acronyms
        if (isMultiWordFromSeparators)
        {
            // Only preserve very short words like "I", "IT", "TV"
            return input.Length <= 3 && !input.Contains('_') && !input.Contains('-');
        }

        // For single words or mixed contexts, preserve potential acronyms more generously
        // This preserves "HELLO", "STRAẞE", "ALLCAPS" etc.
        return !input.Contains('_') && !input.Contains('-');
    }
    
    static bool ContainsMultipleAllCapsWords(string input)
    {
        // Check if input contains multiple ALL-CAPS words separated by spaces
        // This suggests it came from humanizing separator-based input like "LONGER_WORD" → "LONGER WORD"
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length < 2) return false;
        
        int allCapsCount = 0;
        int totalWordCount = 0;
        foreach (var word in words)
        {
            if (word.Any(char.IsLetter))
            {
                totalWordCount++;
                if (word.All(char.IsUpper))
                {
                    allCapsCount++;
                }
            }
        }
        
        // Only consider it "from separators" if ALL or most words are ALL-CAPS
        // This catches "LONGER WORD" but not "Title humanization Honors ALLCAPS"
        return allCapsCount >= 2 && allCapsCount >= totalWordCount * 0.75;
    }

    private static bool IsArticleOrConjunctionOrPreposition(string word) =>
        word is

            // articles
            "a" or "an" or "the" or

            // conjunctions
            "and" or "as" or "but" or "if" or "nor" or "or" or "so" or "yet" or

            // prepositions
            "as" or "at" or "by" or "for" or "in" or "of" or "off" or "on" or "to" or "up" or "via";
}