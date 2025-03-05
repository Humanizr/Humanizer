namespace Humanizer;

/// <summary>
/// Gets a ITruncator
/// </summary>
public static class Truncator
{
    /// <summary>
    /// Fixed length truncator
    /// </summary>
    public static ITruncator FixedLength { get; } = new FixedLengthTruncator();

    /// <summary>
    /// Fixed number of characters truncator
    /// </summary>
    public static ITruncator FixedNumberOfCharacters { get; } = new FixedNumberOfCharactersTruncator();

    /// <summary>
    /// Fixed number of words truncator
    /// </summary>
    public static ITruncator FixedNumberOfWords { get; } = new FixedNumberOfWordsTruncator();

    /// <summary>
    /// Dynamic length And Preserve Words truncator
    /// </summary>
    public static ITruncator DynamicLengthAndPreserveWords { get; } = new DynamicLengthAndPreserveWordsTruncator();

    /// <summary>
    /// Dynamic number of characters And Preserve Words truncator
    /// </summary>
    public static ITruncator DynamicNumberOfCharactersAndPreserveWords { get; } = new DynamicNumberOfCharactersAndPreserveWordsTruncator();
}