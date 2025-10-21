namespace Humanizer;

/// <summary>
/// Humanizes DateTime into human readable sentence
/// </summary>
public static class DateToOrdinalWordsExtensions
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> to its ordinal words representation (e.g., "1st of January, 2023").
    /// </summary>
    /// <param name="input">The date to be converted to ordinal words.</param>
    /// <returns>
    /// A string containing the date expressed in ordinal words format, culture-specific.
    /// For English: "1st of January, 2023", "22nd of December, 2020", etc.
    /// </returns>
    /// <remarks>
    /// The format and style of ordinal words depends on the current culture.
    /// Uses the configured date-to-ordinal-words converter for conversion.
    /// </remarks>
    /// <example>
    /// <code>
    /// new DateTime(2023, 1, 1).ToOrdinalWords() => "1st of January, 2023" (in en-US culture)
    /// new DateTime(2020, 12, 22).ToOrdinalWords() => "22nd of December, 2020" (in en-US culture)
    /// </code>
    /// </example>
    public static string ToOrdinalWords(this DateTime input) =>
        Configurator.DateToOrdinalWordsConverter.Convert(input);

    /// <summary>
    /// Converts a <see cref="DateTime"/> to its ordinal words representation using the specified grammatical case.
    /// </summary>
    /// <param name="input">The date to be converted to ordinal words.</param>
    /// <param name="grammaticalCase">
    /// The grammatical case to use for the output words (e.g., Nominative, Genitive, etc.).
    /// This is particularly important for languages with case systems like Russian, Polish, etc.
    /// </param>
    /// <returns>
    /// A string containing the date expressed in ordinal words format in the specified grammatical case.
    /// </returns>
    /// <remarks>
    /// The grammatical case parameter is primarily used by languages that have case systems.
    /// For languages without grammatical cases (like English), this parameter has no effect.
    /// </remarks>
    /// <example>
    /// <code>
    /// // In Russian culture:
    /// date.ToOrdinalWords(GrammaticalCase.Nominative) => different form than
    /// date.ToOrdinalWords(GrammaticalCase.Genitive)
    /// </code>
    /// </example>
    public static string ToOrdinalWords(this DateTime input, GrammaticalCase grammaticalCase) =>
        Configurator.DateToOrdinalWordsConverter.Convert(input, grammaticalCase);

#if NET6_0_OR_GREATER
    /// <summary>
    /// Converts a <see cref="DateOnly"/> to its ordinal words representation (e.g., "1st of January, 2023").
    /// </summary>
    /// <param name="input">The date to be converted to ordinal words.</param>
    /// <returns>
    /// A string containing the date expressed in ordinal words format, culture-specific.
    /// For English: "1st of January, 2023", "22nd of December, 2020", etc.
    /// </returns>
    /// <remarks>
    /// The format and style of ordinal words depends on the current culture.
    /// Uses the configured date-only-to-ordinal-words converter for conversion.
    /// This method is available only on .NET 6.0 and later.
    /// </remarks>
    /// <example>
    /// <code>
    /// new DateOnly(2023, 1, 1).ToOrdinalWords() => "1st of January, 2023" (in en-US culture)
    /// new DateOnly(2020, 12, 22).ToOrdinalWords() => "22nd of December, 2020" (in en-US culture)
    /// </code>
    /// </example>
    public static string ToOrdinalWords(this DateOnly input) =>
        Configurator.DateOnlyToOrdinalWordsConverter.Convert(input);

    /// <summary>
    /// Converts a <see cref="DateOnly"/> to its ordinal words representation using the specified grammatical case.
    /// </summary>
    /// <param name="input">The date to be converted to ordinal words.</param>
    /// <param name="grammaticalCase">
    /// The grammatical case to use for the output words (e.g., Nominative, Genitive, etc.).
    /// This is particularly important for languages with case systems like Russian, Polish, etc.
    /// </param>
    /// <returns>
    /// A string containing the date expressed in ordinal words format in the specified grammatical case.
    /// </returns>
    /// <remarks>
    /// The grammatical case parameter is primarily used by languages that have case systems.
    /// For languages without grammatical cases (like English), this parameter has no effect.
    /// This method is available only on .NET 6.0 and later.
    /// </remarks>
    /// <example>
    /// <code>
    /// // In Russian culture:
    /// date.ToOrdinalWords(GrammaticalCase.Nominative) => different form than
    /// date.ToOrdinalWords(GrammaticalCase.Genitive)
    /// </code>
    /// </example>
    public static string ToOrdinalWords(this DateOnly input, GrammaticalCase grammaticalCase) =>
        Configurator.DateOnlyToOrdinalWordsConverter.Convert(input, grammaticalCase);
#endif
}