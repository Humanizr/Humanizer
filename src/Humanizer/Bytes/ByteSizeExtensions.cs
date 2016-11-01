using System;
using Humanizer.Bytes;

// ReSharper disable once CheckNamespace
namespace Humanizer
{
    /// <summary>
    /// Provides extension methods for ByteSize
    /// </summary>
    public static class ByteSizeExtensions
    {
        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this byte input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this sbyte input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this short input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this ushort input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this int input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this uint input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bits(this long input)
        {
            return ByteSize.FromBits(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this byte input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this sbyte input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this short input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this ushort input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this int input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this uint input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this double input)
        {
            return ByteSize.FromBytes(input);
        }

        /// <summary>
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this long input)
        {
            return ByteSize.FromBytes(input);
        }

        #region Si-unit extensions (kilo, Mega, Giga, etc.)
        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this byte input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this sbyte input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this short input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this ushort input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this int input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this uint input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this double input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as kilobytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this long input)
        {
            return ByteSize.FromKilobytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this byte input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this sbyte input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this short input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this ushort input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this int input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this uint input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this double input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this long input)
        {
            return ByteSize.FromMegabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this byte input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this sbyte input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this short input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this ushort input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this int input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this uint input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this double input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as gigabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this long input)
        {
            return ByteSize.FromGigabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this byte input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this sbyte input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this short input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this ushort input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this int input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this uint input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this double input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this long input)
        {
            return ByteSize.FromTerabytes(input);
        }
        #endregion

        #region IEC-unit extensions (kibi, Mebi, Gibi, etc.)
        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this byte input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this sbyte input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this short input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this ushort input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this int input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this uint input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this double input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Kibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Kibibytes(this long input)
        {
            return ByteSize.FromKibibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this byte input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this sbyte input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this short input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this ushort input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this int input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this uint input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this double input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Mebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Mebibytes(this long input)
        {
            return ByteSize.FromMebibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this byte input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this sbyte input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this short input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this ushort input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this int input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this uint input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this double input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Gibibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Gibibytes(this long input)
        {
            return ByteSize.FromGibibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this byte input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this sbyte input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this short input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this ushort input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this int input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this uint input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this double input)
        {
            return ByteSize.FromTebibytes(input);
        }

        /// <summary>
        /// Considers input as Tebibytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Tebibytes(this long input)
        {
            return ByteSize.FromTebibytes(input);
        }
        #endregion

        /// <summary>
        /// Turns a byte quantity into human readable form, eg 2 GB
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format">The string format to use</param>
        /// <returns></returns>
        public static string Humanize(this ByteSize input, string format = null)
        {
            return string.IsNullOrWhiteSpace(format) ? input.ToString() : input.ToString(format);
        }

        /// <summary>
        /// Turns a quantity of bytes in a given interval into a rate that can be manipulated
        /// </summary>
        /// <param name="size">Quantity of bytes</param>
        /// <param name="interval">Interval to create rate for</param>
        /// <returns></returns>
        public static ByteRate Per(this ByteSize size, TimeSpan interval)
        {
            return new ByteRate(size, interval);
        }
    }
}
