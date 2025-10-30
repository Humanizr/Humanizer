namespace Humanizer;

/// <summary>
/// Enumerates the ways of displaying a quantity value when converting
/// a word to a quantity string.
/// </summary>
public enum ShowQuantityAs
{
    /// <summary>
    /// Indicates that no quantity will be included in the formatted string.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that the quantity will be included in the output, formatted
    /// as its numeric value (e.g. "1").
    /// </summary>
    Numeric,

    /// <summary>
    /// Incidates that the quantity will be included in the output, formatted as
    /// words (e.g. 123 => "one hundred and twenty three").
    /// </summary>
    Words
}

/// <summary>
/// Provides extensions for formatting a <see cref="string"/> word as a quantity.
/// </summary>
public static class ToQuantityExtensions
{
    /// <summary>
    /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
    /// </summary>
    /// <param name="input">The word to be prefixed</param>
    /// <param name="quantity">The quantity of the word</param>
    /// <param name="showQuantityAs">How to show the quantity. Numeric by default</param>
    /// <example>
    /// "request".ToQuantity(0) => "0 requests"
    /// "request".ToQuantity(1) => "1 request"
    /// "request".ToQuantity(2) => "2 requests"
    /// "men".ToQuantity(2) => "2 men"
    /// "process".ToQuantity(1200, ShowQuantityAs.Words) => "one thousand two hundred processes"
    /// </example>
    public static string ToQuantity(this string input, long quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric) =>
        input.ToQuantity(quantity, showQuantityAs, format: null, formatProvider: null);

    /// <summary>
    /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
    /// </summary>
    /// <param name="input">The word to be prefixed</param>
    /// <param name="quantity">The quantity of the word</param>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <example>
    /// "request".ToQuantity(0) => "0 requests"
    /// "request".ToQuantity(10000, format: "N0") => "10,000 requests"
    /// "request".ToQuantity(1, format: "N0") => "1 request"
    /// </example>
    public static string ToQuantity(this string input, long quantity, string? format, IFormatProvider? formatProvider = null) =>
        input.ToQuantity(quantity, showQuantityAs: ShowQuantityAs.Numeric, format: format, formatProvider: formatProvider);

    static string ToQuantity(this string input, long quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric, string? format = null, IFormatProvider? formatProvider = null)
    {
        var transformedInput = quantity is 1 or -1
            ? input.Singularize(inputIsKnownToBePlural: false)
            : input.Pluralize(inputIsKnownToBeSingular: false);

        if (showQuantityAs == ShowQuantityAs.None)
        {
            return transformedInput;
        }

        if (showQuantityAs == ShowQuantityAs.Numeric)
        {
            var quantityStr = quantity.ToString(format, formatProvider);
            return formatProvider != null
                ? string.Format(formatProvider, "{0} {1}", quantityStr, transformedInput)
                : ConcatWithSpace(quantityStr, transformedInput);
        }

        return ConcatWithSpace(quantity.ToWords(), transformedInput);
    }

    /// <summary>
    /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
    /// </summary>
    /// <param name="input">The word to be prefixed</param>
    /// <param name="quantity">The quantity of the word</param>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
    /// <example>
    /// "request".ToQuantity(0.2) => "0.2 requests"
    /// "request".ToQuantity(10.6, format: "N0") => "10.6 requests"
    /// "request".ToQuantity(1.0, format: "N0") => "1 request"
    /// </example>
    public static string ToQuantity(this string input, double quantity, string? format = null, IFormatProvider? formatProvider = null)
    {
        var isFinite = !(double.IsNaN(quantity) || double.IsInfinity(quantity));
        var isSingular = isFinite && quantity == Math.Truncate(quantity) && Math.Abs(quantity) == 1d;

        var transformedInput = isSingular
            ? input.Singularize(inputIsKnownToBePlural: false)
            : input.Pluralize(inputIsKnownToBeSingular: false);

        var quantityStr = quantity.ToString(format, formatProvider);
        return formatProvider != null
            ? string.Format(formatProvider, "{0} {1}", quantityStr, transformedInput)
            : ConcatWithSpace(quantityStr, transformedInput);
    }

    /// <summary>
    /// Prefixes the provided word with the number and accordingly pluralizes or singularizes the word
    /// </summary>
    /// <param name="input">The word to be prefixed</param>
    /// <param name="quantity">The quantity of the word</param>
    /// <example>
    /// "request".ToQuantity(0.2) => "0.2 requests"
    /// </example>
    public static string ToQuantity(this string input, double quantity) =>
        ToQuantity(input, quantity, null, null);

    static string ConcatWithSpace(string left, string right)
    {
#if NET6_0_OR_GREATER
        return string.Create(left.Length + 1 + right.Length, (left, right), (span, state) =>
        {
            state.left.CopyTo(span);
            span[state.left.Length] = ' ';
            state.right.CopyTo(span[(state.left.Length + 1)..]);
        });
#else
        return string.Concat(left, " ", right);
#endif
    }
}