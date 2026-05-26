namespace Humanizer;

/// <summary>
/// Options for parsing human-friendly duration strings into <see cref="TimeSpan"/> values.
/// </summary>
public sealed class TimeSpanDehumanizeOptions
{
    /// <summary>
    /// Gets the default options instance.
    /// </summary>
    public static TimeSpanDehumanizeOptions Default { get; } = new();

    /// <summary>
    /// Gets how colon-separated values such as <c>3:18</c> are interpreted.
    /// </summary>
    public TimeSpanDehumanizeColonFormat ColonFormat { get; init; } = TimeSpanDehumanizeColonFormat.HoursMinutes;
}
