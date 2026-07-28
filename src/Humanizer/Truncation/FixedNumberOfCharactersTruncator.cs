namespace Humanizer;

/// <summary>
/// Truncate a string to a fixed number of letters or digits
/// </summary>
class FixedNumberOfCharactersTruncator : ITruncator
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

        truncationString ??= string.Empty;

        if (truncationString.Length > length)
        {
            return truncateFrom == TruncateFrom.Right ? value[..length] : value[^length..];
        }

        // Count letter or digit characters up to length + 1 to determine if truncation is needed
        var alphaNumericalCount = 0;
        foreach (var c in value)
        {
            if (char.IsLetterOrDigit(c))
            {
                alphaNumericalCount++;
                if (alphaNumericalCount > length)
                {
                    break;
                }
            }
        }

        if (alphaNumericalCount <= length)
        {
            return value;
        }

        var alphaNumericalCharactersProcessed = 0;
        if (truncateFrom == TruncateFrom.Left)
        {
            for (var i = value.Length - 1; i > 0; i--)
            {
                if (char.IsLetterOrDigit(value[i]))
                {
                    alphaNumericalCharactersProcessed++;
                }

                if (alphaNumericalCharactersProcessed + truncationString.Length == length)
                {
                    return StringHumanizeExtensions.Concat(truncationString.AsSpan(), value.AsSpan(i));
                }
            }
        }

        for (var i = 0; i < value.Length - truncationString.Length; i++)
        {
            if (char.IsLetterOrDigit(value[i]))
            {
                alphaNumericalCharactersProcessed++;
            }

            if (alphaNumericalCharactersProcessed + truncationString.Length == length)
            {
                return StringHumanizeExtensions.Concat(value.AsSpan(0, i + 1), truncationString.AsSpan());
            }
        }

        return value;
    }
}