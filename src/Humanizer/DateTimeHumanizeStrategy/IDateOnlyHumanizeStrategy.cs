#if NET6_0_OR_GREATER

using System;
using System.Globalization;

namespace Humanizer.DateTimeHumanizeStrategy
{
    /// <summary>
    /// Implement this interface to create a new strategy for DateOnly.Humanize and hook it in the Configurator.DateOnlyHumanizeStrategy
    /// </summary>
    public interface IDateOnlyHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates used for DateOnly.Humanize
        /// </summary>
        string Humanize(DateOnly input, DateOnly comparisonBase, CultureInfo culture);
    }
}

#endif
