namespace Humanizer;

/// <summary>
/// Allow strings to be truncated
/// </summary>
public static class TruncateExtensions
{
    /// <summary>
    /// Truncates a string to a specified maximum length using the default truncation string ("…") and
    /// fixed-length truncator.
    /// </summary>
    /// <param name="input">The string to be truncated. Can be null.</param>
    /// <param name="length">The maximum length of the result string, including the truncation indicator.</param>
    /// <returns>
    /// The truncated string if its length exceeds <paramref name="length"/>, otherwise the original string.
    /// Returns null if <paramref name="input"/> is null.
    /// </returns>
    /// <remarks>
    /// The default truncation indicator is "…" (ellipsis), and truncation occurs from the right side of the string.
    /// </remarks>
    /// <example>
    /// <code>
    /// "This is a long string".Truncate(10) => "This is a…"
    /// "Short".Truncate(10) => "Short"
    /// null.Truncate(10) => null
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(input))]
    public static string? Truncate(this string? input, int length) =>
        input.Truncate(length, "…", Truncator.FixedLength);

    /// <summary>
    /// Truncates a string to a specified maximum length using a custom truncator and truncation direction.
    /// </summary>
    /// <param name="input">The string to be truncated. Can be null.</param>
    /// <param name="length">The maximum length of the result string, including the truncation indicator.</param>
    /// <param name="truncator">
    /// The <see cref="ITruncator"/> implementation to use for truncation logic.
    /// Must not be null.
    /// </param>
    /// <param name="from">
    /// Specifies from which side of the string to truncate. Default is <see cref="TruncateFrom.Right"/>.
    /// </param>
    /// <returns>
    /// The truncated string if its length exceeds <paramref name="length"/>, otherwise the original string.
    /// Returns null if <paramref name="input"/> is null.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="truncator"/> is null.</exception>
    /// <example>
    /// <code>
    /// "This is a long string".Truncate(10, Truncator.FixedLength, TruncateFrom.Left) => "…ng string"
    /// "This is a long string".Truncate(10, Truncator.FixedNumberOfWords) => "This is…"
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(input))]
    public static string? Truncate(this string? input, int length, ITruncator truncator, TruncateFrom from = TruncateFrom.Right) =>
        input.Truncate(length, "…", truncator, from);

    /// <summary>
    /// Truncates a string to a specified maximum length using a custom truncation string and fixed-length truncator.
    /// </summary>
    /// <param name="input">The string to be truncated. Can be null.</param>
    /// <param name="length">The maximum length of the result string, including the truncation indicator.</param>
    /// <param name="truncationString">
    /// The string to use as the truncation indicator (e.g., "...", "…", or any custom string).
    /// Can be null or empty.
    /// </param>
    /// <param name="from">
    /// Specifies from which side of the string to truncate. Default is <see cref="TruncateFrom.Right"/>.
    /// </param>
    /// <returns>
    /// The truncated string if its length exceeds <paramref name="length"/>, otherwise the original string.
    /// Returns null if <paramref name="input"/> is null.
    /// </returns>
    /// <example>
    /// <code>
    /// "This is a long string".Truncate(10, "...") => "This is..."
    /// "This is a long string".Truncate(15, "--") => "This is a lo--"
    /// "This is a long string".Truncate(10, "...", TruncateFrom.Left) => "...string"
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(input))]
    public static string? Truncate(this string? input, int length, string? truncationString, TruncateFrom from = TruncateFrom.Right) =>
        input.Truncate(length, truncationString, Truncator.FixedLength, from);

    /// <summary>
    /// Truncates a string to a specified maximum length using a custom truncation string, truncator, and direction.
    /// </summary>
    /// <param name="input">The string to be truncated. Can be null.</param>
    /// <param name="length">The maximum length of the result string, including the truncation indicator.</param>
    /// <param name="truncationString">
    /// The string to use as the truncation indicator (e.g., "...", "…", or any custom string).
    /// Can be null or empty.
    /// </param>
    /// <param name="truncator">
    /// The <see cref="ITruncator"/> implementation to use for truncation logic.
    /// Must not be null.
    /// </param>
    /// <param name="from">
    /// Specifies from which side of the string to truncate. Default is <see cref="TruncateFrom.Right"/>.
    /// </param>
    /// <returns>
    /// The truncated string if its length exceeds <paramref name="length"/>, otherwise the original string.
    /// Returns null if <paramref name="input"/> is null.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="truncator"/> is null.</exception>
    /// <remarks>
    /// This is the most flexible truncation method, allowing full customization of the truncation behavior.
    /// </remarks>
    /// <example>
    /// <code>
    /// "This is a long string".Truncate(10, "...", Truncator.FixedLength, TruncateFrom.Right) => "This is..."
    /// "This is a long string".Truncate(10, "…", Truncator.FixedNumberOfWords, TruncateFrom.Left) => "… string"
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(input))]
    public static string? Truncate(this string? input, int length, string? truncationString, ITruncator truncator, TruncateFrom from = TruncateFrom.Right)
    {
        ArgumentNullException.ThrowIfNull(truncator);

        if (input == null)
        {
            return null;
        }

        return truncator.Truncate(input, length, truncationString, from);
    }
}