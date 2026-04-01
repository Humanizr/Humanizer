namespace Humanizer;

/// <summary>
/// Localizes Humanizer's number, date, duration, and unit formatting.
/// </summary>
public interface IFormatter
{
    /// <summary>
    /// Returns the localized text for the current moment.
    /// </summary>
    string DateHumanize_Now();

    /// <summary>
    /// Returns the localized text used when a date never occurs.
    /// </summary>
    string DateHumanize_Never();

    /// <summary>
    /// Returns the localized representation of a relative date phrase.
    /// </summary>
    /// <param name="timeUnit">The unit being described.</param>
    /// <param name="timeUnitTense">Whether the reference is in the past or the future.</param>
    /// <param name="unit">The number of units being described.</param>
    /// <returns>The localized relative date phrase.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="timeUnit"/> is unsupported or <paramref name="unit"/> is negative.
    /// </exception>
    string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit);

    /// <summary>
    /// Returns the localized representation of a zero-length duration.
    /// </summary>
    /// <returns>The localized zero-duration phrase.</returns>
    string TimeSpanHumanize_Zero();

    /// <summary>
    /// Returns the localized representation of a duration.
    /// </summary>
    /// <param name="timeUnit">The unit being described.</param>
    /// <param name="unit">The number of units being described.</param>
    /// <param name="toWords">Whether the number should be rendered as words.</param>
    /// <returns>The localized duration phrase.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="timeUnit"/> is unsupported or <paramref name="unit"/> is negative.
    /// </exception>
    string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false);

    /// <summary>
    /// Returns the localized age suffix format for a humanized duration.
    /// </summary>
    /// <returns>The localized age suffix format.</returns>
    string TimeSpanHumanize_Age();

    /// <summary>
    /// Returns the localized representation of a data unit, either as a symbol or a full word.
    /// </summary>
    /// <param name="dataUnit">The data unit to format.</param>
    /// <param name="count">The number of units being described.</param>
    /// <param name="toSymbol">Whether the unit should be rendered as a symbol.</param>
    /// <returns>The localized data-unit representation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="dataUnit"/> is unsupported.</exception>
    string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true);

    /// <summary>
    /// Returns the localized symbol for the given <see cref="TimeUnit"/>.
    /// </summary>
    /// <param name="timeUnit">The time unit to format.</param>
    /// <returns>The localized symbol for <paramref name="timeUnit"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="timeUnit"/> is unsupported.</exception>
    string TimeUnitHumanize(TimeUnit timeUnit);
}
