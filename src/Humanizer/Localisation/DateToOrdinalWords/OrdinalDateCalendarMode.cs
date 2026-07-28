namespace Humanizer;

/// <summary>
/// Controls how the calendar is resolved when formatting ordinal dates.
/// </summary>
enum OrdinalDateCalendarMode
{
    /// <summary>Forces the Gregorian calendar regardless of the culture's default calendar.</summary>
    Gregorian,
    /// <summary>Uses the culture's default calendar (e.g., Thai Buddhist, Hebrew, Persian).</summary>
    Native
}