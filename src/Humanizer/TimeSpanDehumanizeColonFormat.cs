namespace Humanizer;

/// <summary>
/// Controls how colon-separated duration strings are interpreted.
/// </summary>
public enum TimeSpanDehumanizeColonFormat
{
    /// <summary>
    /// Two-part values are hours and minutes; three-part values are hours, minutes, and seconds.
    /// </summary>
    HoursMinutes = 0,

    /// <summary>
    /// Two-part values are minutes and seconds; three-part values remain hours, minutes, and seconds.
    /// </summary>
    MinutesSeconds = 1,
}
