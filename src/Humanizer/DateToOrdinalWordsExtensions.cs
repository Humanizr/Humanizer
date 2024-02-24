﻿namespace Humanizer;

/// <summary>
/// Humanizes DateTime into human readable sentence
/// </summary>
public static class DateToOrdinalWordsExtensions
{
    /// <summary>
    /// Turns the provided date into ordinal words
    /// </summary>
    /// <param name="input">The date to be made into ordinal words</param>
    /// <returns>The date in ordinal words</returns>
    public static string ToOrdinalWords(this DateTime input) =>
        Configurator.DateToOrdinalWordsConverter.Convert(input);

    /// <summary>
    /// Turns the provided date into ordinal words
    /// </summary>
    /// <param name="input">The date to be made into ordinal words</param>
    /// <param name="grammaticalCase">The grammatical case to use for output words</param>
    /// <returns>The date in ordinal words</returns>
    public static string ToOrdinalWords(this DateTime input, GrammaticalCase grammaticalCase) =>
        Configurator.DateToOrdinalWordsConverter.Convert(input, grammaticalCase);

#if NET6_0_OR_GREATER
    /// <summary>
    /// Turns the provided date into ordinal words
    /// </summary>
    /// <param name="input">The date to be made into ordinal words</param>
    /// <returns>The date in ordinal words</returns>
    public static string ToOrdinalWords(this DateOnly input) =>
        Configurator.DateOnlyToOrdinalWordsConverter.Convert(input);

    /// <summary>
    /// Turns the provided date into ordinal words
    /// </summary>
    /// <param name="input">The date to be made into ordinal words</param>
    /// <param name="grammaticalCase">The grammatical case to use for output words</param>
    /// <returns>The date in ordinal words</returns>
    public static string ToOrdinalWords(this DateOnly input, GrammaticalCase grammaticalCase) =>
        Configurator.DateOnlyToOrdinalWordsConverter.Convert(input, grammaticalCase);
#endif
}