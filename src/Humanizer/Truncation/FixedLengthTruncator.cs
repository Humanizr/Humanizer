namespace Humanizer;

/// <summary>
/// Truncate a string to a fixed length
/// </summary>
class FixedLengthTruncator : ITruncator
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

        if (truncationString == null || truncationString.Length > length)
        {
            if (value.Length <= length)
            {
                return value;
            }

            return truncateFrom == TruncateFrom.Right
                ? value.Substring(0, length)
                : value.Substring(value.Length - length);
        }

        if (truncateFrom == TruncateFrom.Left)
        {
            return value.Length > length
                ? StringHumanizeExtensions.Concat(truncationString.AsSpan(), value.AsSpan(value.Length - length + truncationString.Length))
                : value;
        }

        return value.Length > length
            ? StringHumanizeExtensions.Concat(value.AsSpan(0, length - truncationString.Length), truncationString.AsSpan())
            : value;
    }
}