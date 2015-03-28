using System;
using System.Globalization;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// Implement this interface to create a new strategy for DateTime.Humanize and hook it in the Configurator.DateTimeOffsetHumanizeStrategy
    /// </summary>
    public interface IDateTimeOffsetHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates used for DateTimeOffset.Humanize
        /// </summary>
        string Humanize(DateTimeOffset input, DateTimeOffset comparisonBase, CultureInfo culture);
    }
}