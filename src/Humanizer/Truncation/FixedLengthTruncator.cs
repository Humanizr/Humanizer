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

        if (value.Length == 0 || value.Length <= length)
        {
            return value;
        }

        if (truncationString == null || truncationString.Length > length)
        {
            return truncateFrom == TruncateFrom.Right
                ? value[..Truncator.PrefixLength(value, length)]
                : value[Truncator.SuffixStart(value, value.Length - length)..];
        }

        return truncateFrom == TruncateFrom.Left ? StringHumanizeExtensions.Concat(truncationString.AsSpan(), value.AsSpan(Truncator.SuffixStart(value, value.Length - length + truncationString.Length))) : StringHumanizeExtensions.Concat(value.AsSpan(0, Truncator.PrefixLength(value, length - truncationString.Length)), truncationString.AsSpan());
    }
}