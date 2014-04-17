namespace Humanizer
{
    /// <summary>
    /// Truncate a string to a fixed length
    /// </summary>
    class FixedLengthTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString, Truncator.TruncateFrom truncateFrom = Truncator.TruncateFrom.Right)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null || truncationString.Length > length)
            {
                return truncateFrom == Truncator.TruncateFrom.Right ? value.Substring(0, length) : value.Substring(value.Length - length);
            }

            if (truncateFrom == Truncator.TruncateFrom.Left)
                return value.Length > length
                    ? truncationString + value.Substring(value.Length - length + truncationString.Length)
                    : value;

            return value.Length > length
                ? value.Substring(0, length - truncationString.Length) + truncationString
                : value;
        }
    }
}