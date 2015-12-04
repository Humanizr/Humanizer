using System;
using System.Globalization;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// The default 'distance of time' -> words calculator.
    /// </summary>
    public class DefaultDateTimeOffsetHumanizeStrategy : IDateTimeOffsetHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates
        /// </summary>
        public string Humanize(DateTimeOffset input, DateTimeOffset comparisonBase, CultureInfo culture)
        {
            return DateTimeHumanizeAlgorithms.DefaultHumanize(input.UtcDateTime, comparisonBase.UtcDateTime, culture);
        }

        /// <summary>
        /// Calculates the distance of time in words between two provided dates
        /// </summary>
        public string Humanize(DateTimeOffset? input, DateTimeOffset comparisonBase, CultureInfo culture)
        {
            if (input.HasValue)
                return DateTimeHumanizeAlgorithms.DefaultHumanize(input.Value.UtcDateTime, comparisonBase.UtcDateTime, culture);
            else
                return DateTimeHumanizeAlgorithms.DefaultHumanize(null, comparisonBase.UtcDateTime, culture);
        }
    }
}