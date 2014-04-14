using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer
{
    class NoMoreWordsAfterNumberOfCharactersTruncator : ITruncator
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
                while (Char.IsLetterOrDigit(value, length))
                {
                    length++;
                }
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
                    while (Char.IsLetterOrDigit(value, i+1))
                    {
                        i++;
                    }
                    return value.Substring(0, i+1) + truncationString;
                }
            }
            return null;
        }
    }
}
