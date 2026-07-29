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
    /// Turns a byte quantity into human-readable form using an explicit unit system.
    /// </summary>
    /// <param name="input">The byte quantity to humanize.</param>
    /// <param name="unitSystem">The unit system to use.</param>
    /// <param name="format">
    /// The numeric format and optional unit token. For decimal SI and binary IEC, unit tokens are
    /// matched case-insensitively and output uses canonical symbol casing.
    /// </param>
    /// <param name="formatProvider">The provider used to format the numeric value.</param>
    /// <returns>The humanized byte quantity.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid, or selects a token not supported by the selected non-legacy system.
    /// </exception>
    public static string HumanizeWithUnitSystem(
        this ByteSize input,
        ByteSizeUnitSystem unitSystem,
        string? format = null,
        IFormatProvider? formatProvider = null) =>
        input.Format(unitSystem, format, formatProvider);

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
    /// Turns a byte quantity into composite human-readable form using one explicit unit system.
    /// </summary>
    /// <param name="input">The byte quantity to humanize.</param>
    /// <param name="unitSystem">The unit system to use.</param>
    /// <param name="precision">The maximum number of non-zero parts to return.</param>
    /// <param name="formatProvider">The provider used to format each numeric part and select localized unit words.</param>
    /// <param name="separator">The separator to place between parts.</param>
    /// <param name="toWords">Uses localized unit words instead of canonical symbols when <see langword="true"/>.</param>
    /// <returns>The composite humanized byte quantity.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="unitSystem"/> is not defined or <paramref name="precision"/> is less than one.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="separator"/> is <see langword="null"/>.</exception>
    public static string HumanizeCompositeWithUnitSystem(
        this ByteSize input,
        ByteSizeUnitSystem unitSystem,
        int precision = 2,
        IFormatProvider? formatProvider = null,
        string separator = " ",
        bool toWords = false)
    {
        if (unitSystem == ByteSizeUnitSystem.Legacy)
        {
            return input.HumanizeComposite(precision, formatProvider, separator, toWords);
        }

        if (unitSystem is < ByteSizeUnitSystem.DecimalSi or > ByteSizeUnitSystem.BinaryIec)
        {
            throw new ArgumentOutOfRangeException(nameof(unitSystem), unitSystem, "Unknown byte-size unit system.");
        }

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
        var units = unitSystem == ByteSizeUnitSystem.DecimalSi ? DecimalCompositeUnits : BinaryCompositeUnits;

        if (remainingBits == 0)
        {
            return string.Concat("0 ", toWords
                ? formatter.DataUnitHumanize(DataUnit.Bit, 0, toSymbol: false)
                : ByteSize.BitSymbol);
        }

        var parts = new List<string>(Math.Min(precision, units.Length + 2));
        foreach (var unit in units)
        {
            if (parts.Count >= precision)
            {
                break;
            }

            var bitsPerUnit = (ulong)unit.Bytes * ByteSize.BitsInByte;
            var count = remainingBits / bitsPerUnit;
            if (count == 0)
            {
                continue;
            }

            var unitText = toWords
                ? formatter.DataUnitHumanize(unit.DataUnit, count, toSymbol: false)
                : unit.Symbol;
            parts.Add(string.Concat(count.ToString(formatProvider), " ", unitText));
            remainingBits %= bitsPerUnit;
        }

        addSmallUnit(ByteSize.BitsInByte, ByteSize.ByteSymbol, DataUnit.Byte);
        addSmallUnit(1, ByteSize.BitSymbol, DataUnit.Bit);

        return string.Concat(
            isNegative ? NumberFormatInfo.GetInstance(formatProvider).NegativeSign : null,
            string.Join(separator, parts));

        void addSmallUnit(ulong bitsPerUnit, string symbol, DataUnit dataUnit)
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
                toWords ? formatter.DataUnitHumanize(dataUnit, count, toSymbol: false) : symbol));
            remainingBits %= bitsPerUnit;
        }
    }

    static readonly CompositeUnit[] DecimalCompositeUnits =
    [
        new(ByteSize.BytesInDecimalExabyte, "EB", DataUnit.DecimalExabyte),
        new(ByteSize.BytesInDecimalPetabyte, "PB", DataUnit.DecimalPetabyte),
        new(ByteSize.BytesInDecimalTerabyte, "TB", DataUnit.DecimalTerabyte),
        new(ByteSize.BytesInDecimalGigabyte, "GB", DataUnit.DecimalGigabyte),
        new(ByteSize.BytesInDecimalMegabyte, "MB", DataUnit.DecimalMegabyte),
        new(ByteSize.BytesInDecimalKilobyte, "kB", DataUnit.DecimalKilobyte)
    ];

    static readonly CompositeUnit[] BinaryCompositeUnits =
    [
        new(ByteSize.BytesInPebibyte, "PiB", DataUnit.BinaryPebibyte),
        new(ByteSize.BytesInTebibyte, "TiB", DataUnit.BinaryTebibyte),
        new(ByteSize.BytesInGibibyte, "GiB", DataUnit.BinaryGibibyte),
        new(ByteSize.BytesInMebibyte, "MiB", DataUnit.BinaryMebibyte),
        new(ByteSize.BytesInKibibyte, "KiB", DataUnit.BinaryKibibyte)
    ];

    readonly record struct CompositeUnit(long Bytes, string Symbol, DataUnit DataUnit);

    /// <summary>
    /// Turns a quantity of bytes in a given interval into a rate that can be manipulated
    /// </summary>
    /// <param name="size">Quantity of bytes</param>
    /// <param name="interval">Interval to create rate for</param>
    public static ByteRate Per(this ByteSize size, TimeSpan interval) =>
        new(size, interval);
}