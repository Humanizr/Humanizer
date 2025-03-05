namespace Humanizer;

/// <summary>
/// Truncate a string to a fixed number of letters or digits,
/// preserving whole words by never cutting a word in half.
/// If a complete word (plus the delimiter, if any) cannot fit, then only the delimiter is returned.
/// When truncating from the left, the delimiter is prepended if a complete word can be preserved;
/// otherwise, only the delimiter is returned.
/// The allowed count is computed by counting only letters/digits.
/// </summary>
public class DynamicNumberOfCharactersAndPreserveWordsTruncator : ITruncator
{
    [return: NotNullIfNotNull(nameof(value))]
    public string? Truncate(string? value, int totalLength, string? delimiter, TruncateFrom truncateFrom = TruncateFrom.Right)
    {
        if (value == null)
            return null;
        if (value.Length == 0)
            return value;
        // Treat a null delimiter as an empty string.
        delimiter ??= string.Empty;
        // If the delimiter itself is longer than totalLength, fallback to a plain substring.
        if (delimiter.Length > totalLength && totalLength >= value.Length)
            return value;

        // Count all alphanumeric characters. If they already fit, return full string.
        var totalAlpha = value.Count(char.IsLetterOrDigit);
        if (totalAlpha <= totalLength)
            return value;

        // Delegate to the appropriate helper.
        return truncateFrom == TruncateFrom.Right
            ? TruncateRight(value, totalLength, delimiter)
            : TruncateLeft(value, totalLength, delimiter);
    }

    static string TruncateRight(string value, int totalLength, string delimiter)
    {
        var dLen = delimiter.Length;

        var alphaCount = 0;
        var candidateIndex = -1;
        var lastSpace = -1;

        for (var i = 0; i < value.Length; i++)
        {
            if (char.IsWhiteSpace(value[i]))
                lastSpace = i;
            if (char.IsLetterOrDigit(value[i]))
                alphaCount++;
            if (alphaCount + dLen == totalLength)
            {
                candidateIndex = i + 1;
                break;
            }

            if (alphaCount == totalLength) // if alpha count without delimiter reached total length
            {
                candidateIndex = i;
                break;
            }
        }
        if (candidateIndex == -1)
            candidateIndex = value.Length;

        // If the candidate index is in the middle of a word, backtrack to last whitespace.
        if (candidateIndex < value.Length && !char.IsWhiteSpace(value[candidateIndex]))
        {
            if (lastSpace > 0)
                candidateIndex = lastSpace;
            else if (delimiter.Length > totalLength)
                return string.Empty;
            else
                return delimiter; // no complete word fits
        }

        // Get the substring and trim trailing spaces.
        var prefix = value.Substring(0, candidateIndex).TrimEnd();

        var prefixLength = 0;
        for (var i = prefix.Length - 1; i >= 0; i--)
            if (char.IsLetterOrDigit(prefix[i]))
                prefixLength++;

        if (prefixLength == 0)
            return delimiter;
        if (delimiter.Length > totalLength && prefixLength > totalLength)
            return string.Empty;
        if (delimiter.Length > prefixLength && delimiter.Length > totalLength)
            return prefix;

        if (delimiter.Length <= totalLength && delimiter.Length + prefixLength > totalLength)
            return delimiter;

        return prefix + delimiter;
    }

    static string TruncateLeft(string value, int totalLength, string delimiter)
    {
        var dLen = delimiter.Length;

        var alphaCount = 0;
        var candidateIndex = -1;
        var nextSpace = -1;
        // Iterate backwards.
        for (var i = value.Length - 1; i >= 0; i--)
        {
            if (char.IsLetterOrDigit(value[i]))
                alphaCount++;
            if (char.IsWhiteSpace(value[i]))
                nextSpace = i;
            if (alphaCount + dLen == totalLength)
            {
                candidateIndex = i;
                break;
            }

            if (alphaCount == totalLength) // if alpha count without delimiter reached total length
            {
                candidateIndex = i;
                break;
            }
        }
        if (candidateIndex == -1)
            candidateIndex = 0;

        // If candidateIndex is in the middle of a word, move forward to the next whitespace.
        if (candidateIndex < value.Length && candidateIndex > 0 && !char.IsWhiteSpace(value[candidateIndex - 1]))
        {
            if (nextSpace >= candidateIndex)
                candidateIndex = nextSpace + 1;
            else if (delimiter.Length > totalLength)
                return string.Empty;
            else
                return delimiter; // no complete word fits
        }

        var suffix = value.Substring(candidateIndex).TrimStart();

        var suffixLength = 0;
        for (var i = suffix.Length - 1; i >= 0; i--)
            if (char.IsLetterOrDigit(suffix[i]))
                suffixLength++;

        if (suffixLength == 0)
            return delimiter;
        if (delimiter.Length > totalLength && suffixLength > totalLength)
            return string.Empty;
        if (delimiter.Length > suffixLength && delimiter.Length > totalLength)
            return suffix;

        if (delimiter.Length <= totalLength && delimiter.Length + suffixLength > totalLength)
            return delimiter;

        return delimiter + suffix;
    }
}