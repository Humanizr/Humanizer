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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Humanizer.Bytes
{
    /// <summary>
    /// Represents a byte size value.
    /// </summary>
#pragma warning disable 1591
    public struct ByteSize : IComparable<ByteSize>, IEquatable<ByteSize>, IComparable
    {
        public static readonly ByteSize MinValue = FromBits(long.MinValue);
        public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

        public const long BitsInByte = 8;
        public static readonly long BytesInKilobyte = Base10Pow(3);
        public static readonly long BytesInMegabyte = Base10Pow(6);
        public static readonly long BytesInGigabyte = Base10Pow(9);
        public static readonly long BytesInTerabyte = Base10Pow(12);

        public static readonly long BytesInKibibyte = Base2Pow(10);
        public static readonly long BytesInMebibyte = Base2Pow(20);
        public static readonly long BytesInGibibyte = Base2Pow(30);
        public static readonly long BytesInTebibyte = Base2Pow(40);

        public const string BitSymbol = "b";
        public const string ByteSymbol = "B";

        public const string KilobyteSymbol = "kB";
        public const string MegabyteSymbol = "MB";
        public const string GigabyteSymbol = "GB";
        public const string TerabyteSymbol = "TB";

        public const string KibibyteSymbol = "kiB";
        public const string MebibyteSymbol = "MiB";
        public const string GibibyteSymbol = "GiB";
        public const string TebibyteSymbol = "TiB";

        private static readonly Dictionary<string, long> UppercasePrefixToMultiplicatorMap = new Dictionary<string, long>
        {
            {"", 1 },

            {"K", BytesInKilobyte },
            {"M", BytesInMegabyte },
            {"G", BytesInGigabyte },
            {"T", BytesInTerabyte },

            {"KI", BytesInKibibyte },
            {"MI", BytesInMebibyte },
            {"GI", BytesInGibibyte },
            {"TI", BytesInTebibyte },
        };
        private static long Base10Pow(int exponent)
        {
            return (long)Math.Pow(10, exponent);
        }
        private static long Base2Pow(int exponent)
        {
            return (long)Math.Pow(2, exponent);
        }

        public long Bits { get; }
        public double Bytes { get; }
        public double Kilobytes { get; }
        public double Megabytes { get; }
        public double Gigabytes { get; }
        public double Terabytes { get; }

        public string LargestWholeNumberSymbol
        {
            get
            {
                // Absolute value is used to deal with negative values
                if (Math.Abs(Terabytes) >= 1)
                    return TerabyteSymbol;

                if (Math.Abs(Gigabytes) >= 1)
                    return GigabyteSymbol;

                if (Math.Abs(Megabytes) >= 1)
                    return MegabyteSymbol;

                if (Math.Abs(Kilobytes) >= 1)
                    return KilobyteSymbol;

                if (Math.Abs(Bytes) >= 1)
                    return ByteSymbol;

                return BitSymbol;
            }
        }
        public double LargestWholeNumberValue
        {
            get
            {
                // Absolute value is used to deal with negative values
                if (Math.Abs(Terabytes) >= 1)
                    return Terabytes;

                if (Math.Abs(Gigabytes) >= 1)
                    return Gigabytes;

                if (Math.Abs(Megabytes) >= 1)
                    return Megabytes;

                if (Math.Abs(Kilobytes) >= 1)
                    return Kilobytes;

                if (Math.Abs(Bytes) >= 1)
                    return Bytes;

                return Bits;
            }
        }

        public ByteSize(double byteSize)
            : this()
        {
            // Get ceiling because bis are whole units
            Bits = (long)Math.Ceiling(byteSize * BitsInByte);

            Bytes = byteSize;
            Kilobytes = byteSize / BytesInKilobyte;
            Megabytes = byteSize / BytesInMegabyte;
            Gigabytes = byteSize / BytesInGigabyte;
            Terabytes = byteSize / BytesInTerabyte;
        }

        public static ByteSize FromBits(long value)
        {
            return new ByteSize(value / (double)BitsInByte);
        }

        public static ByteSize FromBytes(double value)
        {
            return new ByteSize(value);
        }

        public static ByteSize FromKilobytes(double value)
        {
            return new ByteSize(value * BytesInKilobyte);
        }

        public static ByteSize FromMegabytes(double value)
        {
            return new ByteSize(value * BytesInMegabyte);
        }

        public static ByteSize FromGigabytes(double value)
        {
            return new ByteSize(value * BytesInGigabyte);
        }

        public static ByteSize FromTerabytes(double value)
        {
            return new ByteSize(value * BytesInTerabyte);
        }

        /// <summary>
        /// Converts the value of the current ByteSize object to a string.
        /// The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is
        /// the largest metric prefix such that the corresponding value is greater
        ///  than or equal to one.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}", LargestWholeNumberValue, LargestWholeNumberSymbol);
        }

        public string ToString(string format)
        {
            if (!format.Contains("#") && !format.Contains("0"))
                format = "0.## " + format;

            Func<string, bool> has = s => format.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) != -1;
            Func<double, string> output = n => n.ToString(format);

            if (has(TerabyteSymbol))
                return output(Terabytes);
            if (has(GigabyteSymbol))
                return output(Gigabytes);
            if (has(MegabyteSymbol))
                return output(Megabytes);
            if (has(KilobyteSymbol))
                return output(Kilobytes);

            // Byte and Bit symbol look must be case-sensitive
            if (format.IndexOf(ByteSymbol, StringComparison.Ordinal) != -1)
                return output(Bytes);

            if (format.IndexOf(BitSymbol, StringComparison.Ordinal) != -1)
                return output(Bits);

            var formattedLargeWholeNumberValue = LargestWholeNumberValue.ToString(format);

            formattedLargeWholeNumberValue = formattedLargeWholeNumberValue.Equals(string.Empty)
                                              ? "0"
                                              : formattedLargeWholeNumberValue;

            return string.Format("{0} {1}", formattedLargeWholeNumberValue, LargestWholeNumberSymbol);
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            ByteSize other;
            if (value is ByteSize)
                other = (ByteSize)value;
            else
                return false;

            return Equals(other);
        }

        public bool Equals(ByteSize value)
        {
            return Bits == value.Bits;
        }

        public override int GetHashCode()
        {
            return Bits.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is ByteSize))
                throw new ArgumentException("Object is not a ByteSize");

            return CompareTo((ByteSize) obj);
        }

        public int CompareTo(ByteSize other)
        {
            return Bits.CompareTo(other.Bits);
        }

        public ByteSize Add(ByteSize bs)
        {
            return new ByteSize(Bytes + bs.Bytes);
        }

        public ByteSize AddBits(long value)
        {
            return this + FromBits(value);
        }

        public ByteSize AddBytes(double value)
        {
            return this + FromBytes(value);
        }

        public ByteSize AddKilobytes(double value)
        {
            return this + FromKilobytes(value);
        }

        public ByteSize AddMegabytes(double value)
        {
            return this + FromMegabytes(value);
        }

        public ByteSize AddGigabytes(double value)
        {
            return this + FromGigabytes(value);
        }

        public ByteSize AddTerabytes(double value)
        {
            return this + FromTerabytes(value);
        }

        public ByteSize Subtract(ByteSize bs)
        {
            return new ByteSize(Bytes - bs.Bytes);
        }

        public static ByteSize operator +(ByteSize b1, ByteSize b2)
        {
            return new ByteSize(b1.Bytes + b2.Bytes);
        }

        public static ByteSize operator ++(ByteSize b)
        {
            return new ByteSize(b.Bytes + 1);
        }

        public static ByteSize operator -(ByteSize b)
        {
            return new ByteSize(-b.Bytes);
        }

        public static ByteSize operator --(ByteSize b)
        {
            return new ByteSize(b.Bytes - 1);
        }

        public static bool operator ==(ByteSize b1, ByteSize b2)
        {
            return b1.Bits == b2.Bits;
        }

        public static bool operator !=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits != b2.Bits;
        }

        public static bool operator <(ByteSize b1, ByteSize b2)
        {
            return b1.Bits < b2.Bits;
        }

        public static bool operator <=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits <= b2.Bits;
        }

        public static bool operator >(ByteSize b1, ByteSize b2)
        {
            return b1.Bits > b2.Bits;
        }

        public static bool operator >=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits >= b2.Bits;
        }
        
        public static bool TryParse(string s, out ByteSize result)
        {
            result = new ByteSize();

            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException(nameof(s), "String is null or whitespace");

            s = s.TrimStart();
            var firstNonDigit = FindIndexOfFirstNonDigit(s);
            if (firstNonDigit < 0)
            {
                // If no unit is specified, we don't know how to parse the value
                // Although it would be reasonable to assume bytes
                return false;
            }

            var rawNumber = s.Substring(0, firstNonDigit);
            var rawUnit = s.Substring(firstNonDigit, s.Length - firstNonDigit).Trim();

            double value;
            if (!double.TryParse(rawNumber, out value))
                return false;

            var prefixAndUnit = SplitToPrefixAndUnit(rawUnit);

            if (prefixAndUnit == null)
                return false; // unable to determine unit or prefix

            var isByte = prefixAndUnit.Item2 == ByteSymbol;
            var uppercasePrefix = prefixAndUnit.Item1.ToUpperInvariant();

            // No fractional bits
            if (!isByte && value % 1 != 0)
                return false;

            long prefixMultiplicator;
            if (!UppercasePrefixToMultiplicatorMap
                    .TryGetValue(uppercasePrefix, out prefixMultiplicator))
                return false;

            result = isByte
                ? FromBytes(prefixMultiplicator * value)
                : FromBits(prefixMultiplicator * (long)value);

            return true;
        }

        private static Tuple<string, string> SplitToPrefixAndUnit(string unit)
        {
            if (unit.EndsWith(BitSymbol))
                return Tuple.Create(unit.Substring(0, unit.Length - 1).Trim(), BitSymbol);
            if (unit.EndsWith(ByteSymbol))
                return Tuple.Create(unit.Substring(0, unit.Length - 1).Trim(), ByteSymbol);
            
            return null;
        }

        private static int FindIndexOfFirstNonDigit(string s)
        {
            // Make the assumption that the decimal separator is always exactly one char explicit
            Debug.Assert(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Length == 1);

            var decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            for (var index = 0; index < s.Length; index++)
            {
                if (!(char.IsDigit(s[index]) || s[index] == decimalSeparator))
                    return index;
            }

            return -1;
        }

        public static ByteSize Parse(string s)
        {
            ByteSize result;

            if (TryParse(s, out result))
                return result;

            throw new FormatException("Value is not in the correct format");
        }
    }
}
#pragma warning restore 1591
