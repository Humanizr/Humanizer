using System;
using System.Globalization;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// Implement this interface to create a new strategy for DateTime.Humanize and hook it in the Configurator.DateTimeHumanizeStrategy
    /// </summary>
    public interface IDateTimeHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates used for DateTime.Humanize
        /// </summary>
        string Humanize(DateTime input, DateTime comparisonBase, CultureInfo culture);
    }
}