namespace Humanizer;

/// <summary>
/// Extends a formatter with support for fractional-second duration values.
/// </summary>
public interface IFractionalTimeSpanFormatter
{
    /// <summary>
    /// Returns the localized representation of a seconds value.
    /// </summary>
    /// <param name="seconds">The non-negative seconds value to format.</param>
    /// <param name="toSymbols">Whether the seconds unit is rendered as a symbol.</param>
    /// <returns>The localized seconds value.</returns>
    string TimeSpanHumanizeWithFractionalSeconds(decimal seconds, bool toSymbols);
}