namespace Humanizer
{
    /// <summary>
    /// Truncate a string preserving the right most characters
    /// </summary>
    class RightJustifiedFixedLengthTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null || truncationString.Length > length)
                return value.Substring(value.Length - length, length);

            return value.Length > length ? truncationString + value.Substring(value.Length - length + truncationString.Length, length - truncationString.Length) : value;
        }
    }
}