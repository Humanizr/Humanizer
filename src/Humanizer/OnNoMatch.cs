namespace Humanizer
{
    /// <summary>
    /// Dictating what should be done when a match is not found - currently used only for DehumanizeTo
    /// </summary>
    public enum OnNoMatch
    {
        /// <summary>
        /// This is the default behavior which throws a NoMatchFoundException
        /// </summary>
        ThrowsException,

        /// <summary>
        /// If set to ReturnsNull the method returns null instead of throwing an exception
        /// </summary>
        ReturnsNull
    }
}