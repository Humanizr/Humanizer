using System;
using System.Globalization;
using JetBrains.Annotations;

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
        [NotNull]
        [PublicAPI]
        public string Humanize(DateTimeOffset input, DateTimeOffset comparisonBase, [NotNull] CultureInfo culture)
        {
            return DateTimeHumanizeAlgorithms.DefaultHumanize(input.UtcDateTime, comparisonBase.UtcDateTime, culture);
        }
    }
}