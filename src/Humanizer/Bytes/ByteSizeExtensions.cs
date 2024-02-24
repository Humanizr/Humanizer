// ReSharper disable once CheckNamespace
namespace Humanizer;

/// <summary>
/// Provides extension methods for ByteSize
/// </summary>
public static class ByteSizeExtensions
{
    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this byte input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this sbyte input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this short input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this ushort input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this int input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this uint input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bits
    /// </summary>
    public static ByteSize Bits(this long input) =>
        ByteSize.FromBits(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this byte input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this sbyte input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this short input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this ushort input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this int input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this uint input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this double input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as bytes
    /// </summary>
    public static ByteSize Bytes(this long input) =>
        ByteSize.FromBytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this byte input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this sbyte input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this short input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this ushort input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this int input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this uint input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this double input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as kilobytes
    /// </summary>
    public static ByteSize Kilobytes(this long input) =>
        ByteSize.FromKilobytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this byte input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this sbyte input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this short input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this ushort input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this int input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this uint input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this double input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as megabytes
    /// </summary>
    public static ByteSize Megabytes(this long input) =>
        ByteSize.FromMegabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this byte input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this sbyte input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this short input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this ushort input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this int input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this uint input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this double input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as gigabytes
    /// </summary>
    public static ByteSize Gigabytes(this long input) =>
        ByteSize.FromGigabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this byte input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this sbyte input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this short input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this ushort input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this int input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this uint input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this double input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Considers input as terabytes
    /// </summary>
    public static ByteSize Terabytes(this long input) =>
        ByteSize.FromTerabytes(input);

    /// <summary>
    /// Turns a byte quantity into human readable form, eg 2 GB
    /// </summary>
    /// <param name="format">The string format to use</param>
    public static string Humanize(this ByteSize input, string? format = null) =>
        string.IsNullOrWhiteSpace(format) ? input.ToString() : input.ToString(format);

    /// <summary>
    /// Turns a byte quantity into human readable form, eg 2 GB
    /// </summary>
    /// <param name="formatProvider">The format provider to use</param>
    public static string Humanize(this ByteSize input, IFormatProvider formatProvider) =>
        input.ToString(formatProvider);

    /// <summary>
    /// Turns a byte quantity into human readable form, eg 2 GB
    /// </summary>
    /// <param name="format">The string format to use</param>
    /// <param name="formatProvider">The format provider to use</param>
    public static string Humanize(this ByteSize input, string? format, IFormatProvider? formatProvider) =>
        string.IsNullOrWhiteSpace(format) ? input.ToString(formatProvider) : input.ToString(format, formatProvider);

    /// <summary>
    /// Turns a quantity of bytes in a given interval into a rate that can be manipulated
    /// </summary>
    /// <param name="size">Quantity of bytes</param>
    /// <param name="interval">Interval to create rate for</param>
    public static ByteRate Per(this ByteSize size, TimeSpan interval) =>
        new(size, interval);
}