using System;
using System.Globalization;
using Humanizer.Localisation;

namespace Humanizer.TimeSpanHumanizeStrategy
{
    /// <summary>
    /// Calculates the distance of time in words between two provided dates used for TimeSpan.Humanize
    /// </summary>
    public interface ITimeSpanHumanizeStrategy
    {
        /// <summary>
        /// Calculates the distance of time in words between two provided dates used for TimeSpan.Humanize
        /// </summary>
        string Humanize(TimeSpanAnalysisParameters timeSpanAnalysisParameters);
    }
}