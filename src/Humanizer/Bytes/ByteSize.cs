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
        public const long BytesInKilobyte = 1024;
        public const long BytesInMegabyte = 1048576;
        public const long BytesInGigabyte = 1073741824;
        public const long BytesInTerabyte = 1099511627776;

        public const string BitSymbol = "b";
        public const string ByteSymbol = "B";
        public const string KilobyteSymbol = "KB";
        public const string MegabyteSymbol = "MB";
        public const string GigabyteSymbol = "GB";
        public const string TerabyteSymbol = "TB";

        public long Bits { get; private set; }
        public double Bytes { get; private set; }
        public double Kilobytes { get; private set; }
        public double Megabytes { get; private set; }
        public double Gigabytes { get; private set; }
        public double Terabytes { get; private set; }

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
                format = "#.## " + format;

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

            return string.Format("{0} {1}", LargestWholeNumberValue.ToString(format), LargestWholeNumberSymbol);
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
            // Arg checking
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException("s", "String is null or whitespace");

            // Setup the result
            result = new ByteSize();

            // Get the index of the first non-digit character
            s = s.TrimStart(); // Protect against leading spaces

            int num;
            var found = false;

            // Pick first non-digit number
            for (num = 0; num < s.Length; num++)
                if (!(char.IsDigit(s[num]) || s[num] == '.'))
                {
                    found = true;
                    break;
                }

            if (found == false)
                return false;

            int lastNumber = num;

            // Cut the input string in half
            string numberPart = s.Substring(0, lastNumber).Trim();
            string sizePart = s.Substring(lastNumber, s.Length - lastNumber).Trim();

            // Get the numeric part
            double number;
            if (!double.TryParse(numberPart, out number))
                return false;

            // Get the magnitude part
            switch (sizePart.ToUpper())
            {
                case ByteSymbol:
                    if (sizePart == BitSymbol)
                    { // Bits
                        if (number % 1 != 0) // Can't have partial bits
                            return false;

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

                case MegabyteSymbol:
                    result = FromMegabytes(number);
                    break;

                case GigabyteSymbol:
                    result = FromGigabytes(number);
                    break;

                case TerabyteSymbol:
                    result = FromTerabytes(number);
                    break;
            }

            return true;
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
