#if NET6_0_OR_GREATER

using System;
using System.Globalization;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// The default 'distance of time' -> words calculator.
    /// </summary>
    public class DefaultTimeOnlyHumanizeStrategy : ITimeOnlyHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided times
        /// </summary>
        public string Humanize(TimeOnly input, TimeOnly comparisonBase, CultureInfo culture)
        {
            return DateTimeHumanizeAlgorithms.DefaultHumanize(input, comparisonBase, culture);
        }
    }
}

#endif
