namespace Humanizer
{
    /// <summary>
    /// Truncate a string to a fixed length
    /// </summary>
    class FixedLengthTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null || truncationString.Length > length)
                return value.Substring(0, length);

            return value.Length > length ? value.Substring(0, length - truncationString.Length) + truncationString : value;
        }
    }
}