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

using static System.Globalization.NumberStyles;

namespace Humanizer
{
    /// <summary>
    /// Represents a byte size value.
    /// </summary>
#pragma warning disable 1591
    public struct ByteSize(double byteSize) :
        IComparable<ByteSize>,
        IEquatable<ByteSize>,
        IComparable,
        IFormattable
    {
        public static readonly ByteSize MinValue = FromBits(long.MinValue);
        public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

        public const long BitsInByte = 8;
        public const long BytesInKilobyte = 1000;
        public const long BytesInKibibyte = 1024;
        public const long BytesInMegabyte = 1_000_000;
        public const long BytesInMebibyte = 1_048_576;
        public const long BytesInGigabyte = 1_000_000_000;
        public const long BytesInGibibyte = 1_073_741_824;
        public const long BytesInTerabyte = 1_000_000_000_000;
        public const long BytesInTebibyte = 1_099_511_627_776;

        public const string BitSymbol = "b";
        public const string Bit = "bit";
        public const string ByteSymbol = "B";
        public const string Byte = "byte";
        public const string KilobyteSymbol = "KB";
        public const string KibibyteSymbol = "KiB";
        public const string Kilobyte = "kilobyte";
        public const string Kibibyte = "kibibyte";
        public const string MegabyteSymbol = "MB";
        public const string MebibyteSymbol = "MiB";
        public const string Megabyte = "megabyte";
        public const string Mebibyte = "mebibyte";
        public const string GigabyteSymbol = "GB";
        public const string GibibyteSymbol = "GiB";
        public const string Gigabyte = "gigabyte";
        public const string Gibibyte = "gibibyte";
        public const string TerabyteSymbol = "TB";
        public const string TebibyteSymbol = "TiB";
        public const string Terabyte = "terabyte";
        public const string Tebibyte = "tebibyte";

        public long Bits { get; } = (long)Math.Ceiling(byteSize * BitsInByte);
        public double Bytes { get; } = byteSize;
        public double Kilobytes { get; } = byteSize / BytesInKilobyte;
        public double Kibibytes { get; } = byteSize / BytesInKibibyte;
        public double Megabytes { get; } = byteSize / BytesInMegabyte;
        public double Mebibytes { get; } = byteSize / BytesInMebibyte;
        public double Gigabytes { get; } = byteSize / BytesInGigabyte;
        public double Gibibytes { get; } = byteSize / BytesInGibibyte;
        public double Terabytes { get; } = byteSize / BytesInTerabyte;
        public double Tebibytes { get; } = byteSize / BytesInTebibyte;

        public string LargestWholeNumberSymbol => GetLargestWholeNumberSymbol();

        public string GetLargestWholeNumberSymbol(IFormatProvider provider = null)
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

        public string LargestWholeNumberFullWord => GetLargestWholeNumberFullWord();

        public string GetLargestWholeNumberFullWord(IFormatProvider provider = null)
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

        public double LargestWholeNumberValue
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
            new(value / (double) BitsInByte);

        public static ByteSize FromBytes(double value) =>
            new(value);

        public static ByteSize FromKilobytes(double value) =>
            new(value * BytesInKilobyte);

        public static ByteSize FromKibibytes(double value) =>
            new(value * BytesInKibibyte);

        public static ByteSize FromMegabytes(double value) =>
            new(value * BytesInMegabyte);

        public static ByteSize FromMebibytes(double value) =>
            new(value * BytesInMebibyte);

        public static ByteSize FromGigabytes(double value) =>
            new(value * BytesInGigabyte);

        public static ByteSize FromGibibytes(double value) =>
            new(value * BytesInGibibyte);

        public static ByteSize FromTerabytes(double value) =>
            new(value * BytesInTerabyte);

        public static ByteSize FromTebibytes(double value) =>
            new(value * BytesInTebibyte);

        /// <summary>
        /// Converts the value of the current ByteSize object to a string.
        /// The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is
        /// the largest metric prefix such that the corresponding value is greater
        ///  than or equal to one.
        /// </summary>
        public override string ToString() =>
            ToString(NumberFormatInfo.CurrentInfo);

        public string ToString(IFormatProvider provider)
        {
            if (provider == null)
                provider = CultureInfo.CurrentCulture;

            return string.Format(provider, "{0:0.##} {1}", LargestWholeNumberValue, GetLargestWholeNumberSymbol(provider));
        }

        public string ToString(string format) =>
            ToString(format, NumberFormatInfo.CurrentInfo);

        public string ToString(string format, IFormatProvider provider) =>
            ToString(format, provider, toSymbol: true);

        string ToString(string format, IFormatProvider provider, bool toSymbol)
        {
            if (format == null)
                format = "G";

            if (provider == null)
                provider = CultureInfo.CurrentCulture;

            if (format == "G")
                format = "0.##";

            if (!format.Contains("#") && !format.Contains("0"))
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

            if (has(TebibyteSymbol))
            {
                format = format.Replace(TebibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Tebibyte, Tebibytes, toSymbol));
                return output(Tebibytes);
            }

            if (has(GigabyteSymbol))
            {
                format = format.Replace(GigabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Gigabyte, Gigabytes, toSymbol));
                return output(Gigabytes);
            }

            if (has(GibibyteSymbol))
            {
                format = format.Replace(GibibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Gibibyte, Gibibytes, toSymbol));
                return output(Gibibytes);
            }

            if (has(MegabyteSymbol))
            {
                format = format.Replace(MegabyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Megabyte, Megabytes, toSymbol));
                return output(Megabytes);
            }

            if (has(MebibyteSymbol))
            {
                format = format.Replace(MebibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Mebibyte, Mebibytes, toSymbol));
                return output(Mebibytes);
            }

            if (has(KilobyteSymbol))
            {
                format = format.Replace(KilobyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Kilobyte, Kilobytes, toSymbol));
                return output(Kilobytes);
            }

            if (has(KibibyteSymbol))
            {
                format = format.Replace(KibibyteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Kibibyte, Kibibytes, toSymbol));
                return output(Kibibytes);
            }

            // Byte and Bit symbol look must be case-sensitive
            if (format.IndexOf(ByteSymbol, StringComparison.Ordinal) != -1)
            {
                format = format.Replace(ByteSymbol, cultureFormatter.DataUnitHumanize(DataUnit.Byte, Bytes, toSymbol));
                return output(Bytes);
            }

            if (format.IndexOf(BitSymbol, StringComparison.Ordinal) != -1)
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
        public string ToFullWords(string format = null, IFormatProvider provider = null) =>
            ToString(format, provider, toSymbol: false);

        public override bool Equals(object value)
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

        public bool Equals(ByteSize value) =>
            Bits == value.Bits;

        public override int GetHashCode() =>
            Bits.GetHashCode();

        public int CompareTo(object obj)
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

        public int CompareTo(ByteSize other) =>
            Bits.CompareTo(other.Bits);

        public ByteSize Add(ByteSize bs) =>
            new(Bytes + bs.Bytes);

        public ByteSize AddBits(long value) =>
            this + FromBits(value);

        public ByteSize AddBytes(double value) =>
            this + FromBytes(value);

        public ByteSize AddKilobytes(double value) =>
            this + FromKilobytes(value);

        public ByteSize AddKibibytes(double value) =>
            this + FromKibibytes(value);

        public ByteSize AddMegabytes(double value) =>
            this + FromMegabytes(value);

        public ByteSize AddMebibytes(double value) =>
            this + FromMebibytes(value);

        public ByteSize AddGigabytes(double value) =>
            this + FromGigabytes(value);

        public ByteSize AddGibibytes(double value) =>
            this + FromGibibytes(value);

        public ByteSize AddTerabytes(double value) =>
            this + FromTerabytes(value);

        public ByteSize AddTebibytes(double value) =>
            this + FromTebibytes(value);

        public ByteSize Subtract(ByteSize bs) =>
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

        public static bool TryParse(string s, out ByteSize result) =>
            TryParse(s, null, out result);

        public static bool TryParse(string s, IFormatProvider formatProvider, out ByteSize result)
        {
            // Arg checking
            if (string.IsNullOrWhiteSpace(s))
            {
                result = default;
                return false;
            }

            // Acquiring culture-specific parsing info
            var numberFormat = GetNumberFormatInfo(formatProvider);

            const NumberStyles numberStyles = AllowDecimalPoint | AllowThousands | AllowLeadingSign;
            var numberSpecialChars = new[]
            {
                 Convert.ToChar(numberFormat.NumberDecimalSeparator),
                 Convert.ToChar(numberFormat.NumberGroupSeparator),
                 Convert.ToChar(numberFormat.PositiveSign),
                 Convert.ToChar(numberFormat.NegativeSign),
            };

            // Setup the result
            result = new();

            // Get the index of the first non-digit character
            s = s.TrimStart(); // Protect against leading spaces

            int num;
            var found = false;

            // Pick first non-digit number
            for (num = 0; num < s.Length; num++)
            {
                if (!(char.IsDigit(s[num]) || numberSpecialChars.Contains(s[num])))
                {
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                return false;
            }

            var lastNumber = num;

            // Cut the input string in half
            var numberPart = s.Substring(0, lastNumber).Trim();
            var sizePart = s.Substring(lastNumber, s.Length - lastNumber).Trim();

            // Get the numeric part
            if (!double.TryParse(numberPart, numberStyles, formatProvider, out var number))
            {
                return false;
            }

            // Get the magnitude part
            switch (sizePart.ToUpper())
            {
                case ByteSymbol:
                    if (sizePart == BitSymbol)
                    { // Bits
                        if (number % 1 != 0) // Can't have partial bits
                        {
                            return false;
                        }

                        result = FromBits((long)number);
                    }
                    else
                    { // Bytes
                        result = FromBytes(number);
                    }
                    break;

                case KilobyteSymbol:
                    result = FromKilobytes(number);
                    break;

                case KibibyteSymbol:
                    result = FromKibibytes(number);
                    break;

                case MegabyteSymbol:
                    result = FromMegabytes(number);
                    break;

                case MebibyteSymbol:
                    result = FromMebibytes(number);
                    break;

                case GigabyteSymbol:
                    result = FromGigabytes(number);
                    break;

                case GibibyteSymbol:
                    result = FromGibibytes(number);
                    break;

                case TerabyteSymbol:
                    result = FromTerabytes(number);
                    break;

                case TebibyteSymbol:
                    result = FromTebibytes(number);
                    break;

                default:
                    return false;
            }

            return true;
        }

        static NumberFormatInfo GetNumberFormatInfo(IFormatProvider formatProvider)
        {
            if (formatProvider is NumberFormatInfo numberFormat)
                return numberFormat;

            var culture = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;

            return culture.NumberFormat;
        }

        public static ByteSize Parse(string s) =>
            Parse(s, null);

        public static ByteSize Parse(string s, IFormatProvider formatProvider)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (TryParse(s, formatProvider, out var result))
            {
                return result;
            }

            throw new FormatException("Value is not in the correct format");
        }
    }
}
#pragma warning restore 1591
