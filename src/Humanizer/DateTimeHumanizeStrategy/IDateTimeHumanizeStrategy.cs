using System;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// A strategy for converting a 'distance of time' into words.
    /// </summary>
    public interface IDateTimeHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates.
        /// </summary>
        string Humanize(DateTime input, DateTime comparisonBase);
    }
}