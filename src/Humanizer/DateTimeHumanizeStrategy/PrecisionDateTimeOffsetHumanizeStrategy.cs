namespace Humanizer;

/// <summary>
/// Precision-based calculator for distance between two times
/// </summary>
/// <remarks>
/// Constructs a precision-based calculator for distance of time with default precision 0.75.
/// </remarks>
/// <param name="precision">precision of approximation, if not provided  0.75 will be used as a default precision.</param>
public class PrecisionDateTimeOffsetHumanizeStrategy(double precision = .75) : IDateTimeOffsetHumanizeStrategy
{
    readonly double precision = precision;

    /// <summary>
    /// Returns localized &amp; humanized distance of time between two dates; given a specific precision.
    /// </summary>
    public string Humanize(DateTimeOffset input, DateTimeOffset comparisonBase, CultureInfo? culture) =>
        DateTimeHumanizeAlgorithms.PrecisionHumanize(input.UtcDateTime, comparisonBase.UtcDateTime, precision, culture);
}