#if NET6_0_OR_GREATER

using System;
using System.Globalization;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// Precision-based calculator for distance between two times
    /// </summary>
    public class PrecisionTimeOnlyHumanizeStrategy : ITimeOnlyHumanizeStrategy
    {
        private readonly double _precision;

        /// <summary>
        /// Constructs a precision-based calculator for distance of time with default precision 0.75.
        /// </summary>
        /// <param name="precision">precision of approximation, if not provided  0.75 will be used as a default precision.</param>
        public PrecisionTimeOnlyHumanizeStrategy(double precision = .75)
        {
            _precision = precision;
        }

        /// <summary>
        /// Returns localized &amp; humanized distance of time between two dates; given a specific precision.
        /// </summary>
        public string Humanize(TimeOnly input, TimeOnly comparisonBase, CultureInfo culture)
        {
            return DateTimeHumanizeAlgorithms.PrecisionHumanize(input, comparisonBase, _precision, culture);
        }
    }
}

#endif
