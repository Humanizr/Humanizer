using System.Numerics;

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
    /// <param name="culture">Culture to use. If null, current thread's culture is used.</param>
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
    /// Calculates and humanizes this rate using an explicitly selected byte-size unit system.
    /// </summary>
    /// <param name="unitSystem">The byte-size unit system to use.</param>
    /// <param name="format">
    /// The numeric format and optional byte-size unit token. For <see cref="ByteSizeUnitSystem.DecimalSi"/> and
    /// <see cref="ByteSizeUnitSystem.BinaryIec"/>, SI/IEC-prefixed unit tokens are matched case-insensitively,
    /// while <c>b</c> and <c>B</c> remain case-sensitive; output uses canonical symbol casing.
    /// <see cref="ByteSizeUnitSystem.Legacy"/> preserves established matching behavior.
    /// </param>
    /// <param name="timeUnit">The time unit to use for the displayed rate.</param>
    /// <param name="culture">The culture used to format the numeric value, byte-size unit, and time-unit symbol.</param>
    /// <returns>The humanized rate.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="timeUnit"/> is not <see cref="TimeUnit.Second"/>, <see cref="TimeUnit.Minute"/>,
    /// or <see cref="TimeUnit.Hour"/>.
    /// </exception>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid, or selects a token not supported by the selected non-legacy system.
    /// </exception>
    public string HumanizeWithUnitSystem(
        ByteSizeUnitSystem unitSystem,
        string? format = null,
        TimeUnit timeUnit = TimeUnit.Second,
        CultureInfo? culture = null)
    {
        if (unitSystem == ByteSizeUnitSystem.Legacy)
        {
            return Humanize(format, timeUnit, culture);
        }

        var displayInterval = timeUnit switch
        {
            TimeUnit.Second => TimeSpan.FromSeconds(1),
            TimeUnit.Minute => TimeSpan.FromMinutes(1),
            TimeUnit.Hour => TimeSpan.FromHours(1),
            _ => throw new NotSupportedException("timeUnit must be Second, Minute, or Hour"),
        };
        return ScaleForExplicitUnitSystem(displayInterval)
            .HumanizeWithUnitSystem(unitSystem, format, culture) + '/' + timeUnit.ToSymbol(culture);
    }

    ByteSize ScaleForExplicitUnitSystem(TimeSpan displayInterval)
    {
        if (displayInterval == Interval)
        {
            return Size;
        }

        var bytes = Size.Bytes / Interval.TotalSeconds * displayInterval.TotalSeconds;
        if (Interval.Ticks == 0 || !Size.Bytes.Equals(Size.Bits / (double)ByteSize.BitsInByte))
        {
            return new(bytes);
        }

        var numerator = (BigInteger)Size.Bits * displayInterval.Ticks;
        var quotient = BigInteger.DivRem(
            numerator,
            Interval.Ticks,
            out var remainder);
        if (!remainder.IsZero && numerator.Sign == Math.Sign(Interval.Ticks))
        {
            quotient++;
        }

        return quotient >= long.MinValue && quotient <= long.MaxValue
            ? ByteSize.FromBytesAndBits(bytes, (long)quotient)
            : new(bytes);
    }

    /// <summary>
    /// Returns the humanized rate using the default format and time unit.
    /// </summary>
    public override string ToString() =>
        Humanize();

    /// <summary>
    /// Compares this rate with another rate after normalizing both to bytes per second.
    /// Rates with equal normalized values and different runtime types have distinct sort positions.
    /// </summary>
    /// <param name="other">The rate to compare with.</param>
    public int CompareTo(ByteRate? other)
    {
        if (other is null)
            return 1;

        var comparison = BytesPerSecond.CompareTo(other.BytesPerSecond);
        return comparison != 0 || other.GetType() == GetType()
            ? comparison
            : string.CompareOrdinal(GetType().AssemblyQualifiedName, other.GetType().AssemblyQualifiedName);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Equality requires the other instance to have the same runtime type.
    /// </remarks>
    public bool Equals(ByteRate? other) =>
        other?.GetType() == GetType() && BytesPerSecond.Equals(other.BytesPerSecond);

    /// <inheritdoc />
    /// <remarks>
    /// Equality requires the other instance to have the same runtime type.
    /// </remarks>
    public override bool Equals(object? obj) =>
        obj?.GetType() == GetType() && Equals((ByteRate)obj);

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