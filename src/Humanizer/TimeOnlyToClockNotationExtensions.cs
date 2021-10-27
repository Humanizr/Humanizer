#if NET6_0_OR_GREATER

using System;
using Humanizer.Configuration;

namespace Humanizer
{
    /// <summary>
    /// Humanizes TimeOnly into human readable sentence
    /// </summary>
    public static class TimeOnlyToClockNotationExtensions
    {
        /// <summary>
        /// Turns the provided time into clock notation
        /// </summary>
        /// <param name="input">The time to be made into clock notation</param>
        /// <returns>The time in clock notation</returns>
        public static string ToClockNotation(this TimeOnly input)
        {
            return Configurator.TimeOnlyToClockNotationConverter.Convert(input);
        }
    }
}

#endif
