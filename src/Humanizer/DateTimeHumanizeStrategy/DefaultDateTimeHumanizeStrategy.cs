using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// The default 'distance of time' -> words calculator.
    /// </summary>
    public class DefaultDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates
        /// </summary>
        [NotNull]
        [PublicAPI]
        public string Humanize(DateTime input, DateTime comparisonBase, [NotNull] CultureInfo culture)
        {
            return DateTimeHumanizeAlgorithms.DefaultHumanize(input, comparisonBase, culture);
        }
    }
}