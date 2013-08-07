namespace Humanizer
{
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