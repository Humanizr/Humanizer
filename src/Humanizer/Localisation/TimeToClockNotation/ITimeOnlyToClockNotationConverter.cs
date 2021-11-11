#if NET6_0_OR_GREATER

using System;

using Humanizer;

namespace Humanizer.Localisation.TimeToClockNotation
{
    /// <summary>
    /// The interface used to localise the ToClockNotation method.
    /// </summary>
    public interface ITimeOnlyToClockNotationConverter
    {
        /// <summary>
        /// Converts the time to Clock Notation 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="roundToNearestFive"></param>
        /// <returns></returns>
        string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive);
    }
}

#endif
