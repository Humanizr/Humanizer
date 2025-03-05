namespace Humanizer;

/// <summary>
/// Truncate a string to a fixed length while preserving whole words.
/// If the truncation point falls in the middle of a word,
/// that word is dropped entirely and the delimiter is attached.
/// For left-truncation, if no complete word fits in the allowed space,
/// only the delimiter is returned.
/// </summary>
class DynamicLengthAndPreserveWordsTruncator : ITruncator
{
    [return: NotNullIfNotNull(nameof(value))]
    public string? Truncate(string? value, int length, string? truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
    {
        if (value == null)
            return null;
        if (value.Length == 0)
            return value;

        // For this scenario we expect a single-character delimiter.
        // If truncation string is null, treat it as empty.
        if (truncationString == null)
            truncationString = string.Empty;

        // If the delimiter itself is longer than the allowed length, fall back to a basic substring.
        if (truncationString.Length > length)
        {
            return truncateFrom == TruncateFrom.Right
                ? value.Substring(0, length)
                : value.Substring(value.Length - length);
        }

        // If the whole string fits, return it.
        if (value.Length <= length)
            return value;

        return truncateFrom == TruncateFrom.Right
            ? TruncateFromRight(value, length, truncationString)
            : TruncateFromLeft(value, length, truncationString);
    }

    static string TruncateFromLeft(string value, int length, string truncationString)
    {
        // For left truncation, the final output is: delimiter + substring.
        // Determine how many characters (after the delimiter) are allowed.
        var allowedContentLength = length - truncationString.Length;
        if (allowedContentLength <= 0)
            return truncationString;

        // We'll scan backward from the end of the string for a whitespace boundary
        // such that the substring (after trimming) is no longer than allowedContentLength.
        var candidateStart = value.Length;
        for (var i = value.Length - 1; i >= 0; i--)
        {
            if (char.IsWhiteSpace(value[i]))
            {
                candidateStart = i + 1;
                var candidateLength = value.Length - candidateStart;
                if (candidateLength <= allowedContentLength)
                {
                    break;
                }
            }
        }
        var candidate = value.Substring(candidateStart).TrimStart();
        // If the candidate word is too long (i.e. would be partial) or empty, return just the delimiter.
        if (candidate.Length > allowedContentLength || candidate.Length == 0)
            return truncationString;
        return truncationString + candidate;
    }

    static string TruncateFromRight(string value, int length, string truncationString)
    {
        var effectiveLength = length - truncationString.Length;
        if (effectiveLength <= 0)
            return truncationString;
        // If the cutoff falls in the middle of a word, backtrack to the last space.
        if (effectiveLength < value.Length && !char.IsWhiteSpace(value[effectiveLength]))
        {
            var lastSpace = value.LastIndexOf(' ', effectiveLength);
            if (lastSpace > 0)
                effectiveLength = lastSpace;
            else
                return truncationString;
        }
        var prefix = value.Substring(0, effectiveLength).TrimEnd();
        if (prefix.Length == 0)
            return truncationString;
        return prefix + truncationString;
    }
}