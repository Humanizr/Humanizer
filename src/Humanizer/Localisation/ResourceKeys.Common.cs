namespace Humanizer;

/// <summary>
/// Shared helpers for building Humanizer resource keys.
/// </summary>
public partial class ResourceKeys
{
    /// <summary>
    /// Validates that a resource count is not negative.
    /// </summary>
    /// <param name="count">The number of units represented by the resource key.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative.</exception>
    static void ValidateRange(int count)
    {
        // Zero is handled by the callers' special cases; only negative counts are invalid here.
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
    }
}
