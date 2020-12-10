using System;

namespace Humanizer
{
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
        WithSpace = 8
    }
}
