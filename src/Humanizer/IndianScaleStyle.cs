namespace Humanizer;

/// <summary>
/// Selects the large-number vocabulary used by <see cref="NumberToWordsExtension.ToIndianWords(long, IndianScaleStyle)"/>.
/// </summary>
public enum IndianScaleStyle
{
    /// <summary>
    /// Uses the named Indian scales, including arab, kharab, neel, padma, and shankh.
    /// </summary>
    NamedScales,

    /// <summary>
    /// Uses common crore-based expressions through lakh crore, while retaining named scales above that range.
    /// </summary>
    CroreBased
}