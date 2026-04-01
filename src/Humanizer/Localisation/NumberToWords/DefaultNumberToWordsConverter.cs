namespace Humanizer;

/// <summary>
/// Default fallback converter that formats numbers by delegating to the framework culture-aware
/// numeric formatter.
///
/// This implementation is intentionally minimal and is used when a locale does not provide a
/// specialized number-to-words renderer.
/// </summary>
/// <param name="culture">Culture to use.</param>
class DefaultNumberToWordsConverter(CultureInfo? culture) : GenderlessNumberToWordsConverter
{
    readonly CultureInfo? culture = culture;

    /// <summary>
    /// Converts the given value using the configured culture-aware numeric formatter.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The culture-formatted representation of <paramref name="number"/>.</returns>
    public override string Convert(long number) =>
        number.ToString(culture);

    /// <summary>
    /// Converts the given value using the configured culture-aware numeric formatter.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The culture-formatted representation of <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number) =>
        number.ToString(culture);
}
