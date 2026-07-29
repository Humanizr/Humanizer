namespace Humanizer;

/// <summary>
/// Identifies a Unicode CLDR cardinal plural category.
/// </summary>
/// <remarks>
/// The names are grammatical categories, not numeric ranges. Their meaning is defined by the
/// selected culture's CLDR cardinal plural rules.
/// </remarks>
enum CardinalPluralCategory
{
    /// <summary>
    /// The required catch-all category.
    /// </summary>
    Other = 0,

    /// <summary>
    /// The culture's <c>zero</c> category.
    /// </summary>
    Zero = 1,

    /// <summary>
    /// The culture's <c>one</c> category.
    /// </summary>
    One = 2,

    /// <summary>
    /// The culture's <c>two</c> category.
    /// </summary>
    Two = 3,

    /// <summary>
    /// The culture's <c>few</c> category.
    /// </summary>
    Few = 4,

    /// <summary>
    /// The culture's <c>many</c> category.
    /// </summary>
    Many = 5
}