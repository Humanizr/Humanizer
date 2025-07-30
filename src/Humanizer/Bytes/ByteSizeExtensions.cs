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
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this byte input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this sbyte input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this short input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this ushort input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this int input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this uint input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this double input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabyte
    /// </summary>
    public static ByteSize Petabytes(this long input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this byte input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this sbyte input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this short input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this ushort input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this int input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this uint input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this double input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabyte
    /// </summary>
    public static ByteSize Exabytes(this long input) =>
        ByteSize.FromExabytes(input);


    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this byte input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this sbyte input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this short input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this ushort input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this int input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this uint input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this double input) =>
        ByteSize.FromZettabytes(input);

    /// <summary>
    /// Considers input as zettabyte
    /// </summary>
    public static ByteSize Zettabytes(this long input) =>
        ByteSize.FromZettabytes(input);


    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this byte input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this sbyte input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this short input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this ushort input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this int input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this uint input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this double input) =>
        ByteSize.FromYottabytes(input);

    /// <summary>
    /// Considers input as yottabyte
    /// </summary>
    public static ByteSize Yottabytes(this long input) =>
        ByteSize.FromYottabytes(input);


    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this byte input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this sbyte input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this short input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this ushort input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this int input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this uint input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this double input) =>
        ByteSize.FromRonnabytes(input);

    /// <summary>
    /// Considers input as ronnabyte
    /// </summary>
    public static ByteSize Ronnabytes(this long input) =>
        ByteSize.FromRonnabytes(input);


    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this byte input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this sbyte input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this short input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this ushort input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this int input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this uint input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this double input) =>
        ByteSize.FromQuettabytes(input);

    /// <summary>
    /// Considers input as quettabyte
    /// </summary>
    public static ByteSize Quettabytes(this long input) =>
        ByteSize.FromQuettabytes(input);


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