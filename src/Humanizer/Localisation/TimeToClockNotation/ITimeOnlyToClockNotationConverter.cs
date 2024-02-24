#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// The interface used to localise the ToClockNotation method.
/// </summary>
public interface ITimeOnlyToClockNotationConverter
{
    /// <summary>
    /// Converts the time to Clock Notation
    /// </summary>
    string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive);
}

#endif
