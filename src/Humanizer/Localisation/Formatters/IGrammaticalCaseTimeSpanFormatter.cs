namespace Humanizer;

/// <summary>
/// Optionally extends a locale formatter with grammatical-case-aware duration phrases.
/// </summary>
public interface IGrammaticalCaseTimeSpanFormatter : IFormatter
{
    /// <summary>
    /// Returns the locale-authored unit-case phrase for a duration count.
    /// </summary>
    /// <param name="timeUnit">The unit being described.</param>
    /// <param name="unit">The number of units being described.</param>
    /// <param name="grammaticalCase">The grammatical case used to select the unit phrase.</param>
    /// <returns>A locale-authored phrase whose count may be explicit or encoded by the unit form.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeUnit"/> or <paramref name="grammaticalCase"/> is outside its defined enum range.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The locale or duration unit does not have verified support for <paramref name="grammaticalCase"/>.
    /// </exception>
    string TimeSpanHumanize(TimeUnit timeUnit, int unit, GrammaticalCase grammaticalCase);
}