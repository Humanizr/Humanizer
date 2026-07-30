namespace Humanizer;

/// <summary>
/// Flags for formatting the metric representation of numerals.
/// </summary>
[Flags]
public enum MetricNumeralFormats
{
    /// <summary>
    /// Use the metric prefix <a href="https://en.wikipedia.org/wiki/Long_and_short_scales">long scale word</a>.
    /// </summary>
    UseLongScaleWord = 1,

    /// <summary>
    /// Use the metric prefix <a href="https://en.wikipedia.org/wiki/Metric_prefix#List_of_SI_prefixes">name</a> instead of the symbol.
    /// </summary>
    UseName = 2,

    /// <summary>
    /// Use the metric prefix <a href="https://en.wikipedia.org/wiki/Long_and_short_scales">short scale word</a>.
    /// </summary>
    UseShortScaleWord = 4,

    /// <summary>
    /// Include a space after the numeral.
    /// </summary>
    WithSpace = 8,

    /// <summary>
    /// Use the scale word authored for <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
    /// When that locale has no standalone word for the selected power of 1000, use the SI symbol.
    /// The locale-authored grammatical count form follows the displayed, scaled numeral.
    /// Inverse scale words are used only when the authored singular form applies; other counts use the SI symbol.
    /// For example, <c>1E9</c> renders as <c>billion</c> in <c>en-US</c> and <c>Milliarde</c> in <c>de-DE</c>.
    /// </summary>
    UseScaleWord = 16,

    /// <summary>
    /// Include trailing zeros so the number of fractional digits matches the <c>decimals</c> argument.
    /// Has no effect when <c>decimals</c> is <see langword="null"/>.
    /// </summary>
    KeepTrailingZeros = 32
}