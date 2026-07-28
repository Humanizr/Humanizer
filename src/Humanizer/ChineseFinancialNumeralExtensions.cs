namespace Humanizer;

/// <summary>
/// Contains extension methods for converting integers to Chinese financial characters.
/// </summary>
public static class ChineseFinancialNumeralExtensions
{
    static readonly EastAsianGroupedNumberToWordsConverter SimplifiedConverter = CreateConverter(
        "负",
        ["零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"],
        ["", "万", "亿", "兆", "京"]);

    static readonly EastAsianGroupedNumberToWordsConverter TraditionalConverter = CreateConverter(
        "負",
        ["零", "壹", "貳", "參", "肆", "伍", "陸", "柒", "捌", "玖"],
        ["", "萬", "億", "兆", "京"]);

    /// <summary>
    /// Converts the given value to Chinese financial characters.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <param name="culture">
    /// A culture in the <c>zh-Hans</c> or <c>zh-Hant</c> hierarchy, which selects simplified or
    /// traditional financial characters.
    /// </param>
    /// <returns>The value written with Chinese financial digits and units.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">
    /// Thrown when <paramref name="culture"/> does not resolve to simplified or traditional Chinese.
    /// </exception>
    public static string ToChineseFinancialCharacters(this int number, CultureInfo culture) =>
        ((long)number).ToChineseFinancialCharacters(culture);

    /// <summary>
    /// Converts the given value to Chinese financial characters.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <param name="culture">
    /// A culture in the <c>zh-Hans</c> or <c>zh-Hant</c> hierarchy, which selects simplified or
    /// traditional financial characters.
    /// </param>
    /// <returns>The value written with Chinese financial digits and units.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">
    /// Thrown when <paramref name="culture"/> does not resolve to simplified or traditional Chinese.
    /// </exception>
    /// <remarks>
    /// Supports the full <see cref="long"/> range. This method does not add currency names or units
    /// such as yuan, jiao, or fen.
    /// </remarks>
    public static string ToChineseFinancialCharacters(this long number, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(culture);

        for (var current = culture; !string.IsNullOrEmpty(current.Name); current = current.Parent)
        {
            if (current.Name == "zh-Hans")
            {
                return SimplifiedConverter.Convert(number);
            }

            if (current.Name == "zh-Hant")
            {
                return TraditionalConverter.Convert(number);
            }
        }

        throw new NotSupportedException(
            $"Culture '{culture.Name}' does not resolve to simplified or traditional Chinese.");
    }

    static EastAsianGroupedNumberToWordsConverter CreateConverter(
        string negativePrefix,
        string[] digitWords,
        string[] largeUnits) =>
        new(new(
            "零",
            negativePrefix,
            "",
            "",
            digitWords,
            ["", "拾", "佰", "仟"],
            largeUnits,
            false,
            false,
            false,
            false,
            true,
            true));
}