namespace Humanizer
{
    /// <summary>
    /// Provides hint for Humanizer as to whether a word is singular, plural or with unknown plurality
    /// </summary>
    public enum Plurality
    {
        /// <summary>
        /// The word is singular
        /// </summary>
        Singular,
        /// <summary>
        /// The word is plural
        /// </summary>
        Plural,
        /// <summary>
        /// I am unsure of the plurality
        /// </summary>
        CouldBeEither
    }
}