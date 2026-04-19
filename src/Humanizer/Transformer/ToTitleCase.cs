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
        if (TryTransformAscii(input, culture, out var transformed))
        {
            return transformed;
        }

        return TransformWithRegex(input, culture);
    }

    static bool TryTransformAscii(string input, CultureInfo culture, [NotNullWhen(true)] out string? result)
    {
        result = null;
        if (ContainsNonAscii(input, 0, input.Length))
        {
            return false;
        }

        char[]? buffer = null;
        var textInfo = culture.TextInfo;
        var wordIndex = 0;
        for (var i = 0; i < input.Length;)
        {
            if (!IsAsciiWord(input[i]))
            {
                i++;
                continue;
            }

            var wordStart = i;
            i++;
            while (i < input.Length && IsAsciiWord(input[i]))
            {
                i++;
            }

            if (i < input.Length && input[i] == '\'')
            {
                i++;
                while (i < input.Length && IsAsciiWord(input[i]))
                {
                    i++;
                }
            }

            var wordLength = i - wordStart;
            if (AllCapitals(input, wordStart, wordLength) ||
                (wordIndex > 0 && IsArticleOrConjunctionOrPreposition(input.AsSpan(wordStart, wordLength))))
            {
                wordIndex++;
                continue;
            }

            SetCharIfChanged(input, ref buffer, wordStart, textInfo.ToUpper(input[wordStart]));
            for (var j = wordStart + 1; j < wordStart + wordLength; j++)
            {
                SetCharIfChanged(input, ref buffer, j, textInfo.ToLower(input[j]));
            }

            wordIndex++;
        }

        result = buffer is null ? input : new(buffer);
        return true;
    }

    static void SetCharIfChanged(string input, ref char[]? buffer, int index, char value)
    {
        if (input[index] == value)
        {
            return;
        }

        buffer ??= input.ToCharArray();
        buffer[index] = value;
    }

    static bool IsAsciiWord(char c) =>
        c is >= 'a' and <= 'z' ||
        c is >= 'A' and <= 'Z' ||
        c is >= '0' and <= '9' ||
        c == '_';

    static string TransformWithRegex(string input, CultureInfo culture)
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