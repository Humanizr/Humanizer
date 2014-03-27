namespace Humanizer
{
    /// <summary>
    /// Options for specifying the desired truncate mode for the output string 
    /// </summary>
    public enum TruncateMode
    {
        /// <summary>
        /// This is some text -> This is s…
        /// </summary>
        FixedLength,

        /// <summary>
        /// This is some text -> This is so…
        /// </summary>
        FixedNumberOfCharacters,

        /// <summary>
        /// This is some text -> This is …
        /// </summary>
        FixedNumberOfWords,
    }
}