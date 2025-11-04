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
    static readonly ConditionalWeakTable<NumberFormatInfo, HashSet<char>> NumberFormatSpecialCharsCache = new();

    public static readonly ByteSize MinValue = FromBits(long.MinValue);
    public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

    public const long BitsInByte = 8;
    public const long BytesInKilobyte = 1024;
    public const long BytesInMegabyte = 1048576;
    public const long BytesInGigabyte = 1073741824;
    public const long BytesInTerabyte = 1099511627776;

    public const string BitSymbol = "b";
    public const string Bit = "bit";
    public const string ByteSymbol = "B";
    public const string Byte = "byte";
    public const string KilobyteSymbol = "KB";
    public const string Kilobyte = "kilobyte";
    public const string MegabyteSymbol = "MB";
    public const string Megabyte = "megabyte";
    public const string GigabyteSymbol = "GB";
    public const string Gigabyte = "gigabyte";
    public const string TerabyteSymbol = "TB";
    public const string Terabyte = "terabyte";

    public long Bits { get; } = (long)Math.Ceiling(byteSize * BitsInByte);
    public double Bytes { get; } = byteSize;
    public double Kilobytes { get; } = byteSize / BytesInKilobyte;
    public double Megabytes { get; } = byteSize / BytesInMegabyte;
    public double Gigabytes { get; } = byteSize / BytesInGigabyte;
    public double Terabytes { get; } = byteSize / BytesInTerabyte;

    public readonly string LargestWholeNumberSymbol => GetLargestWholeNumberSymbol();

    public readonly string GetLargestWholeNumberSymbol(IFormatProvider? provider = null)
    {
        var cultureFormatter = Configurator.GetFormatter(provider as CultureInfo);

        // Absolute value is used to deal with negative values
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
        new(value / (double)BitsInByte);

    public static ByteSize FromBytes(double value) =>
        new(value);

    public static ByteSize FromKilobytes(double value) =>
        new(value * BytesInKilobyte);

    public static ByteSize FromMegabytes(double value) =>
        new(value * BytesInMegabyte);

    public static ByteSize FromGigabytes(double value) =>
        new(value * BytesInGigabyte);

    public static ByteSize FromTerabytes(double value) =>
        new(value * BytesInTerabyte);

    /// <summary>
    /// Converts the value of the current ByteSize object to a string.
    /// The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is
    /// the largest metric prefix such that the corresponding value is greater
    ///  than or equal to one.
    /// </summary>
    public readonly override string ToString() =>
        ToString(NumberFormatInfo.CurrentInfo);

    public readonly string ToString(IFormatProvider? provider)
    {
        provider ??= CultureInfo.CurrentCulture;

        return string.Format(provider, "{0:0.##} {1}", LargestWholeNumberValue, GetLargestWholeNumberSymbol(provider));
    }

    public readonly string ToString(string? format) =>
        ToString(format, NumberFormatInfo.CurrentInfo);

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
            format = "0.## " + format;
        }

        format = format.Replace("#.##", "0.##");

        var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;

        bool has(string s) => culture.CompareInfo.IndexOf(format, s, CompareOptions.IgnoreCase) != -1;
        string output(double n) => n.ToString(format, provider);

        var cultureFormatter = Configurator.GetFormatter(provider as CultureInfo);

        if (has(TerabyteSymbol))
        {
            format = format.Replace(TerabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Terabyte, Terabytes, toSymbol));
            return output(Terabytes);
        }

        if (has(GigabyteSymbol))
        {
            format = format.Replace(GigabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Gigabyte, Gigabytes, toSymbol));
            return output(Gigabytes);
        }

        if (has(MegabyteSymbol))
        {
            format = format.Replace(MegabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Megabyte, Megabytes, toSymbol));
            return output(Megabytes);
        }

        if (has(KilobyteSymbol))
        {
            format = format.Replace(KilobyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Kilobyte, Kilobytes, toSymbol));
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

        return $"{formattedLargeWholeNumberValue} {(toSymbol ? GetLargestWholeNumberSymbol(provider) : GetLargestWholeNumberFullWord(provider))}";
    }

    /// <summary>
    /// Converts the value of the current ByteSize object to a string with
    /// full words. The metric prefix symbol (bit, byte, kilo, mega, giga,
    /// tera) used is the largest metric prefix such that the corresponding
    /// value is greater than or equal to one.
    /// </summary>
    public readonly string ToFullWords(string? format = null, IFormatProvider? provider = null) =>
        ToString(format, provider, toSymbol: false);

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

        // Acquiring culture-specific parsing info
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

        if (sizePart.Length is not (1 or 2))
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

            case MegabyteSymbol:
                result = FromMegabytes(number);
                break;

            case GigabyteSymbol:
                result = FromGigabytes(number);
                break;

            case TerabyteSymbol:
                result = FromTerabytes(number);
                break;

            default:
                return false;
        }

        return true;
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
