namespace Humanizer;

/// <summary>
/// Truncate a string to a fixed number of words
/// </summary>
class FixedNumberOfWordsTruncator : ITruncator
{
    [return: NotNullIfNotNull(nameof(value))]
    public string? Truncate(string? value, int length, string? truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
    {
        if (value == null)
        {
            return null;
        }

        if (value.Length == 0)
        {
            return value;
        }

        // Count words without allocating array
        var numberOfWords = 0;
        var wasWhiteSpace = true;
        foreach (var c in value)
        {
            if (char.IsWhiteSpace(c))
            {
                wasWhiteSpace = true;
            }
            else if (wasWhiteSpace)
            {
                numberOfWords++;
                wasWhiteSpace = false;
            }
        }

        if (numberOfWords <= length)
        {
            return value;
        }

        return truncateFrom == TruncateFrom.Left
            ? TruncateFromLeft(value, length, truncationString)
            : TruncateFromRight(value, length, truncationString);
    }

    static string TruncateFromRight(string value, int length, string? truncationString)
    {
        var lastCharactersWasWhiteSpace = true;
        var numberOfWordsProcessed = 0;
        for (var i = 0; i < value.Length; i++)
        {
            if (char.IsWhiteSpace(value[i]))
            {
                if (!lastCharactersWasWhiteSpace)
                {
                    numberOfWordsProcessed++;
                }

                lastCharactersWasWhiteSpace = true;

                if (numberOfWordsProcessed == length)
                {
                    return StringHumanizeExtensions.Concat(value.AsSpan(0, i), truncationString.AsSpan());
                }
            }
            else
            {
                lastCharactersWasWhiteSpace = false;
            }
        }
        return value + truncationString;
    }

    static string TruncateFromLeft(string value, int length, string? truncationString)
    {
        var lastCharactersWasWhiteSpace = true;
        var numberOfWordsProcessed = 0;
        for (var i = value.Length - 1; i > 0; i--)
        {
            if (char.IsWhiteSpace(value[i]))
            {
                if (!lastCharactersWasWhiteSpace)
                {
                    numberOfWordsProcessed++;
                }

                lastCharactersWasWhiteSpace = true;

                if (numberOfWordsProcessed == length)
                {
                    return StringHumanizeExtensions.Concat(truncationString.AsSpan(), value.AsSpan(i + 1).TrimEnd());
                }
            }
            else
            {
                lastCharactersWasWhiteSpace = false;
            }
        }
        return truncationString + value;
    }
}