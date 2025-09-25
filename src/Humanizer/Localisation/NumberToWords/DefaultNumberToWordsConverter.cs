namespace Humanizer;

/// <summary>
/// Constructor.
/// </summary>
/// <param name="culture">Culture to use.</param>
class DefaultNumberToWordsConverter(CultureInfo? culture) : GenderlessNumberToWordsConverter
{
    readonly CultureInfo? culture = culture;

    /// <summary>
    /// 3501.ToWords() -> "three thousand five hundred and one"
    /// </summary>
    /// <param name="number">Number to be turned to words</param>
    public override string Convert(long number) =>
        number.ToString(culture);

    /// <summary>
    /// 1.ToOrdinalWords() -> "first"
    /// </summary>
    /// <param name="number">Number to be turned to ordinal words</param>
    public override string ConvertToOrdinal(int number) =>
        number.ToString(culture);
}