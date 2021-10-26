#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Text;

namespace Humanizer.Localisation.TimeToClockNotation
{
    /// <summary>
    /// The interface used to localise the ToClockWords method.
    /// </summary>
    public interface ITimeOnlyToClockNotationConverter
    {
        /// <summary>
        /// Converts the time to Clock Words 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        string Convert(TimeOnly time);
    }
}

#endif
