#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Text;

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
        /// <returns></returns>
        string Convert(TimeOnly time);
    }
}

#endif
