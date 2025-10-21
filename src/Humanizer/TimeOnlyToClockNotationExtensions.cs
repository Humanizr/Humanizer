#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Humanizes TimeOnly into human readable sentence
/// </summary>
public static class TimeOnlyToClockNotationExtensions
{
    /// <summary>
    /// Converts a <see cref="TimeOnly"/> value to its clock notation string representation
    /// (e.g., "three o'clock", "half past four", "quarter to six").
    /// </summary>
    /// <param name="input">The time to be converted to clock notation.</param>
    /// <param name="roundToNearestFive">
    /// Specifies whether and how to round the minutes. Default is <see cref="ClockNotationRounding.None"/>.
    /// - <see cref="ClockNotationRounding.None"/>: Use exact minutes
    /// - <see cref="ClockNotationRounding.NearestFiveMinutes"/>: Round to nearest 5 minutes
    /// </param>
    /// <returns>
    /// A culture-specific string representation of the time in clock notation.
    /// For English: "three o'clock", "ten past four", "quarter to six", etc.
    /// </returns>
    /// <remarks>
    /// The output format varies by culture. Some cultures express time differently than others.
    /// This method is only available on .NET 6.0 and later.
    /// </remarks>
    /// <example>
    /// <code>
    /// // English (en-US) examples:
    /// new TimeOnly(15, 0).ToClockNotation() => "three o'clock"
    /// new TimeOnly(15, 15).ToClockNotation() => "quarter past three"
    /// new TimeOnly(15, 30).ToClockNotation() => "half past three"
    /// new TimeOnly(15, 45).ToClockNotation() => "quarter to four"
    /// new TimeOnly(15, 7).ToClockNotation(ClockNotationRounding.NearestFiveMinutes) => "five past three"
    /// </code>
    /// </example>
    public static string ToClockNotation(this TimeOnly input, ClockNotationRounding roundToNearestFive = ClockNotationRounding.None) =>
        Configurator.TimeOnlyToClockNotationConverter.Convert(input, roundToNearestFive);
}

#endif
