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

    // Moves a cut that would land between the halves of a surrogate pair to before the pair.
    internal static int PrefixLength(string value, int length) =>
        length > 0 && length < value.Length && char.IsSurrogatePair(value[length - 1], value[length]) ? length - 1 : length;

    // Moves a cut that would land between the halves of a surrogate pair to after the pair.
    internal static int SuffixStart(string value, int start) =>
        start > 0 && start < value.Length && char.IsSurrogatePair(value[start - 1], value[start]) ? start + 1 : start;
}