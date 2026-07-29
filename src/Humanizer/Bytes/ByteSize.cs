//The MIT License (MIT)

//Copyright (c) 2013-2014 Omar Khudeira (http://omar.io)

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System.Diagnostics;
using System.Runtime.CompilerServices;

using static System.Globalization.NumberStyles;

namespace Humanizer;
#pragma warning disable 1591
public struct ByteSize(double byteSize) :
    IComparable<ByteSize>,
    IEquatable<ByteSize>,
    IComparable,
    IFormattable
{
    ByteSize(double byteSize, long bits)
        : this(byteSize) =>
        Bits = bits;

    static readonly ConditionalWeakTable<NumberFormatInfo, HashSet<char>> NumberFormatSpecialCharsCache = new();

    public static readonly ByteSize MinValue = FromBits(long.MinValue);
    public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

    public const long BitsInByte = 8;
    public const long BytesInKilobyte = 1024;
    /// <summary>
    /// The number of bytes in a kibibyte, equivalent to the established kilobyte factor.
    /// </summary>
    public const long BytesInKibibyte = BytesInKilobyte;
    public const long BytesInMegabyte = 1048576;
    /// <summary>
    /// The number of bytes in a mebibyte, equivalent to the established megabyte factor.
    /// </summary>
    public const long BytesInMebibyte = BytesInMegabyte;
    public const long BytesInGigabyte = 1073741824;
    /// <summary>
    /// The number of bytes in a gibibyte, equivalent to the established gigabyte factor.
    /// </summary>
    public const long BytesInGibibyte = BytesInGigabyte;
    public const long BytesInTerabyte = 1099511627776;
    /// <summary>
    /// The number of bytes in a tebibyte, equivalent to the established terabyte factor.
    /// </summary>
    public const long BytesInTebibyte = BytesInTerabyte;
    public const long BytesInPetabyte = 1000000000000000;
    public const long BytesInExabyte = 1000000000000000000;
    public const long BytesInPebibyte = 1125899906842624;

    /// <summary>Gets the number of bytes in one decimal SI kilobyte.</summary>
    public const long BytesInDecimalKilobyte = 1000;
    /// <summary>Gets the number of bytes in one decimal SI megabyte.</summary>
    public const long BytesInDecimalMegabyte = 1000000;
    /// <summary>Gets the number of bytes in one decimal SI gigabyte.</summary>
    public const long BytesInDecimalGigabyte = 1000000000;
    /// <summary>Gets the number of bytes in one decimal SI terabyte.</summary>
    public const long BytesInDecimalTerabyte = 1000000000000;
    /// <summary>Gets the number of bytes in one decimal SI petabyte.</summary>
    public const long BytesInDecimalPetabyte = 1000000000000000;
    /// <summary>Gets the number of bytes in one decimal SI exabyte.</summary>
    public const long BytesInDecimalExabyte = 1000000000000000000;

    public const string BitSymbol = "b";
    public const string Bit = "bit";
    public const string ByteSymbol = "B";
    public const string Byte = "byte";
    public const string KilobyteSymbol = "KB";
    public const string Kilobyte = "kilobyte";
    /// <summary>
    /// The symbol for a kibibyte.
    /// </summary>
    public const string KibibyteSymbol = "KiB";
    /// <summary>
    /// The name of a kibibyte.
    /// </summary>
    public const string Kibibyte = "kibibyte";
    public const string MegabyteSymbol = "MB";
    public const string Megabyte = "megabyte";
    /// <summary>
    /// The symbol for a mebibyte.
    /// </summary>
    public const string MebibyteSymbol = "MiB";
    /// <summary>
    /// The name of a mebibyte.
    /// </summary>
    public const string Mebibyte = "mebibyte";
    public const string GigabyteSymbol = "GB";
    public const string Gigabyte = "gigabyte";
    /// <summary>
    /// The symbol for a gibibyte.
    /// </summary>
    public const string GibibyteSymbol = "GiB";
    /// <summary>
    /// The name of a gibibyte.
    /// </summary>
    public const string Gibibyte = "gibibyte";
    public const string TerabyteSymbol = "TB";
    public const string Terabyte = "terabyte";
    /// <summary>
    /// The symbol for a tebibyte.
    /// </summary>
    public const string TebibyteSymbol = "TiB";
    /// <summary>
    /// The name of a tebibyte.
    /// </summary>
    public const string Tebibyte = "tebibyte";
    public const string PetabyteSymbol = "PB";
    public const string Petabyte = "petabyte";
    public const string ExabyteSymbol = "EB";
    public const string Exabyte = "exabyte";
    public const string PebibyteSymbol = "PiB";
    public const string Pebibyte = "pebibyte";

    public long Bits { get; } = (long)Math.Ceiling(byteSize * BitsInByte);
    public double Bytes { get; } = byteSize;
    public double Kilobytes { get; } = byteSize / BytesInKilobyte;
    /// <summary>
    /// Gets the size in kibibytes.
    /// </summary>
    public readonly double Kibibytes => Bytes / BytesInKibibyte;
    public double Megabytes { get; } = byteSize / BytesInMegabyte;
    /// <summary>
    /// Gets the size in mebibytes.
    /// </summary>
    public readonly double Mebibytes => Bytes / BytesInMebibyte;
    public double Gigabytes { get; } = byteSize / BytesInGigabyte;
    /// <summary>
    /// Gets the size in gibibytes.
    /// </summary>
    public readonly double Gibibytes => Bytes / BytesInGibibyte;
    public double Terabytes { get; } = byteSize / BytesInTerabyte;
    /// <summary>
    /// Gets the size in tebibytes.
    /// </summary>
    public readonly double Tebibytes => Bytes / BytesInTebibyte;
    public readonly double Petabytes => Bytes / BytesInPetabyte;
    public readonly double Exabytes => Bytes / BytesInExabyte;
    public readonly double Pebibytes => Bytes / BytesInPebibyte;
    /// <summary>Gets this value expressed in decimal SI kilobytes.</summary>
    public readonly double DecimalKilobytes => Bytes / BytesInDecimalKilobyte;
    /// <summary>Gets this value expressed in decimal SI megabytes.</summary>
    public readonly double DecimalMegabytes => Bytes / BytesInDecimalMegabyte;
    /// <summary>Gets this value expressed in decimal SI gigabytes.</summary>
    public readonly double DecimalGigabytes => Bytes / BytesInDecimalGigabyte;
    /// <summary>Gets this value expressed in decimal SI terabytes.</summary>
    public readonly double DecimalTerabytes => Bytes / BytesInDecimalTerabyte;
    /// <summary>Gets this value expressed in decimal SI petabytes.</summary>
    public readonly double DecimalPetabytes => Bytes / BytesInDecimalPetabyte;
    /// <summary>Gets this value expressed in decimal SI exabytes.</summary>
    public readonly double DecimalExabytes => Bytes / BytesInDecimalExabyte;

    public readonly string LargestWholeNumberSymbol => GetLargestWholeNumberSymbol();

    public readonly string GetLargestWholeNumberSymbol(IFormatProvider? provider = null)
    {
        var cultureFormatter = Configurator.GetFormatter(provider as CultureInfo);

        // Absolute value is used to deal with negative values
        if (Math.Abs(Exabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Exabyte, Exabytes, toSymbol: true);
        }

        if (Math.Abs(Petabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Petabyte, Petabytes, toSymbol: true);
        }

        if (Math.Abs(Terabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Terabyte, Terabytes, toSymbol: true);
        }

        if (Math.Abs(Gigabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Gigabyte, Gigabytes, toSymbol: true);
        }

        if (Math.Abs(Megabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Megabyte, Megabytes, toSymbol: true);
        }

        if (Math.Abs(Kilobytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Kilobyte, Kilobytes, toSymbol: true);
        }

        if (Math.Abs(Bytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Byte, Bytes, toSymbol: true);
        }

        return cultureFormatter.DataUnitHumanize(DataUnit.Bit, Bits, toSymbol: true);
    }

    public readonly string LargestWholeNumberFullWord => GetLargestWholeNumberFullWord();

    public readonly string GetLargestWholeNumberFullWord(IFormatProvider? provider = null)
    {
        var cultureFormatter = Configurator.GetFormatter(provider as CultureInfo);

        // Absolute value is used to deal with negative values
        if (Math.Abs(Exabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Exabyte, Exabytes, toSymbol: false);
        }

        if (Math.Abs(Petabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Petabyte, Petabytes, toSymbol: false);
        }

        if (Math.Abs(Terabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Terabyte, Terabytes, toSymbol: false);
        }

        if (Math.Abs(Gigabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Gigabyte, Gigabytes, toSymbol: false);
        }

        if (Math.Abs(Megabytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Megabyte, Megabytes, toSymbol: false);
        }

        if (Math.Abs(Kilobytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Kilobyte, Kilobytes, toSymbol: false);
        }

        if (Math.Abs(Bytes) >= 1)
        {
            return cultureFormatter.DataUnitHumanize(DataUnit.Byte, Bytes, toSymbol: false);
        }

        return cultureFormatter.DataUnitHumanize(DataUnit.Bit, Bits, toSymbol: false);
    }

    public readonly double LargestWholeNumberValue
    {
        get
        {
            // Absolute value is used to deal with negative values
            if (Math.Abs(Exabytes) >= 1)
            {
                return Exabytes;
            }

            if (Math.Abs(Petabytes) >= 1)
            {
                return Petabytes;
            }

            if (Math.Abs(Terabytes) >= 1)
            {
                return Terabytes;
            }

            if (Math.Abs(Gigabytes) >= 1)
            {
                return Gigabytes;
            }

            if (Math.Abs(Megabytes) >= 1)
            {
                return Megabytes;
            }

            if (Math.Abs(Kilobytes) >= 1)
            {
                return Kilobytes;
            }

            if (Math.Abs(Bytes) >= 1)
            {
                return Bytes;
            }

            return Bits;
        }
    }

    // Get ceiling because bis are whole units

    public static ByteSize FromBits(long value) =>
        new(value / (double)BitsInByte, value);

    internal static ByteSize FromBytesAndBits(double bytes, long bits) =>
        new(bytes, bits);

    public static ByteSize FromBytes(double value) =>
        new(value);

    public static ByteSize FromKilobytes(double value) =>
        new(value * BytesInKilobyte);

    /// <summary>
    /// Creates a byte size from a number of kibibytes.
    /// </summary>
    public static ByteSize FromKibibytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInKibibyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInKibibyte);
    }

    public static ByteSize FromMegabytes(double value) =>
        new(value * BytesInMegabyte);

    /// <summary>
    /// Creates a byte size from a number of mebibytes.
    /// </summary>
    public static ByteSize FromMebibytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInMebibyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInMebibyte);
    }

    public static ByteSize FromGigabytes(double value) =>
        new(value * BytesInGigabyte);

    /// <summary>
    /// Creates a byte size from a number of gibibytes.
    /// </summary>
    public static ByteSize FromGibibytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInGibibyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInGibibyte);
    }

    public static ByteSize FromTerabytes(double value) =>
        new(value * BytesInTerabyte);

    /// <summary>
    /// Creates a byte size from a number of tebibytes.
    /// </summary>
    public static ByteSize FromTebibytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInTebibyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInTebibyte);
    }

    public static ByteSize FromPetabytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInPetabyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInPetabyte);
    }

    public static ByteSize FromExabytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInExabyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInExabyte);
    }

    public static ByteSize FromPebibytes(double value)
    {
        if (!IsSupportedUnitValue(value, BytesInPebibyte))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * BytesInPebibyte);
    }

    /// <summary>Creates a byte size from a number of decimal SI kilobytes.</summary>
    /// <param name="value">The number of decimal SI kilobytes.</param>
    /// <returns>The equivalent byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not finite or is outside the range supported by <see cref="Bits"/>.
    /// </exception>
    public static ByteSize FromDecimalKilobytes(double value) =>
        FromDecimalUnit(value, BytesInDecimalKilobyte);

    /// <summary>Creates a byte size from a number of decimal SI megabytes.</summary>
    /// <param name="value">The number of decimal SI megabytes.</param>
    /// <returns>The equivalent byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not finite or is outside the range supported by <see cref="Bits"/>.
    /// </exception>
    public static ByteSize FromDecimalMegabytes(double value) =>
        FromDecimalUnit(value, BytesInDecimalMegabyte);

    /// <summary>Creates a byte size from a number of decimal SI gigabytes.</summary>
    /// <param name="value">The number of decimal SI gigabytes.</param>
    /// <returns>The equivalent byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not finite or is outside the range supported by <see cref="Bits"/>.
    /// </exception>
    public static ByteSize FromDecimalGigabytes(double value) =>
        FromDecimalUnit(value, BytesInDecimalGigabyte);

    /// <summary>Creates a byte size from a number of decimal SI terabytes.</summary>
    /// <param name="value">The number of decimal SI terabytes.</param>
    /// <returns>The equivalent byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not finite or is outside the range supported by <see cref="Bits"/>.
    /// </exception>
    public static ByteSize FromDecimalTerabytes(double value) =>
        FromDecimalUnit(value, BytesInDecimalTerabyte);

    /// <summary>Creates a byte size from a number of decimal SI petabytes.</summary>
    /// <param name="value">The number of decimal SI petabytes.</param>
    /// <returns>The equivalent byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not finite or is outside the range supported by <see cref="Bits"/>.
    /// </exception>
    public static ByteSize FromDecimalPetabytes(double value) =>
        FromDecimalUnit(value, BytesInDecimalPetabyte);

    /// <summary>Creates a byte size from a number of decimal SI exabytes.</summary>
    /// <param name="value">The number of decimal SI exabytes.</param>
    /// <returns>The equivalent byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="value"/> is not finite or is outside the range supported by <see cref="Bits"/>.
    /// </exception>
    public static ByteSize FromDecimalExabytes(double value) =>
        FromDecimalUnit(value, BytesInDecimalExabyte);

    static ByteSize FromDecimalUnit(double value, long bytesPerUnit)
    {
        if (!IsSupportedUnitValue(value, bytesPerUnit))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value must be finite and fit the range supported by ByteSize.Bits.");
        }

        return new(value * bytesPerUnit);
    }

    /// <summary>
    /// Converts the value of the current ByteSize object to a string.
    /// The metric prefix symbol (bit, byte, kilo, mega, giga, tera, peta, exa) used is
    /// the largest metric prefix such that the corresponding value is greater
    ///  than or equal to one.
    /// </summary>
    public readonly override string ToString() =>
        ToString(CultureInfo.CurrentCulture);

    public readonly string ToString(IFormatProvider? provider)
    {
        provider ??= CultureInfo.CurrentCulture;

        var culture = provider as CultureInfo;

        // Only inject override for culture-backed providers, never for caller-supplied custom NFI
        if (culture is not null)
        {
            provider = LocaleNumberFormattingOverrides.GetFormattingNumberFormat(culture);
        }

        return string.Concat(
            LargestWholeNumberValue.ToString("0.##", provider),
            " ",
            GetLargestWholeNumberSymbol(culture));
    }

    public readonly string ToString(string? format) =>
        ToString(format, CultureInfo.CurrentCulture);

    public readonly string ToString(string? format, IFormatProvider? provider) =>
        ToString(format, provider, toSymbol: true);

    readonly string ToString(string? format, IFormatProvider? provider, bool toSymbol)
    {
        format ??= "G";
        provider ??= CultureInfo.CurrentCulture;

        if (format == "G")
        {
            format = "0.##";
        }

        if (format.IndexOfAny(['#', '0']) < 0)
        {
            format = string.Concat("0.## ", format);
        }

        format = format.Replace("#.##", "0.##");

        var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;

        // Only inject override for culture-backed providers, never for caller-supplied custom NFI
        if (provider is CultureInfo overrideCulture)
        {
            provider = LocaleNumberFormattingOverrides.GetFormattingNumberFormat(overrideCulture);
        }

        bool has(string s) => format.Contains(s, StringComparison.OrdinalIgnoreCase);
        string replace(string source, string oldValue, string newValue)
        {
            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, oldValue, CompareOptions.OrdinalIgnoreCase);
            return source.Remove(index, oldValue.Length).Insert(index, newValue);
        }

        string output(double n) => n.ToString(format, provider);

        var cultureFormatter = Configurator.GetFormatter(culture);

        if (has(PebibyteSymbol))
        {
            format = replace(format, PebibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Pebibyte, Pebibytes, toSymbol));
            return output(Pebibytes);
        }

        if (has(TebibyteSymbol))
        {
            format = replace(format, TebibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Tebibyte, Tebibytes, toSymbol));
            return output(Tebibytes);
        }

        if (has(GibibyteSymbol))
        {
            format = replace(format, GibibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Gibibyte, Gibibytes, toSymbol));
            return output(Gibibytes);
        }

        if (has(MebibyteSymbol))
        {
            format = replace(format, MebibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Mebibyte, Mebibytes, toSymbol));
            return output(Mebibytes);
        }

        if (has(KibibyteSymbol))
        {
            format = replace(format, KibibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Kibibyte, Kibibytes, toSymbol));
            return output(Kibibytes);
        }

        if (has(ExabyteSymbol))
        {
            format = replace(format, ExabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Exabyte, Exabytes, toSymbol));
            return output(Exabytes);
        }

        if (has(PetabyteSymbol))
        {
            format = replace(format, PetabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Petabyte, Petabytes, toSymbol));
            return output(Petabytes);
        }

        if (has(TerabyteSymbol))
        {
            format = replace(format, TerabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Terabyte, Terabytes, toSymbol));
            return output(Terabytes);
        }

        if (has(GigabyteSymbol))
        {
            format = replace(format, GigabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Gigabyte, Gigabytes, toSymbol));
            return output(Gigabytes);
        }

        if (has(MegabyteSymbol))
        {
            format = replace(format, MegabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Megabyte, Megabytes, toSymbol));
            return output(Megabytes);
        }

        if (has(KilobyteSymbol))
        {
            format = replace(format, KilobyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Kilobyte, Kilobytes, toSymbol));
            return output(Kilobytes);
        }

        // Byte and Bit symbol look must be case-sensitive
        if (format.Contains(ByteSymbol, StringComparison.Ordinal))
        {
            format = format.Replace(ByteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Byte, Bytes, toSymbol));
            return output(Bytes);
        }

        if (format.Contains(BitSymbol, StringComparison.Ordinal))
        {
            format = format.Replace(BitSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Bit, Bits, toSymbol));
            return output(Bits);
        }

        var formattedLargeWholeNumberValue = LargestWholeNumberValue.ToString(format, provider);

        formattedLargeWholeNumberValue = formattedLargeWholeNumberValue.Equals(string.Empty)
            ? "0"
            : formattedLargeWholeNumberValue;

        return string.Concat(
            formattedLargeWholeNumberValue,
            " ",
            toSymbol ? GetLargestWholeNumberSymbol(culture) : GetLargestWholeNumberFullWord(culture));
    }

    /// <summary>
    /// Converts the value of the current ByteSize object to a string with
    /// full words. The metric prefix symbol (bit, byte, kilo, mega, giga,
    /// tera, peta, exa) used is the largest metric prefix such that the corresponding
    /// value is greater than or equal to one.
    /// </summary>
    public readonly string ToFullWords(string? format = null, IFormatProvider? provider = null) =>
        ToString(format, provider, toSymbol: false);

    /// <summary>
    /// Formats this value with an explicitly selected unit system.
    /// </summary>
    /// <param name="unitSystem">The unit system to use.</param>
    /// <param name="format">
    /// The numeric format and optional unit token. SI/IEC prefixed unit tokens are matched case-insensitively,
    /// while <c>b</c> and <c>B</c> remain case-sensitive; output uses canonical symbol casing.
    /// </param>
    /// <param name="formatProvider">The provider used to format the numeric value.</param>
    /// <returns>The formatted byte size.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid, or selects a token not supported by the selected non-legacy system.
    /// </exception>
    public readonly string Format(
        ByteSizeUnitSystem unitSystem,
        string? format = null,
        IFormatProvider? formatProvider = null) =>
        unitSystem == ByteSizeUnitSystem.Legacy
            ? ToString(format, formatProvider)
            : FormatWithUnitSystem(unitSystem, format, formatProvider, toSymbol: true);

    /// <summary>
    /// Formats this value with localized unit words from an explicitly selected unit system.
    /// </summary>
    /// <param name="unitSystem">The unit system to use.</param>
    /// <param name="format">
    /// The numeric format and optional unit token. SI/IEC prefixed unit tokens are matched case-insensitively,
    /// while <c>b</c> and <c>B</c> remain case-sensitive; localized words replace the selected token.
    /// </param>
    /// <param name="formatProvider">The provider used to format the numeric value and select localized unit words.</param>
    /// <returns>The formatted byte size using localized unit words.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    /// <exception cref="FormatException">
    /// <paramref name="format"/> is invalid, or selects a token not supported by the selected non-legacy system.
    /// </exception>
    public readonly string FormatFullWords(
        ByteSizeUnitSystem unitSystem,
        string? format = null,
        IFormatProvider? formatProvider = null) =>
        unitSystem == ByteSizeUnitSystem.Legacy
            ? ToFullWords(format, formatProvider)
            : FormatWithUnitSystem(unitSystem, format, formatProvider, toSymbol: false);

    readonly string FormatWithUnitSystem(
        ByteSizeUnitSystem unitSystem,
        string? format,
        IFormatProvider? formatProvider,
        bool toSymbol)
    {
        ValidateUnitSystem(unitSystem);
        formatProvider ??= CultureInfo.CurrentCulture;
        var culture = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
        if (formatProvider is CultureInfo overrideCulture)
        {
            formatProvider = LocaleNumberFormattingOverrides.GetFormattingNumberFormat(overrideCulture);
        }

        var units = GetUnits(unitSystem);
        var explicitUnit = FindFormatUnit(format, units);
        var unit = explicitUnit ?? SelectUnit(units);
        var value = unit.DataUnit == DataUnit.Bit
            ? explicitUnit is not null
                ? Bits
                : (Bytes < 0 ? -1 : 1) * Math.Ceiling(Math.Abs(Bytes) * BitsInByte)
            : Bytes / unit.Bytes;
        var numericFormat = string.IsNullOrWhiteSpace(format) || format == "G" ? "0.##" : format!;

        if (explicitUnit is null)
        {
            var systemRadix = unitSystem == ByteSizeUnitSystem.DecimalSi ? 1000d : 1024d;
            var unitIndex = Array.IndexOf(units, unit);
            SystemUnit? nextUnit = unit.DataUnit switch
            {
                DataUnit.Byte => units[^1],
                _ when unitIndex > 0 => units[unitIndex - 1],
                _ => null
            };
            if (nextUnit is { } promotedUnit &&
                TryGetDisplayedCount(
                    value,
                    numericFormat.Replace("#.##", "0.##"),
                    formatProvider,
                    out var displayedValue) &&
                Math.Abs(displayedValue) >= (decimal)systemRadix)
            {
                unit = promotedUnit;
                value = Bytes / unit.Bytes;
            }
        }

        var phraseCount = unit.DataUnit == DataUnit.Bit && explicitUnit is not null ? Bits : value;
        decimal? exactPhraseCount = unit.DataUnit == DataUnit.Bit && explicitUnit is not null ? Bits : null;
        if (!toSymbol)
        {
            var countFormat = explicitUnit is { } selectedUnit
                ? ReplaceFormatToken(numericFormat, selectedUnit.Symbol, string.Empty).Trim()
                : numericFormat;
            if (countFormat.Length > 0)
            {
                var resolvedCountFormat = countFormat.Replace("#.##", "0.##");
                var hasDisplayedCount = unit.DataUnit == DataUnit.Bit && explicitUnit is not null
                    ? TryGetDisplayedCount(Bits, resolvedCountFormat, formatProvider, out var displayedDecimal)
                    : TryGetDisplayedCount(value, resolvedCountFormat, formatProvider, out displayedDecimal);
                if (hasDisplayedCount)
                {
                    exactPhraseCount = displayedDecimal;
                }
            }
        }

        var formatter = Configurator.GetFormatter(culture);
        var builtInFormatter = formatter as DefaultFormatter;
        if (builtInFormatter is not null &&
            formatter.GetType().Assembly != typeof(DefaultFormatter).Assembly)
        {
            builtInFormatter = null;
        }

        var unitText = toSymbol
            ? unit.Symbol
            : builtInFormatter is not null
                ? exactPhraseCount is { } exactCount
                    ? builtInFormatter.DataUnitHumanizeExact(unit.DataUnit, exactCount, toSymbol: false)
                    : builtInFormatter.DataUnitHumanize(unit.DataUnit, Math.Abs(phraseCount), toSymbol: false)
                : formatter.DataUnitHumanize(unit.DataUnit, phraseCount, toSymbol: false);

        if (explicitUnit is { } selected)
        {
            var standardFormat = ReplaceFormatToken(numericFormat, selected.Symbol, string.Empty).Trim();
            if (IsStandardNumericFormat(standardFormat))
            {
                var formattedValue = unit.DataUnit == DataUnit.Bit
                    ? Bits.ToString(standardFormat, formatProvider)
                    : value.ToString(standardFormat, formatProvider);
                return ReplaceFormatToken(
                    ReplaceFormatToken(numericFormat, standardFormat, formattedValue),
                    selected.Symbol,
                    unitText);
            }

            numericFormat = ReplaceFormatToken(numericFormat, selected.Symbol, unitText);
            if (numericFormat.IndexOfAny(['#', '0']) < 0)
            {
                numericFormat = string.Concat("0.## ", numericFormat);
            }

            var resolvedFormat = numericFormat.Replace("#.##", "0.##");
            return unit.DataUnit == DataUnit.Bit
                ? Bits.ToString(resolvedFormat, formatProvider)
                : value.ToString(resolvedFormat, formatProvider);
        }

        return string.Concat(value.ToString(numericFormat.Replace("#.##", "0.##"), formatProvider), " ", unitText);
    }

    static bool TryGetDisplayedCount(
        double value,
        string format,
        IFormatProvider formatProvider,
        out decimal count) =>
        TryParseDisplayedCount(value.ToString(SanitizeNumericFormat(format), formatProvider), formatProvider, out count);

    static bool TryGetDisplayedCount(
        long value,
        string format,
        IFormatProvider formatProvider,
        out decimal count) =>
        TryParseDisplayedCount(value.ToString(SanitizeNumericFormat(format), formatProvider), formatProvider, out count);

    static bool TryParseDisplayedCount(string value, IFormatProvider formatProvider, out decimal count)
    {
        const NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands;
        if (decimal.TryParse(value, styles, formatProvider, out count))
        {
            return true;
        }

        var numberFormat = NumberFormatInfo.GetInstance(formatProvider);
        var withoutDecorators = value;
        if (numberFormat.PercentSymbol.Length > 0)
        {
            withoutDecorators = withoutDecorators.Replace(numberFormat.PercentSymbol, string.Empty);
        }

        if (numberFormat.PerMilleSymbol.Length > 0)
        {
            withoutDecorators = withoutDecorators.Replace(numberFormat.PerMilleSymbol, string.Empty);
        }

        return decimal.TryParse(withoutDecorators.Trim(), styles, formatProvider, out count);
    }

    static string SanitizeNumericFormat(string format)
    {
        if (IsStandardNumericFormat(format))
        {
            return format;
        }

        var result = new StringBuilder(format.Length);
        var quote = '\0';
        for (var index = 0; index < format.Length; index++)
        {
            var character = format[index];
            if (quote != '\0')
            {
                if (character == '\\')
                {
                    index++;
                    continue;
                }

                if (character == quote)
                {
                    quote = '\0';
                }

                continue;
            }

            if (character is '\'' or '"')
            {
                quote = character;
                continue;
            }

            if (character == '\\')
            {
                index++;
                continue;
            }

            if (character is '0' or '#' or '.' or ',' or '%' or '‰' or ';')
            {
                result.Append(character);
                continue;
            }

            if (character is 'E' or 'e' &&
                index + 1 < format.Length &&
                format[index + 1] is '+' or '-' or '0')
            {
                result.Append(character);
                if (format[index + 1] is '+' or '-')
                {
                    result.Append(format[++index]);
                }
            }
        }

        return result.Length == 0 ? "0.##" : result.ToString();
    }

    static bool IsStandardNumericFormat(string format)
    {
        if (format.Length == 0 ||
            format[0] is not (>= 'A' and <= 'Z') and not (>= 'a' and <= 'z'))
        {
            return false;
        }

        for (var index = 1; index < format.Length; index++)
        {
            if (format[index] is < '0' or > '9')
            {
                return false;
            }
        }

        return true;
    }

    readonly SystemUnit SelectUnit(SystemUnit[] units)
    {
        var absoluteBytes = Math.Abs(Bytes);
        foreach (var unit in units)
        {
            if (absoluteBytes >= unit.Bytes)
            {
                return unit;
            }
        }

        return absoluteBytes >= 1
            ? new(1, ByteSymbol, DataUnit.Byte)
            : new(1d / BitsInByte, BitSymbol, DataUnit.Bit);
    }

    static SystemUnit? FindFormatUnit(string? format, SystemUnit[] units)
    {
        if (string.IsNullOrWhiteSpace(format) || format == "G")
        {
            return null;
        }

        var maskedFormat = MaskFormatLiterals(format!);
        var incompatibleUnits = ReferenceEquals(units, DecimalUnits) ? BinaryUnits : DecimalUnits;
        foreach (var unit in incompatibleUnits)
        {
            if (maskedFormat.Contains(unit.Symbol, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException($"Unit token '{unit.Symbol}' does not belong to the selected byte-size unit system.");
            }
        }

        if (maskedFormat.Contains("EiB", StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException("EiB is outside the range supported by ByteSize.Bits.");
        }

        SystemUnit? selectedUnit = null;
        var remainingFormat = maskedFormat;
        foreach (var unit in units)
        {
            if (!remainingFormat.Contains(unit.Symbol, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (selectedUnit is not null && selectedUnit.Value.DataUnit != unit.DataUnit)
            {
                throw new FormatException("A byte-size format can contain only one distinct unit token.");
            }

            selectedUnit = unit;
            remainingFormat = ReplaceOrdinalIgnoreCase(
                remainingFormat,
                unit.Symbol,
                new(' ', unit.Symbol.Length));
        }

        var containsBytes = remainingFormat.Contains(ByteSymbol, StringComparison.Ordinal);
        var containsBits = remainingFormat.Contains(BitSymbol, StringComparison.Ordinal);
        var tokenCount = (selectedUnit is null ? 0 : 1) + (containsBytes ? 1 : 0) + (containsBits ? 1 : 0);
        if (tokenCount > 1)
        {
            throw new FormatException("A byte-size format can contain only one distinct unit token.");
        }

        if (selectedUnit is not null)
        {
            return selectedUnit;
        }

        if (containsBytes)
        {
            return new(1, ByteSymbol, DataUnit.Byte);
        }

        return containsBits
            ? new(1d / BitsInByte, BitSymbol, DataUnit.Bit)
            : null;
    }

    static string MaskFormatLiterals(string format)
    {
        var result = format.ToCharArray();
        var quote = '\0';
        for (var index = 0; index < result.Length; index++)
        {
            var character = result[index];
            if (quote != '\0')
            {
                result[index] = '\0';
                if (character == '\\' && ++index < result.Length)
                {
                    result[index] = '\0';
                    continue;
                }

                if (character == quote)
                {
                    quote = '\0';
                }

                continue;
            }

            if (character is '\'' or '"')
            {
                quote = character;
                result[index] = '\0';
            }
            else if (character == '\\')
            {
                result[index] = '\0';
                if (++index < result.Length)
                {
                    result[index] = '\0';
                }
            }
        }

        return new(result);
    }

    static string ReplaceFormatToken(string format, string token, string replacement)
    {
        var maskedFormat = MaskFormatLiterals(format);
        var searchStart = 0;
        var indexes = new List<int>();
        while (true)
        {
            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                maskedFormat,
                token,
                searchStart,
                CompareOptions.OrdinalIgnoreCase);
            if (index < 0)
            {
                break;
            }

            indexes.Add(index);
            searchStart = index + token.Length;
        }

        for (var index = indexes.Count - 1; index >= 0; index--)
        {
            format = format.Remove(indexes[index], token.Length).Insert(indexes[index], replacement);
        }

        return format;
    }

    static string ReplaceOrdinalIgnoreCase(string value, string oldValue, string newValue)
    {
        var searchStart = 0;
        while (true)
        {
            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                value,
                oldValue,
                searchStart,
                CompareOptions.OrdinalIgnoreCase);
            if (index < 0)
            {
                return value;
            }

            value = value.Remove(index, oldValue.Length).Insert(index, newValue);
            searchStart = index + newValue.Length;
        }
    }

    static SystemUnit[] GetUnits(ByteSizeUnitSystem unitSystem) =>
        unitSystem switch
        {
            ByteSizeUnitSystem.DecimalSi => DecimalUnits,
            ByteSizeUnitSystem.BinaryIec => BinaryUnits,
            _ => throw new ArgumentOutOfRangeException(nameof(unitSystem), unitSystem, "Unknown byte-size unit system.")
        };

    static readonly SystemUnit[] DecimalUnits =
    [
        new(BytesInDecimalExabyte, "EB", DataUnit.DecimalExabyte),
        new(BytesInDecimalPetabyte, "PB", DataUnit.DecimalPetabyte),
        new(BytesInDecimalTerabyte, "TB", DataUnit.DecimalTerabyte),
        new(BytesInDecimalGigabyte, "GB", DataUnit.DecimalGigabyte),
        new(BytesInDecimalMegabyte, "MB", DataUnit.DecimalMegabyte),
        new(BytesInDecimalKilobyte, "kB", DataUnit.DecimalKilobyte)
    ];

    static readonly SystemUnit[] BinaryUnits =
    [
        new(BytesInPebibyte, "PiB", DataUnit.BinaryPebibyte),
        new(BytesInTebibyte, "TiB", DataUnit.BinaryTebibyte),
        new(BytesInGibibyte, "GiB", DataUnit.BinaryGibibyte),
        new(BytesInMebibyte, "MiB", DataUnit.BinaryMebibyte),
        new(BytesInKibibyte, "KiB", DataUnit.BinaryKibibyte)
    ];

    readonly record struct SystemUnit(double Bytes, string Symbol, DataUnit DataUnit);

    public readonly override bool Equals(object? value)
    {
        if (value == null)
        {
            return false;
        }

        ByteSize other;
        if (value is ByteSize size)
        {
            other = size;
        }
        else
        {
            return false;
        }

        return Equals(other);
    }

    public readonly bool Equals(ByteSize value) =>
        Bits == value.Bits;

    public readonly override int GetHashCode() =>
        Bits.GetHashCode();

    public readonly int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is ByteSize size)
        {
            return CompareTo(size);
        }

        throw new ArgumentException("Object is not a ByteSize");
    }

    public readonly int CompareTo(ByteSize other) =>
        Bits.CompareTo(other.Bits);

    public readonly ByteSize Add(ByteSize bs) =>
        new(Bytes + bs.Bytes);

    public readonly ByteSize AddBits(long value) =>
        this + FromBits(value);

    public readonly ByteSize AddBytes(double value) =>
        this + FromBytes(value);

    public readonly ByteSize AddKilobytes(double value) =>
        this + FromKilobytes(value);

    public readonly ByteSize AddMegabytes(double value) =>
        this + FromMegabytes(value);

    public readonly ByteSize AddGigabytes(double value) =>
        this + FromGigabytes(value);

    public readonly ByteSize AddTerabytes(double value) =>
        this + FromTerabytes(value);

    public readonly ByteSize AddPetabytes(double value) =>
        AddUnit(Petabytes, value, BytesInPetabyte);

    public readonly ByteSize AddExabytes(double value) =>
        AddUnit(Exabytes, value, BytesInExabyte);

    public readonly ByteSize AddPebibytes(double value) =>
        AddUnit(Pebibytes, value, BytesInPebibyte);

    public readonly ByteSize Subtract(ByteSize bs) =>
        new(Bytes - bs.Bytes);

    public static ByteSize operator +(ByteSize b1, ByteSize b2) =>
        new(b1.Bytes + b2.Bytes);

    public static ByteSize operator -(ByteSize b1, ByteSize b2) =>
        new(b1.Bytes - b2.Bytes);

    public static ByteSize operator ++(ByteSize b) =>
        new(b.Bytes + 1);

    public static ByteSize operator -(ByteSize b) =>
        new(-b.Bytes);

    public static ByteSize operator --(ByteSize b) =>
        new(b.Bytes - 1);

    public static bool operator ==(ByteSize b1, ByteSize b2) =>
        b1.Bits == b2.Bits;

    public static bool operator !=(ByteSize b1, ByteSize b2) =>
        b1.Bits != b2.Bits;

    public static bool operator <(ByteSize b1, ByteSize b2) =>
        b1.Bits < b2.Bits;

    public static bool operator <=(ByteSize b1, ByteSize b2) =>
        b1.Bits <= b2.Bits;

    public static bool operator >(ByteSize b1, ByteSize b2) =>
        b1.Bits > b2.Bits;

    public static bool operator >=(ByteSize b1, ByteSize b2) =>
        b1.Bits >= b2.Bits;

    public static bool TryParse(string? s, out ByteSize result) =>
        TryParse(s, null, out result);

    public static bool TryParse(CharSpan s, out ByteSize result) =>
        TryParse(s, null, out result);

    public static bool TryParse(string? s, IFormatProvider? formatProvider, out ByteSize result) =>
        TryParse(s.AsSpan(), formatProvider, out result);

    public static bool TryParse(CharSpan s, IFormatProvider? formatProvider, out ByteSize result)
    {
        // Arg checking
        s = s.TrimStart(); // Protect against leading spaces
        if (s.IsEmpty)
        {
            result = default;
            return false;
        }

        // Acquiring culture-specific parsing info — apply decimal separator override for culture-backed providers
        if (formatProvider is CultureInfo parseCulture
            && LocaleNumberFormattingOverrides.TryGetDecimalSeparator(parseCulture, out var parseSep))
        {
            formatProvider = LocaleNumberFormattingOverrides.GetCachedNumberFormat(parseCulture, parseSep!);
        }

        var numberFormat = NumberFormatInfo.GetInstance(formatProvider);

        // Get or create cached set of special characters from number format strings
        // Note: These can be multi-character strings in some cultures (e.g., Arabic)
        var specialCharsSet = NumberFormatSpecialCharsCache.GetValue(
            numberFormat,
            static nfi => new HashSet<char>(
                nfi.NumberDecimalSeparator
                    .Concat(nfi.NumberGroupSeparator)
                    .Concat(nfi.PositiveSign)
                    .Concat(nfi.NegativeSign)
            )
        );

        // Setup the result
        result = default;

        // Pick first non-digit number
        int lastNumber;
        for (lastNumber = 0; lastNumber < s.Length; lastNumber++)
        {
            if (!(char.IsDigit(s[lastNumber]) || specialCharsSet.Contains(s[lastNumber])))
            {
                break;
            }
        }

        if (lastNumber == s.Length)
        {
            return false;
        }

        // Cut the input string in half
        var numberPart = s
[..lastNumber]
            .Trim();
        var sizePart = s[lastNumber..]
            .Trim();

        if (sizePart.Length is < 1 or > 3)
        {
            return false;
        }

        // Get the numeric part
        const NumberStyles numberStyles = AllowDecimalPoint | AllowThousands | AllowLeadingSign;
        if (!double.TryParse(
#if NET
            numberPart,
#else
            numberPart.ToString(),
#endif
            numberStyles, formatProvider, out var number))
        {
            return false;
        }

        Span<char> sizePartUpper = stackalloc char[sizePart.Length];
        var upperLength = sizePart.ToUpperInvariant(sizePartUpper);
        Debug.Assert(sizePartUpper.Length == upperLength);

        // Get the magnitude part
        switch (sizePartUpper)
        {
            case ByteSymbol:
                if (sizePart.SequenceEqual(BitSymbol))
                {
                    if (!double.IsFinite(number))
                    {
                        return false;
                    }

                    if (number != Math.Truncate(number))
                    {
                        return false;
                    }

                    if (number < long.MinValue || number > long.MaxValue)
                    {
                        return false;
                    }

                    result = FromBits((long)number);
                }
                else
                {
                    result = FromBytes(number);
                }

                break;

            case KilobyteSymbol:
                result = FromKilobytes(number);
                break;

            case "KIB":
                if (!IsSupportedUnitValue(number, BytesInKibibyte))
                {
                    return false;
                }

                result = FromKibibytes(number);
                break;

            case MegabyteSymbol:
                result = FromMegabytes(number);
                break;

            case "MIB":
                if (!IsSupportedUnitValue(number, BytesInMebibyte))
                {
                    return false;
                }

                result = FromMebibytes(number);
                break;

            case GigabyteSymbol:
                result = FromGigabytes(number);
                break;

            case "GIB":
                if (!IsSupportedUnitValue(number, BytesInGibibyte))
                {
                    return false;
                }

                result = FromGibibytes(number);
                break;

            case TerabyteSymbol:
                result = FromTerabytes(number);
                break;

            case "TIB":
                if (!IsSupportedUnitValue(number, BytesInTebibyte))
                {
                    return false;
                }

                result = FromTebibytes(number);
                break;

            case PetabyteSymbol:
                if (!IsSupportedUnitValue(number, BytesInPetabyte))
                {
                    return false;
                }

                result = FromPetabytes(number);
                break;

            case ExabyteSymbol:
                if (!IsSupportedUnitValue(number, BytesInExabyte))
                {
                    return false;
                }

                result = FromExabytes(number);
                break;

            case "PIB":
                if (!IsSupportedUnitValue(number, BytesInPebibyte))
                {
                    return false;
                }

                result = FromPebibytes(number);
                break;

            default:
                return false;
        }

        return true;
    }

    /// <summary>
    /// Parses a byte size using only the tokens defined by the selected unit system.
    /// </summary>
    /// <param name="s">The text to parse.</param>
    /// <param name="unitSystem">The unit system whose tokens are accepted.</param>
    /// <param name="formatProvider">The provider used to parse the numeric value.</param>
    /// <returns>The parsed byte size.</returns>
    /// <remarks>
    /// SI/IEC prefixed unit tokens are matched case-insensitively, while <c>b</c> and <c>B</c> remain case-sensitive.
    /// Legacy parsing preserves the established behavior.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not valid for the selected unit system.</exception>
    public static ByteSize ParseWithUnitSystem(
        string s,
        ByteSizeUnitSystem unitSystem,
        IFormatProvider? formatProvider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        if (TryParseSpanWithUnitSystem(s.AsSpan(), unitSystem, formatProvider, out var result))
        {
            return result;
        }

        throw new FormatException("Value is not in the correct format");
    }

    /// <summary>
    /// Attempts to parse a byte size using only the tokens defined by the selected unit system.
    /// </summary>
    /// <param name="s">The text to parse.</param>
    /// <param name="unitSystem">The unit system whose tokens are accepted.</param>
    /// <param name="result">
    /// When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value.
    /// </param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// SI/IEC prefixed unit tokens are matched case-insensitively, while <c>b</c> and <c>B</c> remain case-sensitive.
    /// Legacy parsing preserves the established behavior.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    public static bool TryParseWithUnitSystem(
        string? s,
        ByteSizeUnitSystem unitSystem,
        out ByteSize result) =>
        TryParseSpanWithUnitSystem(s.AsSpan(), unitSystem, null, out result);

    /// <summary>
    /// Attempts to parse a byte size using only the tokens defined by the selected unit system.
    /// </summary>
    /// <param name="s">The text to parse.</param>
    /// <param name="unitSystem">The unit system whose tokens are accepted.</param>
    /// <param name="formatProvider">The provider used to parse the numeric value.</param>
    /// <param name="result">
    /// When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value.
    /// </param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// SI/IEC prefixed unit tokens are matched case-insensitively, while <c>b</c> and <c>B</c> remain case-sensitive.
    /// Legacy parsing preserves the established behavior.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    public static bool TryParseWithUnitSystem(
        string? s,
        ByteSizeUnitSystem unitSystem,
        IFormatProvider? formatProvider,
        out ByteSize result) =>
        TryParseSpanWithUnitSystem(s.AsSpan(), unitSystem, formatProvider, out result);

    /// <summary>
    /// Attempts to parse a byte-size character span using only the tokens defined by the selected unit system.
    /// </summary>
    /// <param name="s">The character span to parse.</param>
    /// <param name="unitSystem">The unit system whose tokens are accepted.</param>
    /// <param name="result">
    /// When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value.
    /// </param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// SI/IEC prefixed unit tokens are matched case-insensitively, while <c>b</c> and <c>B</c> remain case-sensitive.
    /// Legacy parsing preserves the established behavior.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    public static bool TryParseSpanWithUnitSystem(
        CharSpan s,
        ByteSizeUnitSystem unitSystem,
        out ByteSize result) =>
        TryParseSpanWithUnitSystem(s, unitSystem, null, out result);

    /// <summary>
    /// Attempts to parse a byte-size character span using only the tokens defined by the selected unit system.
    /// </summary>
    /// <param name="s">The character span to parse.</param>
    /// <param name="unitSystem">The unit system whose tokens are accepted.</param>
    /// <param name="formatProvider">The provider used to parse the numeric value.</param>
    /// <param name="result">
    /// When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value.
    /// </param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// SI/IEC prefixed unit tokens are matched case-insensitively, while <c>b</c> and <c>B</c> remain case-sensitive.
    /// Legacy parsing preserves the established behavior.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="unitSystem"/> is not defined.</exception>
    public static bool TryParseSpanWithUnitSystem(
        CharSpan s,
        ByteSizeUnitSystem unitSystem,
        IFormatProvider? formatProvider,
        out ByteSize result)
    {
        ValidateUnitSystem(unitSystem);
        if (unitSystem == ByteSizeUnitSystem.Legacy)
        {
            return TryParse(s, formatProvider, out result);
        }

        s = s.TrimStart();
        result = default;
        if (s.IsEmpty)
        {
            return false;
        }

        if (formatProvider is CultureInfo parseCulture &&
            LocaleNumberFormattingOverrides.TryGetDecimalSeparator(parseCulture, out var parseSeparator))
        {
            formatProvider = LocaleNumberFormattingOverrides.GetCachedNumberFormat(parseCulture, parseSeparator!);
        }

        var numberFormat = NumberFormatInfo.GetInstance(formatProvider);
        var specialCharacters = NumberFormatSpecialCharsCache.GetValue(
            numberFormat,
            static value => new HashSet<char>(
                value.NumberDecimalSeparator
                    .Concat(value.NumberGroupSeparator)
                    .Concat(value.PositiveSign)
                    .Concat(value.NegativeSign)));

        var lastNumber = 0;
        while (lastNumber < s.Length &&
               (char.IsDigit(s[lastNumber]) || specialCharacters.Contains(s[lastNumber])))
        {
            lastNumber++;
        }

        if (lastNumber == s.Length)
        {
            return false;
        }

        var numberPart = s[..lastNumber].Trim();
        var unitPart = s[lastNumber..].Trim();
        if (unitPart.Length is < 1 or > 3)
        {
            return false;
        }

        const NumberStyles numberStyles = AllowDecimalPoint | AllowThousands | AllowLeadingSign;
        if (!double.TryParse(
#if NET
            numberPart,
#else
            numberPart.ToString(),
#endif
            numberStyles,
            formatProvider,
            out var number))
        {
            return false;
        }

        if (unitPart.SequenceEqual(BitSymbol))
        {
            if (!long.TryParse(
#if NET
                numberPart,
#else
                numberPart.ToString(),
#endif
                AllowThousands | AllowLeadingSign,
                formatProvider,
                out var bits))
            {
                return false;
            }

            result = FromBits(bits);
            return true;
        }

        if (unitPart.SequenceEqual(ByteSymbol))
        {
            if (!IsSupportedUnitValue(number, 1))
            {
                return false;
            }

            result = FromBytes(number);
            return true;
        }

        var units = GetUnits(unitSystem);
        foreach (var unit in units)
        {
            if (!unitPart.Equals(unit.Symbol, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!IsSupportedUnitValue(number, unit.Bytes))
            {
                return false;
            }

            result = new(number * unit.Bytes);
            return true;
        }

        return false;
    }

    static void ValidateUnitSystem(ByteSizeUnitSystem unitSystem)
    {
        if (unitSystem is < ByteSizeUnitSystem.Legacy or > ByteSizeUnitSystem.BinaryIec)
        {
            throw new ArgumentOutOfRangeException(nameof(unitSystem), unitSystem, "Unknown byte-size unit system.");
        }
    }

    static bool IsSupportedUnitValue(double value, double bytesPerUnit)
    {
        var bytes = value * bytesPerUnit;
        return double.IsFinite(bytes) &&
            bytes >= -BytesInPebibyte * 1024d &&
            bytes < BytesInPebibyte * 1024d;
    }

    static ByteSize AddUnit(double current, double value, double bytesPerUnit)
    {
        var result = current + value;
        if (!IsSupportedUnitValue(result, bytesPerUnit))
        {
            throw new OverflowException("The result is outside the range supported by ByteSize.Bits.");
        }

        return new(result * bytesPerUnit);
    }

    public static ByteSize Parse(string s) =>
        Parse(s, null);

    public static ByteSize Parse(string s, IFormatProvider? formatProvider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, formatProvider, out var result))
        {
            return result;
        }

        throw new FormatException("Value is not in the correct format");
    }
}
#pragma warning restore 1591