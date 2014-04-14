using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer
{
    class MaximumLengthWithoutWordBreak : ITruncator
    {
        public string Truncate(string value, int length, string truncationString)
        {
            if (value == null)
                return null;

            if (value.Length == 0 || length >= value.Length)
                return value;

            var alphaNumericalCharactersProcessed = 0;

            var numberOfCharactersEqualToTruncateLength = value.ToCharArray().Count(Char.IsLetterOrDigit) == length;

            if (truncationString == null || truncationString.Length > length)
            {
                while (Char.IsLetterOrDigit(value, length) && length > 0)
                    length--;
                return value.Substring(0, length);
            }

            for (var i = 0; i < value.Length - truncationString.Length; i++)
            {
                if (Char.IsLetterOrDigit(value[i]))
                    alphaNumericalCharactersProcessed++;

                if (numberOfCharactersEqualToTruncateLength && alphaNumericalCharactersProcessed == length)
                    return value;

                if (!numberOfCharactersEqualToTruncateLength && alphaNumericalCharactersProcessed + truncationString.Length == length)
                {
                    while (Char.IsLetterOrDigit(value, i) && i > 0)
                        i--;
                    return value.Substring(0, i) + truncationString;
                }
            }
            return null;
        }
    }
}
