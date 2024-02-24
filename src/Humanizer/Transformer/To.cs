namespace Humanizer;

/// <summary>
/// A portal to string transformation using IStringTransformer
/// </summary>
public static class To
{
    /// <summary>
    /// Transforms a string using the provided transformers. Transformations are applied in the provided order.
    /// </summary>
    public static string Transform(this string input, params IStringTransformer[] transformers) =>
        transformers.Aggregate(input, (current, stringTransformer) => stringTransformer.Transform(current));

    /// <summary>
    /// Transforms a string using the provided transformers. Transformations are applied in the provided order.
    /// </summary>
    public static string Transform(this string input, CultureInfo culture, params ICulturedStringTransformer[] transformers) =>
        transformers.Aggregate(input, (current, stringTransformer) => stringTransformer.Transform(current, culture));

    /// <summary>
    /// Changes string to title case
    /// </summary>
    /// <example>
    /// "INvalid caSEs arE corrected" -> "Invalid Cases Are Corrected"
    /// </example>
    public static ICulturedStringTransformer TitleCase { get; } = new ToTitleCase();

    /// <summary>
    /// Changes the string to lower case
    /// </summary>
    /// <example>
    /// "Sentence casing" -> "sentence casing"
    /// </example>
    public static ICulturedStringTransformer LowerCase { get; } = new ToLowerCase();

    /// <summary>
    /// Changes the string to upper case
    /// </summary>
    /// <example>
    /// "lower case statement" -> "LOWER CASE STATEMENT"
    /// </example>
    public static ICulturedStringTransformer UpperCase { get; } = new ToUpperCase();

    /// <summary>
    /// Changes the string to sentence case
    /// </summary>
    /// <example>
    /// "lower case statement" -> "Lower case statement"
    /// </example>
    public static ICulturedStringTransformer SentenceCase { get; } = new ToSentenceCase();
}