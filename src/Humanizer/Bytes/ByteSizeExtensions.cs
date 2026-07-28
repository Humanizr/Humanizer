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
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this byte input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this sbyte input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this short input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this ushort input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this int input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this uint input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this double input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as petabytes
    /// </summary>
    public static ByteSize Petabytes(this long input) =>
        ByteSize.FromPetabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this byte input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this sbyte input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this short input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this ushort input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this int input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this uint input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this double input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as exabytes
    /// </summary>
    public static ByteSize Exabytes(this long input) =>
        ByteSize.FromExabytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this byte input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this sbyte input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this short input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this ushort input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this int input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this uint input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this double input) =>
        ByteSize.FromPebibytes(input);

    /// <summary>
    /// Considers input as pebibytes
    /// </summary>
    public static ByteSize Pebibytes(this long input) =>
        ByteSize.FromPebibytes(input);

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
    /// Turns a byte quantity into a composite human-readable form using descending units, e.g. 10 KB 2 B.
    /// </summary>
    /// <param name="input">The byte quantity to humanize.</param>
    /// <param name="precision">The maximum number of non-zero parts to return.</param>
    /// <param name="formatProvider">The format provider to use. If null, the current culture is used.</param>
    /// <param name="separator">The separator to use between parts.</param>
    /// <param name="toWords">Uses unit words instead of symbols if true.</param>
    /// <returns>The composite byte quantity.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="precision"/> is less than one.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="separator"/> is null.</exception>
    public static string HumanizeComposite(
        this ByteSize input,
        int precision = 2,
        IFormatProvider? formatProvider = null,
        string separator = " ",
        bool toWords = false)
    {
        if (precision < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(precision), precision, "Precision must be greater than zero.");
        }

        ArgumentNullException.ThrowIfNull(separator);

        formatProvider ??= CultureInfo.CurrentCulture;
        var culture = formatProvider as CultureInfo;
        if (culture is not null)
        {
            formatProvider = LocaleNumberFormattingOverrides.GetFormattingNumberFormat(culture);
        }

        var formatter = Configurator.GetFormatter(culture);
        var isNegative = input.Bits < 0;
        var remainingBits = isNegative
            ? (ulong)(-(input.Bits + 1)) + 1
            : (ulong)input.Bits;

        if (remainingBits == 0)
        {
            return string.Concat("0 ", formatter.DataUnitHumanize(DataUnit.Bit, 0, toSymbol: !toWords));
        }

        var parts = new List<string>(Math.Min(precision, 8));

        void addPart(ulong bitsPerUnit, DataUnit unit)
        {
            if (parts.Count >= precision)
            {
                return;
            }

            var count = remainingBits / bitsPerUnit;
            if (count == 0)
            {
                return;
            }

            parts.Add(string.Concat(
                count.ToString(formatProvider),
                " ",
                formatter.DataUnitHumanize(unit, count, toSymbol: !toWords)));
            remainingBits %= bitsPerUnit;
        }

        addPart((ulong)ByteSize.BytesInExabyte * ByteSize.BitsInByte, DataUnit.Exabyte);
        addPart((ulong)ByteSize.BytesInPetabyte * ByteSize.BitsInByte, DataUnit.Petabyte);
        addPart((ulong)ByteSize.BytesInTerabyte * ByteSize.BitsInByte, DataUnit.Terabyte);
        addPart((ulong)ByteSize.BytesInGigabyte * ByteSize.BitsInByte, DataUnit.Gigabyte);
        addPart((ulong)ByteSize.BytesInMegabyte * ByteSize.BitsInByte, DataUnit.Megabyte);
        addPart((ulong)ByteSize.BytesInKilobyte * ByteSize.BitsInByte, DataUnit.Kilobyte);
        addPart(ByteSize.BitsInByte, DataUnit.Byte);
        addPart(1, DataUnit.Bit);

        return string.Concat(
            isNegative ? NumberFormatInfo.GetInstance(formatProvider).NegativeSign : null,
            string.Join(separator, parts));
    }

    /// <summary>
    /// Turns a quantity of bytes in a given interval into a rate that can be manipulated
    /// </summary>
    /// <param name="size">Quantity of bytes</param>
    /// <param name="interval">Interval to create rate for</param>
    public static ByteRate Per(this ByteSize size, TimeSpan interval) =>
        new(size, interval);
}