namespace Humanizer
{
    /// <summary>
    /// Options for specifying the desired letter casing for the output string 
    /// </summary>
    public enum LetterCasing
    {
        /// <summary>
        /// SomeString -> Some String
        /// </summary>
        Title,
        /// <summary>
        /// SomeString -> SOME STRING
        /// </summary>
        AllCaps,
        /// <summary>
        /// SomeString -> some string
        /// </summary>
        LowerCase,
        /// <summary>
        /// SomeString -> Some string
        /// </summary>
        Sentence,
    }
}