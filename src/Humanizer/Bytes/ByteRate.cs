namespace Humanizer;

/// <summary>
/// Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer
/// </summary>
/// <remarks>
/// Create a ByteRate with given quantity of bytes across an interval
/// </remarks>
public class ByteRate(ByteSize size, TimeSpan interval) :
    IComparable<ByteRate>,
    IEquatable<ByteRate>,
    IComparable
{
    /// <summary>
    /// Quantity of bytes
    /// </summary>
    public ByteSize Size { get; } = size;

    /// <summary>
    /// Interval that bytes were transferred in
    /// </summary>
    public TimeSpan Interval { get; } = interval;

    /// <summary>
    /// Calculate rate for the quantity of bytes and interval defined by this instance
    /// </summary>
    /// <param name="timeUnit">Unit of time to calculate rate for (defaults is per second)</param>
    public string Humanize(TimeUnit timeUnit = TimeUnit.Second) =>
        Humanize(null, timeUnit);

    /// <summary>
    /// Calculate rate for the quantity of bytes and interval defined by this instance
    /// </summary>
    /// <param name="timeUnit">Unit of time to calculate rate for (defaults is per second)</param>
    /// <param name="format">The string format to use for the number of bytes</param>
    /// <param name="culture">Culture to use. If null, current thread's UI culture is used.</param>
    public string Humanize(string? format, TimeUnit timeUnit = TimeUnit.Second, CultureInfo? culture = null)
    {
        var displayInterval = timeUnit switch
        {
            TimeUnit.Second => TimeSpan.FromSeconds(1),
            TimeUnit.Minute => TimeSpan.FromMinutes(1),
            TimeUnit.Hour => TimeSpan.FromHours(1),
            _ => throw new NotSupportedException("timeUnit must be Second, Minute, or Hour"),
        };
        return new ByteSize(Size.Bytes / Interval.TotalSeconds * displayInterval.TotalSeconds)
            .Humanize(format, culture) + '/' + timeUnit.ToSymbol(culture);
    }

    /// <summary>
    /// Returns the humanized rate using the default format and time unit.
    /// </summary>
    public override string ToString() =>
        Humanize();

    /// <summary>
    /// Compares this rate with another rate after normalizing both to bytes per second.
    /// </summary>
    /// <param name="other">The rate to compare with.</param>
    public int CompareTo(ByteRate? other) =>
        other is null ? 1 : BytesPerSecond.CompareTo(other.BytesPerSecond);

    /// <inheritdoc />
    public bool Equals(ByteRate? other) =>
        other is not null && BytesPerSecond.Equals(other.BytesPerSecond);

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is ByteRate other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        BytesPerSecond.GetHashCode();

    /// <inheritdoc />
    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ByteRate other => CompareTo(other),
            _ => throw new ArgumentException("Object is not a ByteRate", nameof(obj)),
        };

    double BytesPerSecond => Size.Bytes / Interval.TotalSeconds;
}