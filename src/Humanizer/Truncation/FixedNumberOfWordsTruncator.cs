using System;
using System.Linq;

namespace Humanizer
{
    /// <summary>
    /// Truncate a string to a fixed number of words
    /// </summary>
    class FixedNumberOfWordsTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString)
        {
            if (value == null)
                return null;

            if (value.Length == 0)
                return value;

            var numberOfWordsProcessed = 0;
            var numberOfWords = value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();

            if (numberOfWords <= length)
                return value;

            var lastCharactersWasWhiteSpace = true;

            for (var i = 0; i < value.Length; i++)
            {
                if (Char.IsWhiteSpace(value[i]))
                {
                    if (!lastCharactersWasWhiteSpace)
                        numberOfWordsProcessed++;

                    lastCharactersWasWhiteSpace = true;

                    if (numberOfWordsProcessed == length)
                        return value.Substring(0, i) + truncationString;
                }
                else
                {
                    lastCharactersWasWhiteSpace = false;
                }
            }

            return value + truncationString;
        }
    }
}