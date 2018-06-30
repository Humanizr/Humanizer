using System;
using System.Linq;

namespace Humanizer
{
    /// <summary>
    /// Truncate a string to a fixed number of words
    /// </summary>
    internal class FixedNumberOfWordsTruncator : ITruncator
    {
        public string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right)
        {
            if (value == null)
            {
                return null;
            }

            if (value.Length == 0)
            {
                return value;
            }

            var numberOfWords = value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();
            if (numberOfWords <= length)
            {
                return value;
            }

            return truncateFrom == TruncateFrom.Left
                ? TruncateFromLeft(value, length, truncationString)
                : TruncateFromRight(value, length, truncationString);
        }

        private static string TruncateFromRight(string value, int length, string truncationString)
        {
            var lastCharactersWasWhiteSpace = true;
            var numberOfWordsProcessed = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                {
                    if (!lastCharactersWasWhiteSpace)
                    {
                        numberOfWordsProcessed++;
                    }

                    lastCharactersWasWhiteSpace = true;

                    if (numberOfWordsProcessed == length)
                    {
                        return value.Substring(0, i) + truncationString;
                    }
                }
                else
                {
                    lastCharactersWasWhiteSpace = false;
                }
            }
            return value + truncationString;
        }

        private static string TruncateFromLeft(string value, int length, string truncationString)
        {
            var lastCharactersWasWhiteSpace = true;
            var numberOfWordsProcessed = 0;
            for (var i = value.Length - 1; i > 0; i--)
            {
                if (char.IsWhiteSpace(value[i]))
                {
                    if (!lastCharactersWasWhiteSpace)
                    {
                        numberOfWordsProcessed++;
                    }

                    lastCharactersWasWhiteSpace = true;

                    if (numberOfWordsProcessed == length)
                    {
                        return truncationString + value.Substring(i + 1).TrimEnd();
                    }
                }
                else
                {
                    lastCharactersWasWhiteSpace = false;
                }
            }
            return truncationString + value;
        }
    }
}