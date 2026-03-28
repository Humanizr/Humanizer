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
    /// Automatically select between <a href="https://en.wikipedia.org/wiki/Long_and_short_scales">long and short scale words</a>
    /// based on <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
    /// For example, <c>1E9</c> renders as <c>billion</c> in <c>en-US</c> and <c>milliard</c> in <c>de-DE</c>.
    /// </summary>
    UseScaleWord = 16
}