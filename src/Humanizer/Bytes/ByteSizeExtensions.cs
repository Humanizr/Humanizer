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
        public static ByteSize Bits(this long input)
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
        /// Considers input as bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this double input)
        {
            return ByteSize.FromBytes(input);
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
        /// Considers input as megabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this double input)
        {
            return ByteSize.FromMegabytes(input);
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
        /// Considers input as terabytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this double input)
        {
            return ByteSize.FromTerabytes(input);
        }

        /// <summary>
        /// Turns a byte quantity into human readable form, eg 2 GB
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format">The string format to use</param>
        /// <returns></returns>
        public static string Humanize(this ByteSize input, string format = null)
        {
            if (string.IsNullOrWhiteSpace(format))
                return input.ToString();

            return input.ToString(format);
        }
    }
}
