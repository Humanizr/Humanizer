using JetBrains.Annotations;

namespace Humanizer
{
    /// <summary>
    /// Gets a ITruncator
    /// </summary>
    public static class Truncator
    {
        /// <summary>
        /// Fixed length truncator
        /// </summary>
        [NotNull]
        public static ITruncator FixedLength
        {
            get
            {
                return new FixedLengthTruncator();
            }
        }

        /// <summary>
        /// Fixed number of characters truncator
        /// </summary>
        [NotNull]
        public static ITruncator FixedNumberOfCharacters
        {
            get
            {
                return new FixedNumberOfCharactersTruncator();
            }
        }

        /// <summary>
        /// Fixed number of words truncator
        /// </summary>
        [NotNull]
        public static ITruncator FixedNumberOfWords
        {
            get
            {
                return new FixedNumberOfWordsTruncator();
            }
        }
    }
}
