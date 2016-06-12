using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Humanizer.Configuration;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;
using Humanizer.TimeSpanHumanizeStrategy;

namespace Humanizer
{
    /// <summary>
    /// Humanizes TimeSpan into human readable form
    /// </summary>
    public static class TimeSpanHumanizeExtensions
    {
        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan">The time amount to be interpreted.</param>
        /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <returns>The given <paramref name="timeSpan"/> interpreted in a more friendly form.</returns>
        public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
        {
            var timeSpanData = new TimeSpanAnalysisParameters(timeSpan, precision, culture, maxUnit, minUnit, collectionSeparator: collectionSeparator);

            return Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpanData);
        }

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan">The time amount to be interpreted.</param>
        /// <param name="referenceTime">A reference in time from which the given <paramref name="timeSpan"/> is calculated.</param>
        /// <param name="periodBeforeReference">Denotes whether the <paramref name="timeSpan"/> happened before or after the given <paramref name="referenceTime"/>.</param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <returns>The given <paramref name="timeSpan"/> interpreted in a more friendly form.</returns>
        public static string Humanize(this TimeSpan timeSpan, DateTime referenceTime, bool periodBeforeReference, int precision = 1, bool countEmptyUnits = false, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
        {
            var timeSpanData = new TimeSpanAnalysisParameters(timeSpan, precision, culture, maxUnit, minUnit, referenceTime, periodBeforeReference, countEmptyUnits, collectionSeparator);
            
            return Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpanData);
        }

        /// <summary>
        /// Turns a TimeSpan into a human readable form. E.g. 1 day.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="precision">The maximum number of time units to return.</param>
        /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
        /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
        /// <param name="maxUnit">The maximum unit of time to output.</param>
        /// <param name="minUnit">The minimum unit of time to output.</param>
        /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
        /// <returns>The given <paramref name="timeSpan"/> interpreted in a more friendly form.</returns>
        public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string collectionSeparator = ", ")
        {
            var timeSpanData = new TimeSpanAnalysisParameters(timeSpan, precision, culture, maxUnit, minUnit, countEmptyUnits: countEmptyUnits, collectionSeparator: collectionSeparator);

            return Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpanData);
        }
    }
}