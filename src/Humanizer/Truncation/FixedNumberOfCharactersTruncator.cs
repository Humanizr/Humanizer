using System;
using System.Linq;

namespace Humanizer
{
    /// <summary>
    /// Truncate a string to a fixed number of characters
    /// </summary>
    class FixedNumberOfCharactersTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            if (truncationString == null || truncationString.Length > length)
                return value.Substring(0, length);

            var alphaNumericalCharactersProcessed = 0;

            var numberOfCharactersEqualToTruncateLength = value.ToCharArray().Count(Char.IsLetterOrDigit) == length;

            for (var i = 0; i < value.Length - truncationString.Length; i++)
            {
                if (Char.IsLetterOrDigit(value[i]))
                    alphaNumericalCharactersProcessed++;

                if (numberOfCharactersEqualToTruncateLength && alphaNumericalCharactersProcessed == length)
                    return value;

                if (!numberOfCharactersEqualToTruncateLength && alphaNumericalCharactersProcessed + truncationString.Length == length)
                    return value.Substring(0, i + 1) + truncationString;
            }

            return value;
        }
    }
}