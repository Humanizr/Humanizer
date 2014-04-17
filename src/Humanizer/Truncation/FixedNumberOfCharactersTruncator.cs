using System;
using System.Linq;

namespace Humanizer
{
    /// <summary>
    /// Truncate a string to a fixed number of letters or digits
    /// </summary>
    class FixedNumberOfCharactersTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null || truncationString.Length > length)
                return truncateFrom == TruncateFrom.Right ? value.Substring(0, length) : value.Substring(value.Length - length);

            var alphaNumericalCharactersProcessed = 0;

            if (value.ToCharArray().Count(Char.IsLetterOrDigit) <= length)
                return value;

            if (truncateFrom == TruncateFrom.Left)
            {
                for (var i = value.Length - 1; i > 0; i--)
                {
                    if (Char.IsLetterOrDigit(value[i]))
                        alphaNumericalCharactersProcessed++;

                    if (alphaNumericalCharactersProcessed + truncationString.Length == length)
                        return truncationString + value.Substring(i);
                }         
            }

            for (var i = 0; i < value.Length - truncationString.Length; i++)
            {
                if (Char.IsLetterOrDigit(value[i]))
                    alphaNumericalCharactersProcessed++;

                if (alphaNumericalCharactersProcessed + truncationString.Length == length)
                    return value.Substring(0, i + 1) + truncationString;
            }

            return value;
        }
    }
}